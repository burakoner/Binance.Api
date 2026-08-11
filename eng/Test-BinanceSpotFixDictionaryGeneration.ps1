[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string] $WorkDirectory,

    [ValidateNotNullOrEmpty()]
    [string] $ToolCacheDirectory = (Join-Path ([Environment]::GetFolderPath("LocalApplicationData")) "Binance.Api\tool-cache"),

    [ValidateNotNullOrEmpty()]
    [string] $SchemaLockFile = (Join-Path $PSScriptRoot "binance-spot-fix-schema-lock.json"),

    [ValidateNotNullOrEmpty()]
    [string] $ToolchainLockFile = (Join-Path $PSScriptRoot "binance-spot-fix-ddtool-lock.json")
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
Add-Type -AssemblyName System.Net.Http

$utf8NoBom = [System.Text.UTF8Encoding]::new($false)
$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))

function Get-Sha256Hex([byte[]] $Bytes)
{
    $sha256 = [System.Security.Cryptography.SHA256]::Create()
    try
    {
        return ([System.BitConverter]::ToString($sha256.ComputeHash($Bytes))).Replace("-", "").ToLowerInvariant()
    }
    finally
    {
        $sha256.Dispose()
    }
}

function Assert-FileIntegrity([string] $Path, [long] $ExpectedSize, [string] $ExpectedSha256)
{
    $file = Get-Item -LiteralPath $Path
    if ($file.Length -ne $ExpectedSize)
    {
        throw "Size mismatch for '$Path': expected $ExpectedSize, received $($file.Length)."
    }

    $actualHash = (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant()
    if ($actualHash -cne $ExpectedSha256)
    {
        throw "SHA-256 mismatch for '$Path': expected $ExpectedSha256, received $actualHash."
    }
}

function Assert-OutsideRepository([string] $Path, [string] $Description)
{
    $repositoryPrefix = $repositoryRoot.TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar,
        [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar

    if ($Path -ieq $repositoryRoot -or
        $Path.StartsWith($repositoryPrefix, [System.StringComparison]::OrdinalIgnoreCase))
    {
        throw "$Description must be outside the repository worktree."
    }
}

function Ensure-CachedArtifact(
    [System.Net.Http.HttpClient] $HttpClient,
    [string] $Url,
    [string] $Destination,
    [long] $ExpectedSize,
    [string] $ExpectedSha256)
{
    if (Test-Path -LiteralPath $Destination)
    {
        Assert-FileIntegrity $Destination $ExpectedSize $ExpectedSha256
        return
    }

    $bytes = $HttpClient.GetByteArrayAsync($Url).GetAwaiter().GetResult()
    if ($bytes.LongLength -ne $ExpectedSize)
    {
        throw "Downloaded size mismatch for '$Url': expected $ExpectedSize, received $($bytes.LongLength)."
    }

    $actualHash = Get-Sha256Hex $bytes
    if ($actualHash -cne $ExpectedSha256)
    {
        throw "Downloaded SHA-256 mismatch for '$Url': expected $ExpectedSha256, received $actualHash."
    }

    [System.IO.File]::WriteAllBytes($Destination, $bytes)
}

function Invoke-NativeCaptured([string] $FilePath, [string[]] $Arguments)
{
    $savedErrorActionPreference = $ErrorActionPreference
    try
    {
        $ErrorActionPreference = "Continue"
        $records = @(& $FilePath @Arguments 2>&1)
        $exitCode = $LASTEXITCODE
    }
    finally
    {
        $ErrorActionPreference = $savedErrorActionPreference
    }

    return [pscustomobject]@{
        ExitCode = $exitCode
        Lines = @($records | ForEach-Object { $_.ToString() })
    }
}

function Get-SourceManifest([string] $Root, [string[]] $Directories)
{
    $resolvedRoot = (Resolve-Path -LiteralPath $Root).Path.TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar,
        [System.IO.Path]::AltDirectorySeparatorChar)
    $files = @($Directories | ForEach-Object {
        Get-ChildItem -LiteralPath (Join-Path $resolvedRoot $_) -Filter "*.cs" -File -Recurse
    })
    $entries = @($files | ForEach-Object {
        [pscustomobject]@{
            RelativePath = $_.FullName.Substring($resolvedRoot.Length + 1).Replace("\", "/")
            Size = [long] $_.Length
            Sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    } | Sort-Object RelativePath)

    return $entries
}

function Get-ManifestEvidence([object[]] $Entries)
{
    $lines = @($Entries | ForEach-Object { "$($_.RelativePath)`t$($_.Size)`t$($_.Sha256)" })
    $canonicalManifest = ($lines -join "`n") + "`n"
    return [pscustomobject]@{
        FileCount = $Entries.Count
        Bytes = [long] (($Entries | Measure-Object -Property Size -Sum).Sum)
        Sha256 = Get-Sha256Hex $utf8NoBom.GetBytes($canonicalManifest)
    }
}

function Assert-Manifest([object] $Evidence, [int] $ExpectedFiles, [long] $ExpectedBytes, [string] $ExpectedSha256, [string] $Description)
{
    if ($Evidence.FileCount -ne $ExpectedFiles -or
        $Evidence.Bytes -ne $ExpectedBytes -or
        $Evidence.Sha256 -cne $ExpectedSha256)
    {
        throw "$Description manifest mismatch: files=$($Evidence.FileCount), bytes=$($Evidence.Bytes), sha256=$($Evidence.Sha256)."
    }
}

function Get-TreeState([string] $Root)
{
    $resolvedRoot = (Resolve-Path -LiteralPath $Root).Path.TrimEnd("\")
    $state = @{}
    Get-ChildItem -LiteralPath $resolvedRoot -File -Recurse | ForEach-Object {
        $relativePath = $_.FullName.Substring($resolvedRoot.Length + 1).Replace("\", "/")
        $state[$relativePath] = "$($_.Length):$((Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant())"
    }
    return $state
}

function Get-ChangedPaths([hashtable] $Before, [hashtable] $After)
{
    $allPaths = @($Before.Keys) + @($After.Keys) | Sort-Object -Unique
    return @($allPaths | Where-Object {
        -not $Before.ContainsKey($_) -or -not $After.ContainsKey($_) -or $Before[$_] -cne $After[$_]
    })
}

function Assert-ExactStrings([string[]] $Actual, [string[]] $Expected, [string] $Description)
{
    [string[]] $actualSorted = @($Actual)
    [string[]] $expectedSorted = @($Expected)
    [System.Array]::Sort($actualSorted, [System.StringComparer]::Ordinal)
    [System.Array]::Sort($expectedSorted, [System.StringComparer]::Ordinal)
    if (($actualSorted -join "`n") -cne ($expectedSorted -join "`n"))
    {
        throw "$Description mismatch. Expected [$($expectedSorted -join ', ')], received [$($actualSorted -join ', ')]."
    }
}

function Write-AdaptedDictionary([string] $Source, [string] $Destination, [string] $CustomName)
{
    if ($CustomName -cnotmatch '^[A-Za-z][A-Za-z0-9]+$')
    {
        throw "Unsafe DDTool custom name '$CustomName'."
    }

    $raw = [System.IO.File]::ReadAllText($Source)
    $marker = "<fix "
    if (($raw.Length - $raw.Replace($marker, "").Length) / $marker.Length -ne 1)
    {
        throw "Expected exactly one FIX root marker in '$Source'."
    }

    [System.IO.File]::WriteAllText(
        $Destination,
        $raw.Replace($marker, "<fix customname='$CustomName' "),
        $utf8NoBom)
}

function Copy-RawSources([string] $QuickFixRoot, [string] $OutputRoot, [string] $Destination, [object[]] $Dictionaries)
{
    $fieldsDirectory = Join-Path $Destination "Fields"
    [System.IO.Directory]::CreateDirectory($fieldsDirectory) | Out-Null
    foreach ($fieldFile in @("Fields.cs", "FieldTags.cs"))
    {
        [System.IO.File]::Copy(
            (Join-Path $QuickFixRoot "Fields\$fieldFile"),
            (Join-Path $fieldsDirectory $fieldFile),
            $true)
    }

    foreach ($dictionary in $Dictionaries)
    {
        $roleDirectory = Join-Path $Destination $dictionary.role
        [System.IO.Directory]::CreateDirectory($roleDirectory) | Out-Null
        $generatedDirectory = Join-Path $OutputRoot "Messages\$($dictionary.customName)"
        $generatedSources = @(Get-ChildItem -LiteralPath $generatedDirectory -Filter "*.cs" -File)
        if ($generatedSources.Count -ne [int] $dictionary.generatedSourceFileCount)
        {
            throw "Unexpected generated source count for $($dictionary.role): $($generatedSources.Count)."
        }

        $expectedNames = @($dictionary.messageNames | ForEach-Object { "$_.cs" }) + @("Message.cs", "MessageFactory.cs")
        Assert-ExactStrings @($generatedSources.Name) $expectedNames "$($dictionary.role) generated filenames"
        foreach ($source in $generatedSources)
        {
            [System.IO.File]::Copy($source.FullName, (Join-Path $roleDirectory $source.Name), $true)
        }
    }
}

function Invoke-DictionaryGeneration(
    [string] $ToolAssembly,
    [string] $SourceRoot,
    [string] $OutputRoot,
    [string[]] $DictionaryPaths,
    [object] $GenerationLock)
{
    [System.IO.Directory]::CreateDirectory($OutputRoot) | Out-Null
    $quickFixRoot = Join-Path $SourceRoot "QuickFIXn"
    $before = Get-TreeState $quickFixRoot
    $arguments = @($ToolAssembly, "--reporoot", $SourceRoot, "--outputdir", $OutputRoot) + $DictionaryPaths
    $result = Invoke-NativeCaptured "dotnet" $arguments
    if ($result.ExitCode -ne 0)
    {
        throw "DDTool failed with exit code $($result.ExitCode):`n$($result.Lines -join [Environment]::NewLine)"
    }
    if ($result.Lines -match '^Errors found\.\s+Code generation')
    {
        throw "DDTool reported validation errors despite returning success:`n$($result.Lines -join [Environment]::NewLine)"
    }

    $after = Get-TreeState $quickFixRoot
    Assert-ExactStrings (Get-ChangedPaths $before $after) @($GenerationLock.expectedQuickFixSourceMutations) "DDTool QuickFIX/n source mutations"

    $generatedProjects = @(Get-ChildItem -LiteralPath $OutputRoot -Filter "*.csproj" -File -Recurse)
    if ($generatedProjects.Count -ne 2)
    {
        throw "Expected two disposable DDTool project files, found $($generatedProjects.Count)."
    }
}

function Write-InternalizedSources([string] $RawRoot, [string] $Destination, [object[]] $Dictionaries, [object] $GenerationLock)
{
    foreach ($directory in @("Fields", "OrderEntry", "MarketData"))
    {
        [System.IO.Directory]::CreateDirectory((Join-Path $Destination $directory)) | Out-Null
    }

    $fields = [System.IO.File]::ReadAllText((Join-Path $RawRoot "Fields\Fields.cs"))
    if ([regex]::Matches($fields, '(?m)^public sealed class ').Count -ne [int] $GenerationLock.internalFieldAccessibilityReplacements)
    {
        throw "Unexpected generated field accessibility surface."
    }
    $fields = $fields.Replace(
        "using System;",
        "using System;`r`nusing QuickFix.Fields;`r`nusing SeqNumType = System.UInt64;`r`nusing SeqNumFieldType = QuickFix.Fields.ULongField;")
    $fields = $fields.Replace("namespace QuickFix.Fields;", "namespace Binance.FIX.Api.Internal.Fields;")
    $fields = $fields.Replace("Converters.TimeStampPrecision", "QuickFix.Fields.Converters.TimeStampPrecision")
    $fields = [regex]::Replace($fields, '(?m)^public sealed class ', 'internal sealed class ')
    [System.IO.File]::WriteAllText((Join-Path $Destination "Fields\Fields.cs"), $fields, $utf8NoBom)

    $tags = [System.IO.File]::ReadAllText((Join-Path $RawRoot "Fields\FieldTags.cs"))
    if ([regex]::Matches($tags, '(?m)^public static class Tags').Count -ne [int] $GenerationLock.internalTagAccessibilityReplacements)
    {
        throw "Unexpected generated tag accessibility surface."
    }
    $tags = $tags.Replace("namespace QuickFix.Fields;", "namespace Binance.FIX.Api.Internal.Fields;")
    $tags = $tags.Replace("public static class Tags", "internal static class Tags")
    [System.IO.File]::WriteAllText((Join-Path $Destination "Fields\FieldTags.cs"), $tags, $utf8NoBom)

    $messageReplacements = 0
    foreach ($dictionary in $Dictionaries)
    {
        $sourceDirectory = Join-Path $RawRoot $dictionary.role
        $destinationDirectory = Join-Path $Destination $dictionary.role
        foreach ($source in Get-ChildItem -LiteralPath $sourceDirectory -Filter "*.cs" -File)
        {
            $content = [System.IO.File]::ReadAllText($source.FullName)
            $upstreamNamespace = "QuickFix.$($dictionary.customName)"
            if (-not $content.Contains($upstreamNamespace))
            {
                throw "Expected namespace '$upstreamNamespace' in '$($source.FullName)'."
            }
            $content = $content.Replace($upstreamNamespace, [string] $dictionary.generatedNamespace)
            $content = $content.Replace("using QuickFix.Fields;", "using Binance.FIX.Api.Internal.Fields;")
            $content = $content.Replace("QuickFix.Fields.Tags.", "Binance.FIX.Api.Internal.Fields.Tags.")
            $content = $content.Replace("QuickFix.Fields.BeginString(", "Binance.FIX.Api.Internal.Fields.BeginString(")

            $matches = [regex]::Matches($content, '(?m)^public (?=(?:abstract )?class )')
            if ($matches.Count -ne 1)
            {
                throw "Expected one top-level public class in '$($source.FullName)', found $($matches.Count)."
            }
            $messageReplacements += $matches.Count
            $content = [regex]::Replace($content, '(?m)^public (?=(?:abstract )?class )', 'internal ')
            if ($content.Contains("using QuickFix;"))
            {
                throw "Unexpected pre-existing QuickFix namespace import in '$($source.FullName)'."
            }
            $fieldUsing = "using Binance.FIX.Api.Internal.Fields;`r`n"
            if ($content.Contains($fieldUsing))
            {
                $content = $content.Replace($fieldUsing, "$fieldUsing" + "using QuickFix;`r`n")
            }
            else
            {
                $namespacePattern = [regex]::new('(?m)^namespace ')
                if ($namespacePattern.Matches($content).Count -ne 1)
                {
                    throw "Expected one namespace declaration in '$($source.FullName)'."
                }
                $content = $namespacePattern.Replace($content, "using QuickFix;`r`n`r`nnamespace ", 1)
            }
            [System.IO.File]::WriteAllText((Join-Path $destinationDirectory $source.Name), $content, $utf8NoBom)
        }
    }

    if ($messageReplacements -ne [int] $GenerationLock.internalMessageAccessibilityReplacements)
    {
        throw "Unexpected message accessibility replacement count: $messageReplacements."
    }
}

if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT)
{
    throw "This manifest is intentionally locked to Windows because DDTool uses host line endings."
}

$resolvedWorkDirectory = [System.IO.Path]::GetFullPath($WorkDirectory)
$resolvedToolCacheDirectory = [System.IO.Path]::GetFullPath($ToolCacheDirectory)
Assert-OutsideRepository $resolvedWorkDirectory "Work directory"
Assert-OutsideRepository $resolvedToolCacheDirectory "Tool cache directory"
if (Test-Path -LiteralPath $resolvedWorkDirectory)
{
    if (@(Get-ChildItem -LiteralPath $resolvedWorkDirectory -Force).Count -ne 0)
    {
        throw "Work directory '$resolvedWorkDirectory' must be empty."
    }
}
else
{
    [System.IO.Directory]::CreateDirectory($resolvedWorkDirectory) | Out-Null
}
[System.IO.Directory]::CreateDirectory($resolvedToolCacheDirectory) | Out-Null

$toolLock = Get-Content -Raw -LiteralPath ([System.IO.Path]::GetFullPath($ToolchainLockFile)) | ConvertFrom-Json -ErrorAction Stop
if ($toolLock.artifactsRedistributed -ne $false -or $toolLock.generatedArtifactsCommitted -ne $false)
{
    throw "The text FIX generation lock must remain non-redistributing and non-committing before Review 33."
}
if ($toolLock.hostOperatingSystem -cne "Windows")
{
    throw "Unexpected generation host lock '$($toolLock.hostOperatingSystem)'."
}

$sdkVersionResult = Invoke-NativeCaptured "dotnet" @("--version")
if ($sdkVersionResult.ExitCode -ne 0 -or $sdkVersionResult.Lines.Count -ne 1)
{
    throw "Unable to determine the .NET SDK version."
}
$sdkMajor = [int] ($sdkVersionResult.Lines[0].Split('.')[0])
if ($sdkMajor -lt [int] $toolLock.minimumDotnetSdkMajor)
{
    throw ".NET SDK $($toolLock.minimumDotnetSdkMajor) or later is required to build DDTool 1.14.1."
}

$sourceArchive = Join-Path $resolvedToolCacheDirectory "quickfixn-$($toolLock.quickFixN.sourceCommit).zip"
$corePackage = Join-Path $resolvedToolCacheDirectory "quickfixn.core.$($toolLock.quickFixN.version).nupkg"
$httpClient = [System.Net.Http.HttpClient]::new()
try
{
    Ensure-CachedArtifact $httpClient $toolLock.quickFixN.sourceArchive.url $sourceArchive `
        ([long] $toolLock.quickFixN.sourceArchive.size) $toolLock.quickFixN.sourceArchive.sha256
    Ensure-CachedArtifact $httpClient $toolLock.quickFixN.corePackage.url $corePackage `
        ([long] $toolLock.quickFixN.corePackage.size) $toolLock.quickFixN.corePackage.sha256
}
finally
{
    $httpClient.Dispose()
}

$schemaDirectory = Join-Path $resolvedWorkDirectory "schemas"
& (Join-Path $PSScriptRoot "Download-BinanceSpotFixSchemas.ps1") `
    -OutputDirectory $schemaDirectory `
    -LockFile ([System.IO.Path]::GetFullPath($SchemaLockFile))

$dictionaryDirectory = Join-Path $resolvedWorkDirectory "dictionaries"
[System.IO.Directory]::CreateDirectory($dictionaryDirectory) | Out-Null
$allFieldsByName = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::Ordinal)
$allFieldsByTag = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::Ordinal)
$sharedFieldNames = [System.Collections.Generic.Dictionary[string, bool]]::new([System.StringComparer]::Ordinal)
$seenFieldNames = [System.Collections.Generic.Dictionary[string, bool]]::new([System.StringComparer]::Ordinal)
$enumDescriptions = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::Ordinal)
$enumValues = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::Ordinal)
$dictionaryPaths = @()
foreach ($dictionary in $toolLock.dictionaries)
{
    $schemaPath = Join-Path $schemaDirectory $dictionary.schemaFileName
    [xml] $xml = [System.IO.File]::ReadAllText($schemaPath)
    if ([string] $xml.fix.major -cne "4" -or [string] $xml.fix.minor -cne "4" -or [string] $xml.fix.type -cne "FIX")
    {
        throw "$($dictionary.schemaFileName) is not the reviewed FIX 4.4 dictionary."
    }
    $fields = @($xml.fix.fields.field)
    $messages = @($xml.fix.messages.message)
    if ($fields.Count -ne [int] $dictionary.fieldCount -or $messages.Count -ne [int] $dictionary.messageCount)
    {
        throw "$($dictionary.role) dictionary inventory changed."
    }
    Assert-ExactStrings @($messages | ForEach-Object { [string] $_.name }) @($dictionary.messageNames) "$($dictionary.role) message names"
    Assert-ExactStrings @($messages | ForEach-Object { [string] $_.msgtype }) @($dictionary.messageTypes) "$($dictionary.role) message types"

    $currentNames = [System.Collections.Generic.Dictionary[string, bool]]::new([System.StringComparer]::Ordinal)
    foreach ($field in $fields)
    {
        $name = [string] $field.name
        $tag = [string] $field.number
        $type = [string] $field.type
        if ($currentNames.ContainsKey($name))
        {
            throw "Duplicate field name '$name' in $($dictionary.schemaFileName)."
        }
        $currentNames[$name] = $true
        $nameSignature = "$tag|$type"
        $tagSignature = "$name|$type"
        if ($allFieldsByName.ContainsKey($name) -and $allFieldsByName[$name] -cne $nameSignature)
        {
            throw "Conflicting definition for field '$name'."
        }
        if ($allFieldsByTag.ContainsKey($tag) -and $allFieldsByTag[$tag] -cne $tagSignature)
        {
            throw "Conflicting definition for field tag '$tag'."
        }
        if ($seenFieldNames.ContainsKey($name))
        {
            $sharedFieldNames[$name] = $true
        }
        $seenFieldNames[$name] = $true
        $allFieldsByName[$name] = $nameSignature
        $allFieldsByTag[$tag] = $tagSignature

        foreach ($enum in @($field.SelectNodes("value")))
        {
            $enumDescription = $enum.GetAttribute("description")
            $enumValue = $enum.GetAttribute("enum")
            if ([string]::IsNullOrEmpty($enumDescription) -or [string]::IsNullOrEmpty($enumValue))
            {
                throw "Incomplete enum definition for field '$name'."
            }
            $descriptionKey = "$name|$enumDescription"
            $valueKey = "$name|$enumValue"
            if ($enumDescriptions.ContainsKey($descriptionKey) -and $enumDescriptions[$descriptionKey] -cne $enumValue)
            {
                throw "Conflicting enum description '$descriptionKey'."
            }
            if ($enumValues.ContainsKey($valueKey) -and $enumValues[$valueKey] -cne $enumDescription)
            {
                throw "Conflicting enum value '$valueKey'."
            }
            $enumDescriptions[$descriptionKey] = $enumValue
            $enumValues[$valueKey] = $enumDescription
        }
    }

    $adaptedPath = Join-Path $dictionaryDirectory $dictionary.schemaFileName
    Write-AdaptedDictionary $schemaPath $adaptedPath $dictionary.customName
    $dictionaryPaths += $adaptedPath
}
if ($allFieldsByName.Count -ne [int] $toolLock.generation.unionFieldCount -or
    $allFieldsByTag.Count -ne [int] $toolLock.generation.unionFieldCount -or
    $sharedFieldNames.Count -ne [int] $toolLock.generation.sharedFieldCount)
{
    throw "Combined dictionary field inventory changed."
}

$runRoots = @()
foreach ($runName in @("run-a", "run-b"))
{
    $runRoot = Join-Path $resolvedWorkDirectory $runName
    [System.IO.Directory]::CreateDirectory($runRoot) | Out-Null
    [System.IO.Compression.ZipFile]::ExtractToDirectory($sourceArchive, $runRoot)
    $sourceDirectories = @(Get-ChildItem -LiteralPath $runRoot -Directory)
    if ($sourceDirectories.Count -ne 1 -or $sourceDirectories[0].Name -cne "quickfixn-$($toolLock.quickFixN.sourceCommit)")
    {
        throw "Unexpected QuickFIX/n source archive root."
    }
    $runRoots += $sourceDirectories[0].FullName
}

$toolProject = Join-Path $runRoots[0] $toolLock.quickFixN.ddToolProject.Replace("/", "\")
$toolBuild = Invoke-NativeCaptured "dotnet" @("build", $toolProject, "--configuration", "Release", "--nologo", "--verbosity", "minimal")
if ($toolBuild.ExitCode -ne 0)
{
    throw "DDTool build failed:`n$($toolBuild.Lines -join [Environment]::NewLine)"
}
$toolAssembly = Join-Path (Split-Path -Parent $toolProject) "bin\Release\$($toolLock.quickFixN.ddToolTargetFramework)\DDTool.dll"
if (-not (Test-Path -LiteralPath $toolAssembly))
{
    throw "DDTool assembly was not produced at '$toolAssembly'."
}

$rawEvidence = @()
$adaptedEvidence = @()
for ($index = 0; $index -lt 2; $index++)
{
    $outputRoot = Join-Path $resolvedWorkDirectory "generated-$index"
    Invoke-DictionaryGeneration $toolAssembly $runRoots[$index] $outputRoot $dictionaryPaths $toolLock.generation

    $rawRoot = Join-Path $resolvedWorkDirectory "raw-$index"
    Copy-RawSources (Join-Path $runRoots[$index] "QuickFIXn") $outputRoot $rawRoot $toolLock.dictionaries
    $rawManifest = @(Get-SourceManifest $rawRoot @("Fields", "OrderEntry", "MarketData"))
    $raw = Get-ManifestEvidence $rawManifest
    Assert-Manifest $raw ([int] $toolLock.generation.rawSourceFileCount) `
        ([long] $toolLock.generation.rawSourceBytes) $toolLock.generation.rawSourceManifestSha256 "Raw DDTool output"
    $rawEvidence += $raw

    $adaptedRoot = Join-Path $resolvedWorkDirectory "adapted-$index"
    Write-InternalizedSources $rawRoot $adaptedRoot $toolLock.dictionaries $toolLock.generation
    $adaptedManifest = @(Get-SourceManifest $adaptedRoot @("Fields", "OrderEntry", "MarketData"))
    $adapted = Get-ManifestEvidence $adaptedManifest
    Assert-Manifest $adapted ([int] $toolLock.generation.adaptedSourceFileCount) `
        ([long] $toolLock.generation.adaptedSourceBytes) $toolLock.generation.adaptedSourceManifestSha256 "Adapted DDTool output"
    $adaptedEvidence += $adapted
}
if ($rawEvidence[0].Sha256 -cne $rawEvidence[1].Sha256 -or
    $adaptedEvidence[0].Sha256 -cne $adaptedEvidence[1].Sha256)
{
    throw "Independent dictionary generations are not deterministic."
}

$packageRoot = Join-Path $resolvedWorkDirectory "quickfixn-core"
[System.IO.Compression.ZipFile]::ExtractToDirectory($corePackage, $packageRoot)
$nuspec = [System.IO.File]::ReadAllText((Join-Path $packageRoot "quickfixn.core.nuspec"))
if (-not $nuspec.Contains("commit=`"$($toolLock.quickFixN.corePackage.repositoryCommit)`"") -or
    -not [System.IO.File]::ReadAllText((Join-Path $packageRoot "LICENSE")).StartsWith($toolLock.quickFixN.license))
{
    throw "QuickFIXn.Core package provenance or license changed."
}
$coreAssemblies = @{}
foreach ($assembly in $toolLock.quickFixN.corePackage.assemblies)
{
    $path = Join-Path $packageRoot $assembly.path.Replace("/", "\")
    Assert-FileIntegrity $path ([long] $assembly.size) $assembly.sha256
    $coreAssemblies[$assembly.targetFramework] = $path
}

$compiledRoot = Join-Path $resolvedWorkDirectory "adapted-0"
$net8AssemblyPath = [System.Security.SecurityElement]::Escape($coreAssemblies["net8.0"])
$net10AssemblyPath = [System.Security.SecurityElement]::Escape($coreAssemblies["net10.0"])
$targetFrameworks = @($toolLock.compileTargetFrameworks) -join ";"
$compileProject = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>$targetFrameworks</TargetFrameworks>
    <AssemblyName>Binance.FIX.Api.GeneratedSpike</AssemblyName>
    <Nullable>enable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Fields/**/*.cs" />
    <Compile Include="OrderEntry/**/*.cs" />
    <Compile Include="MarketData/**/*.cs" />
  </ItemGroup>
  <ItemGroup Condition="'`$(TargetFramework)' == 'net8.0' Or '`$(TargetFramework)' == 'net9.0'">
    <Reference Include="QuickFix"><HintPath>$net8AssemblyPath</HintPath><Private>true</Private></Reference>
  </ItemGroup>
  <ItemGroup Condition="'`$(TargetFramework)' == 'net10.0'">
    <Reference Include="QuickFix"><HintPath>$net10AssemblyPath</HintPath><Private>true</Private></Reference>
  </ItemGroup>
</Project>
"@
$compileProjectPath = Join-Path $compiledRoot "Generated.csproj"
[System.IO.File]::WriteAllText($compileProjectPath, $compileProject, $utf8NoBom)
$compile = Invoke-NativeCaptured "dotnet" @("build", $compileProjectPath, "--configuration", "Release", "--nologo", "--verbosity", "minimal", "--force")
if ($compile.ExitCode -ne 0)
{
    throw "Generated dictionary compile failed:`n$($compile.Lines -join [Environment]::NewLine)"
}

$inspectorRoot = Join-Path $resolvedWorkDirectory "inspector"
[System.IO.Directory]::CreateDirectory($inspectorRoot) | Out-Null
$inspectorProject = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
</Project>
'@
$inspectorSource = @'
using System;
using System.Linq;
using System.Reflection;

if (args.Length != 8)
    throw new ArgumentException("Expected assembly path and seven locked evidence arguments.");

var assembly = Assembly.LoadFrom(args[0]);
var expectedDefinedTypes = int.Parse(args[1]);
var expectedFieldTypes = int.Parse(args[2]);
var expectedVersion = Version.Parse(args[3]);
var expectedOrderNames = args[4].Split(',', StringSplitOptions.RemoveEmptyEntries);
var expectedOrderTypes = args[5].Split(',', StringSplitOptions.RemoveEmptyEntries);
var expectedMarketNames = args[6].Split(',', StringSplitOptions.RemoveEmptyEntries);
var expectedMarketTypes = args[7].Split(',', StringSplitOptions.RemoveEmptyEntries);

if (assembly.GetExportedTypes().Length != 0)
    throw new InvalidOperationException("Generated assembly exports types.");
var types = assembly.GetTypes();
if (types.Length != expectedDefinedTypes)
    throw new InvalidOperationException($"Defined type count changed: {types.Length}.");
var fieldTypes = types.Where(type => type.Namespace == "Binance.FIX.Api.Internal.Fields").ToArray();
if (fieldTypes.Length != expectedFieldTypes)
    throw new InvalidOperationException($"Internal field namespace type count changed: {fieldTypes.Length}.");
if (types.Any(type => type.Namespace == "QuickFix.Fields" ||
                      type.Namespace?.StartsWith("QuickFix.BinanceSpot", StringComparison.Ordinal) == true))
    throw new InvalidOperationException("Generated types leaked into a QuickFIX/n-owned namespace.");

static bool DerivesFrom(Type type, string expectedBaseType)
{
    for (var current = type.BaseType; current is not null; current = current.BaseType)
        if (current.FullName == expectedBaseType)
            return true;
    return false;
}

static void AssertRole(Type[] allTypes, string roleNamespace, string[] expectedNames, string[] expectedMessageTypes)
{
    var messages = allTypes
        .Where(type => type.Namespace == roleNamespace && !type.IsAbstract && DerivesFrom(type, "QuickFix.Message"))
        .OrderBy(type => type.Name, StringComparer.Ordinal)
        .ToArray();
    if (!messages.Select(type => type.Name).SequenceEqual(expectedNames.OrderBy(value => value, StringComparer.Ordinal)))
        throw new InvalidOperationException($"{roleNamespace} message names changed.");
    var messageTypes = messages
        .Select(type => (string)(type.GetField("MsgType", BindingFlags.Public | BindingFlags.Static)?.GetRawConstantValue()
            ?? throw new InvalidOperationException($"{type.FullName} has no MsgType constant.")))
        .OrderBy(value => value, StringComparer.Ordinal)
        .ToArray();
    if (!messageTypes.SequenceEqual(expectedMessageTypes.OrderBy(value => value, StringComparer.Ordinal)))
        throw new InvalidOperationException($"{roleNamespace} message types changed.");
    foreach (var message in messages)
        _ = Activator.CreateInstance(message, nonPublic: true)
            ?? throw new InvalidOperationException($"Could not construct {message.FullName}.");
    var factory = allTypes.Single(type => type.FullName == roleNamespace + ".MessageFactory");
    if (!factory.GetInterfaces().Any(type => type.FullName == "QuickFix.IMessageFactory"))
        throw new InvalidOperationException($"{roleNamespace} factory does not implement IMessageFactory.");
}

AssertRole(types, "Binance.FIX.Api.Internal.OrderEntry", expectedOrderNames, expectedOrderTypes);
AssertRole(types, "Binance.FIX.Api.Internal.MarketData", expectedMarketNames, expectedMarketTypes);
var quickFixReference = assembly.GetReferencedAssemblies().Single(reference => reference.Name == "QuickFix");
if (quickFixReference.Version != expectedVersion)
    throw new InvalidOperationException($"QuickFix reference version changed: {quickFixReference.Version}.");
Console.WriteLine($"types={types.Length}; exported=0; fields={fieldTypes.Length}; QuickFix={quickFixReference.Version}");
'@
[System.IO.File]::WriteAllText((Join-Path $inspectorRoot "Inspector.csproj"), $inspectorProject, $utf8NoBom)
[System.IO.File]::WriteAllText((Join-Path $inspectorRoot "Program.cs"), $inspectorSource, $utf8NoBom)
$inspectorBuild = Invoke-NativeCaptured "dotnet" @("build", (Join-Path $inspectorRoot "Inspector.csproj"), "--configuration", "Release", "--nologo", "--verbosity", "minimal")
if ($inspectorBuild.ExitCode -ne 0)
{
    throw "Generated assembly inspector build failed:`n$($inspectorBuild.Lines -join [Environment]::NewLine)"
}

$inspectorAssembly = Join-Path $inspectorRoot "bin\Release\net10.0\Inspector.dll"
$orderEntry = @($toolLock.dictionaries | Where-Object role -eq "OrderEntry")
$marketData = @($toolLock.dictionaries | Where-Object role -eq "MarketData")
if ($orderEntry.Count -ne 1 -or $marketData.Count -ne 1)
{
    throw "Expected one Order Entry and one Market Data dictionary lock."
}
foreach ($framework in $toolLock.compileTargetFrameworks)
{
    $generatedAssembly = Join-Path $compiledRoot "bin\Release\$framework\Binance.FIX.Api.GeneratedSpike.dll"
    $inspection = Invoke-NativeCaptured "dotnet" @(
        $inspectorAssembly,
        $generatedAssembly,
        [string] $toolLock.expectedDefinedTypeCount,
        [string] $toolLock.expectedInternalFieldNamespaceTypeCount,
        [string] $toolLock.expectedReferencedQuickFixAssemblyVersion,
        (@($orderEntry[0].messageNames) -join ','),
        (@($orderEntry[0].messageTypes) -join ','),
        (@($marketData[0].messageNames) -join ','),
        (@($marketData[0].messageTypes) -join ','))
    if ($inspection.ExitCode -ne 0)
    {
        throw "Generated assembly inspection failed for ${framework}:`n$($inspection.Lines -join [Environment]::NewLine)"
    }
}

Write-Output (
    "Verified QuickFIX/n $($toolLock.quickFixN.version) DDTool generation: " +
    "$($toolLock.generation.rawSourceFileCount) raw and internalized C# files, " +
    "two deterministic manifests, $($toolLock.generation.unionFieldCount) fields, " +
    "$($orderEntry[0].messageCount)+$($marketData[0].messageCount) role messages, " +
    "$(@($toolLock.compileTargetFrameworks).Count) warning-free targets, zero exported types.")
