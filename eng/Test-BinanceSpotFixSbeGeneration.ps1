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
    [string] $ToolchainLockFile = (Join-Path $PSScriptRoot "binance-spot-fix-sbe-toolchain-lock.json")
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
Add-Type -AssemblyName System.Net.Http

$utf8NoBom = [System.Text.UTF8Encoding]::new($false)
$topLevelPublicPattern = '(?m)^    public (?=(?:(?:sealed|unsafe|partial|readonly|static|abstract)\s+)*(?:class|struct|enum|interface|record)\s+)'

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

function Assert-OutsideRepository([string] $Path, [string] $RepositoryRoot, [string] $Description)
{
    $repositoryPrefix = $RepositoryRoot.TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar,
        [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar

    if ($Path -ieq $RepositoryRoot -or
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

function Get-SourceManifest([string] $Root)
{
    $relativePaths = @(Get-ChildItem -LiteralPath $Root -Filter "*.cs" -File -Recurse |
        ForEach-Object { $_.FullName.Substring($Root.Length + 1).Replace("\", "/") })
    [System.Array]::Sort($relativePaths, [System.StringComparer]::Ordinal)

    $entries = @()
    foreach ($relativePath in $relativePaths)
    {
        $fullPath = Join-Path $Root $relativePath.Replace("/", "\")
        $file = Get-Item -LiteralPath $fullPath
        $entries += [pscustomobject]@{
            RelativePath = $relativePath
            Size = $file.Length
            Sha256 = (Get-FileHash -LiteralPath $fullPath -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    }

    return $entries
}

function Get-ManifestSha256([object[]] $Entries)
{
    $lines = @($Entries | ForEach-Object { "$($_.RelativePath)`t$($_.Size)`t$($_.Sha256)" })
    $canonicalManifest = ($lines -join "`n") + "`n"
    return Get-Sha256Hex $utf8NoBom.GetBytes($canonicalManifest)
}

function Assert-GeneratedManifest([string] $Root, [object] $GenerationLock, [string] $Description)
{
    $manifest = @(Get-SourceManifest $Root)
    $totalBytes = ($manifest | Measure-Object -Property Size -Sum).Sum
    $manifestHash = Get-ManifestSha256 $manifest
    if ($manifest.Count -ne [int] $GenerationLock.generatedFileCount -or
        $totalBytes -ne [long] $GenerationLock.generatedBytes -or
        $manifestHash -cne [string] $GenerationLock.generatedManifestSha256)
    {
        throw "$Description no longer matches the reviewed generated manifest. " +
            "files=$($manifest.Count), bytes=$totalBytes, sha256=$manifestHash."
    }
}

function Write-InternalizedSources([string] $SourceRoot, [string] $DestinationRoot, [int] $ExpectedReplacements)
{
    $relativePaths = @(Get-ChildItem -LiteralPath $SourceRoot -Filter "*.cs" -File -Recurse |
        ForEach-Object { $_.FullName.Substring($SourceRoot.Length + 1).Replace("\", "/") })
    [System.Array]::Sort($relativePaths, [System.StringComparer]::Ordinal)

    $replacements = 0
    foreach ($relativePath in $relativePaths)
    {
        $source = Join-Path $SourceRoot $relativePath.Replace("/", "\")
        $content = [System.IO.File]::ReadAllText($source)
        $matches = [System.Text.RegularExpressions.Regex]::Matches($content, $topLevelPublicPattern)
        if ($matches.Count -ne 1)
        {
            throw "Expected exactly one top-level public declaration in '$relativePath', found $($matches.Count)."
        }

        $internalized = [System.Text.RegularExpressions.Regex]::Replace(
            $content,
            $topLevelPublicPattern,
            "    internal ")
        if ([System.Text.RegularExpressions.Regex]::IsMatch($internalized, $topLevelPublicPattern))
        {
            throw "Top-level public declaration remains in '$relativePath'."
        }

        $destination = Join-Path $DestinationRoot $relativePath.Replace("/", "\")
        $destinationParent = Split-Path -Parent $destination
        if (-not (Test-Path -LiteralPath $destinationParent))
        {
            $null = New-Item -ItemType Directory -Path $destinationParent
        }

        [System.IO.File]::WriteAllText($destination, $internalized, $utf8NoBom)
        $replacements += $matches.Count
    }

    if ($replacements -ne $ExpectedReplacements)
    {
        throw "Expected $ExpectedReplacements top-level accessibility replacements, performed $replacements."
    }
}

$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
$resolvedWorkDirectory = [System.IO.Path]::GetFullPath($WorkDirectory)
$resolvedToolCacheDirectory = [System.IO.Path]::GetFullPath($ToolCacheDirectory)
$resolvedSchemaLockFile = [System.IO.Path]::GetFullPath($SchemaLockFile)
$resolvedToolchainLockFile = [System.IO.Path]::GetFullPath($ToolchainLockFile)

Assert-OutsideRepository $resolvedWorkDirectory $repositoryRoot "WorkDirectory"
Assert-OutsideRepository $resolvedToolCacheDirectory $repositoryRoot "ToolCacheDirectory"
if (Test-Path -LiteralPath $resolvedWorkDirectory)
{
    throw "WorkDirectory '$resolvedWorkDirectory' already exists. Use a new external directory."
}

$schemaLock = Get-Content -Raw -LiteralPath $resolvedSchemaLockFile | ConvertFrom-Json -ErrorAction Stop
$lock = Get-Content -Raw -LiteralPath $resolvedToolchainLockFile | ConvertFrom-Json -ErrorAction Stop
if ($lock.artifactsRedistributed -ne $false -or $lock.generatedArtifactsCommitted -ne $false)
{
    throw "The feasibility lock must not claim redistribution or committed generated artifacts."
}

if ($lock.java.distribution -cne "Eclipse Temurin" -or $lock.java.version -cne "17.0.20+8" -or
    $lock.java.archiveUrl -cne "https://github.com/adoptium/temurin17-binaries/releases/download/jdk-17.0.20%2B8/OpenJDK17U-jdk_x64_windows_hotspot_17.0.20_8.zip" -or
    $lock.sbe.version -cne "1.39.0" -or
    $lock.sbe.sourceRepository -cne "https://github.com/aeron-io/simple-binary-encoding" -or
    $lock.sbe.sourceCommit -cne "e773b57cac6b2008ce30dd219a33de49766c6013" -or
    $lock.sbe.executable.url -cne "https://repo1.maven.org/maven2/uk/co/real-logic/sbe-all/1.39.0/sbe-all-1.39.0.jar" -or
    $lock.systemMemory.version -cne "4.5.3" -or
    $lock.systemMemory.packageUrl -cne "https://api.nuget.org/v3-flatcontainer/system.memory/4.5.3/system.memory.4.5.3.nupkg")
{
    throw "The toolchain lock no longer identifies the reviewed official toolchain."
}

$expectedRuntimePaths = @(
    "csharp/sbe-dll/ByteOrder.cs",
    "csharp/sbe-dll/DirectBuffer.cs",
    "csharp/sbe-dll/EndianessConverter.cs",
    "csharp/sbe-dll/PrimitiveType.cs",
    "csharp/sbe-dll/PrimitiveValue.cs",
    "csharp/sbe-dll/SbePrimitiveType.cs",
    "csharp/sbe-dll/ThrowHelper.cs")
$actualRuntimePaths = @($lock.sbe.runtimeSources | ForEach-Object { [string] $_.upstreamPath })
[System.Array]::Sort($expectedRuntimePaths, [System.StringComparer]::Ordinal)
[System.Array]::Sort($actualRuntimePaths, [System.StringComparer]::Ordinal)
if (($expectedRuntimePaths -join "`n") -cne ($actualRuntimePaths -join "`n"))
{
    throw "The SBE runtime source set no longer matches the reviewed seven files."
}

$expectedFrameworks = @("netstandard2.0", "netstandard2.1", "net8.0", "net9.0", "net10.0")
$actualFrameworks = @($lock.compileTargetFrameworks | ForEach-Object { [string] $_ })
if (($expectedFrameworks -join ";") -cne ($actualFrameworks -join ";"))
{
    throw "The compile target framework matrix no longer matches every current consumer."
}

if ($lock.generation.schemaFileName -cne "spot-fixsbe-1_1.xml" -or
    [int] $lock.generation.schemaId -ne 1 -or [int] $lock.generation.schemaVersion -ne 1 -or
    $lock.generation.targetLanguage -cne "uk.co.real_logic.sbe.generation.csharp.CSharp" -or
    $lock.generation.generateNamespaceDirectory -ne $true -or
    $lock.generation.generatePrecedenceChecks -ne $true)
{
    throw "The generation settings no longer match the reviewed FIX SBE 1:1 pipeline."
}

$null = New-Item -ItemType Directory -Path $resolvedWorkDirectory
if (-not (Test-Path -LiteralPath $resolvedToolCacheDirectory))
{
    $null = New-Item -ItemType Directory -Path $resolvedToolCacheDirectory
}

$httpClient = [System.Net.Http.HttpClient]::new()
$httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Binance.Api-FIX-SBE-feasibility")
try
{
    $jdkArchive = Join-Path $resolvedToolCacheDirectory ([string] $lock.java.archiveFileName)
    Ensure-CachedArtifact $httpClient $lock.java.archiveUrl $jdkArchive `
        ([long] $lock.java.archiveSize) $lock.java.archiveSha256

    $sbeJar = Join-Path $resolvedToolCacheDirectory ([string] $lock.sbe.executable.fileName)
    Ensure-CachedArtifact $httpClient $lock.sbe.executable.url $sbeJar `
        ([long] $lock.sbe.executable.size) $lock.sbe.executable.sha256

    $systemMemoryPackage = Join-Path $resolvedToolCacheDirectory ([string] $lock.systemMemory.packageFileName)
    Ensure-CachedArtifact $httpClient $lock.systemMemory.packageUrl $systemMemoryPackage `
        ([long] $lock.systemMemory.packageSize) $lock.systemMemory.packageSha256

    $runtimePublicRoot = Join-Path $resolvedWorkDirectory "runtime-public"
    $null = New-Item -ItemType Directory -Path $runtimePublicRoot
    foreach ($runtimeSource in $lock.sbe.runtimeSources)
    {
        $fileName = [System.IO.Path]::GetFileName([string] $runtimeSource.upstreamPath)
        $destination = Join-Path $runtimePublicRoot $fileName
        $url = "https://raw.githubusercontent.com/aeron-io/simple-binary-encoding/$($lock.sbe.sourceCommit)/$($runtimeSource.upstreamPath)"
        Ensure-CachedArtifact $httpClient $url $destination ([long] $runtimeSource.size) $runtimeSource.sha256
    }

}
finally
{
    $httpClient.Dispose()
}

$jdkRoot = Join-Path $resolvedToolCacheDirectory ([string] $lock.java.extractedDirectoryName)
if (-not (Test-Path -LiteralPath $jdkRoot))
{
    [System.IO.Compression.ZipFile]::ExtractToDirectory($jdkArchive, $resolvedToolCacheDirectory)
}

$javaExecutable = Join-Path $jdkRoot ([string] $lock.java.javaExecutablePath).Replace("/", "\")
Assert-FileIntegrity $javaExecutable ([long] $lock.java.javaExecutableSize) $lock.java.javaExecutableSha256
$javaVersion = Invoke-NativeCaptured $javaExecutable @("-version")
if ($javaVersion.ExitCode -ne 0 -or $javaVersion.Lines.Count -lt 2 -or
    $javaVersion.Lines[0] -cne 'openjdk version "17.0.20" 2026-07-21' -or
    -not ($javaVersion.Lines -contains 'OpenJDK Runtime Environment Temurin-17.0.20+8 (build 17.0.20+8)'))
{
    throw "Portable Java runtime no longer matches Eclipse Temurin 17.0.20+8.`n$($javaVersion.Lines -join "`n")"
}

$schemaRoot = Join-Path $resolvedWorkDirectory "schemas"
& (Join-Path $PSScriptRoot "Download-BinanceSpotFixSchemas.ps1") `
    -OutputDirectory $schemaRoot -LockFile $resolvedSchemaLockFile
$schema = Join-Path $schemaRoot ([string] $lock.generation.schemaFileName)
if ($schemaLock.sourceCommit -cne "b483413fcdf4da783cd3fcaad6fab7200a93297f")
{
    throw "The FIX schema lock is no longer the reviewed Slice 143 commit."
}

[xml] $schemaXml = Get-Content -Raw -LiteralPath $schema
$namespaceManager = [System.Xml.XmlNamespaceManager]::new($schemaXml.NameTable)
$namespaceManager.AddNamespace("mbx", [string] $lock.generation.vendorMetadataNamespace)
$vendorExponentAttributes = @($schemaXml.SelectNodes("//@mbx:exponent", $namespaceManager))
$vendorExponentReferences = @($vendorExponentAttributes | ForEach-Object { [string] $_.Value } | Sort-Object -Unique)
$expectedExponentReferences = @($lock.generation.vendorExponentReferences | ForEach-Object { [string] $_ })
if ($lock.generation.standardSbeXsdCompatible -ne $false -or
    $vendorExponentAttributes.Count -ne [int] $lock.generation.vendorExponentAttributeCount -or
    ($vendorExponentReferences -join "`n") -cne ($expectedExponentReferences -join "`n"))
{
    throw "Binance vendor exponent metadata no longer matches the reviewed non-standard SBE extension contract."
}

$generationOutputs = @()
foreach ($outputName in @("generated-a", "generated-b"))
{
    $outputRoot = Join-Path $resolvedWorkDirectory $outputName
    $null = New-Item -ItemType Directory -Path $outputRoot
    $arguments = @(
        "--add-opens=java.base/jdk.internal.misc=ALL-UNNAMED",
        "-Dsbe.output.dir=$outputRoot",
        "-Dsbe.generate.ir=false",
        "-Dsbe.target.language=$($lock.generation.targetLanguage)",
        "-Dsbe.csharp.generate.namespace.dir=true",
        "-Dsbe.generate.precedence.checks=true",
        "-Dsbe.validation.stop.on.error=true",
        "-Dsbe.validation.warnings.fatal=false",
        "-jar",
        $sbeJar,
        $schema)
    $generation = Invoke-NativeCaptured $javaExecutable $arguments
    if ($generation.ExitCode -ne 0)
    {
        throw "SbeTool generation failed with exit code $($generation.ExitCode).`n$($generation.Lines -join "`n")"
    }

    $expectedWarnings = @($lock.generation.expectedWarnings | ForEach-Object { [string] $_ })
    if (($generation.Lines -join "`n") -cne ($expectedWarnings -join "`n"))
    {
        throw "SbeTool output no longer matches the exact reviewed warning allowlist.`n$($generation.Lines -join "`n")"
    }

    Assert-GeneratedManifest $outputRoot $lock.generation $outputName
    $messageHeader = Join-Path $outputRoot "fix_sbe\MessageHeader.g.cs"
    if (-not (Test-Path -LiteralPath $messageHeader) -or
        -not ([System.IO.File]::ReadAllText($messageHeader).Contains("namespace $($lock.generation.namespace)")))
    {
        throw "Generated namespace no longer matches '$($lock.generation.namespace)'."
    }

    $generationOutputs += $outputRoot
}

$firstManifest = Get-ManifestSha256 @(Get-SourceManifest $generationOutputs[0])
$secondManifest = Get-ManifestSha256 @(Get-SourceManifest $generationOutputs[1])
if ($firstManifest -cne $secondManifest)
{
    throw "Repeated SBE generation is not byte-for-byte deterministic."
}

$internalizedOutputs = @()
foreach ($index in 0..1)
{
    $internalizedRoot = Join-Path $resolvedWorkDirectory "generated-internal-$index"
    $null = New-Item -ItemType Directory -Path $internalizedRoot
    Write-InternalizedSources $generationOutputs[$index] $internalizedRoot `
        ([int] $lock.generation.topLevelAccessibilityReplacements)

    $manifest = @(Get-SourceManifest $internalizedRoot)
    $totalBytes = ($manifest | Measure-Object -Property Size -Sum).Sum
    $manifestHash = Get-ManifestSha256 $manifest
    if ($totalBytes -ne [long] $lock.generation.internalizedBytes -or
        $manifestHash -cne [string] $lock.generation.internalizedManifestSha256)
    {
        throw "Internalized generated output no longer matches the reviewed manifest."
    }

    $internalizedOutputs += $internalizedRoot
}

$runtimeInternalRoot = Join-Path $resolvedWorkDirectory "runtime-internal"
$null = New-Item -ItemType Directory -Path $runtimeInternalRoot
Write-InternalizedSources $runtimePublicRoot $runtimeInternalRoot $expectedRuntimePaths.Count

$supportRoot = Join-Path $resolvedWorkDirectory "support"
$null = New-Item -ItemType Directory -Path $supportRoot
$systemMemoryAssembly = Join-Path $supportRoot "System.Memory.dll"
$packageArchive = [System.IO.Compression.ZipFile]::OpenRead($systemMemoryPackage)
try
{
    $assemblyEntry = $packageArchive.GetEntry([string] $lock.systemMemory.assemblyPath)
    if ($null -eq $assemblyEntry)
    {
        throw "System.Memory package does not contain '$($lock.systemMemory.assemblyPath)'."
    }

    [System.IO.Compression.ZipFileExtensions]::ExtractToFile($assemblyEntry, $systemMemoryAssembly, $false)
}
finally
{
    $packageArchive.Dispose()
}
Assert-FileIntegrity $systemMemoryAssembly ([long] $lock.systemMemory.assemblySize) $lock.systemMemory.assemblySha256

$compileRoot = Join-Path $resolvedWorkDirectory "compile"
$null = New-Item -ItemType Directory -Path $compileRoot
$projectFile = Join-Path $compileRoot "Binance.SBE.Generated.Feasibility.csproj"
$targetFrameworks = $actualFrameworks -join ";"
$projectXml = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>$targetFrameworks</TargetFrameworks>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <Nullable>disable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <GenerateDocumentationFile>false</GenerateDocumentationFile>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <Deterministic>true</Deterministic>
    <AssemblyName>Binance.SBE.Generated.Feasibility</AssemblyName>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="../generated-internal-0/**/*.cs" />
    <Compile Include="../runtime-internal/*.cs" />
  </ItemGroup>
  <ItemGroup Condition="'`$(TargetFramework)' == 'netstandard2.0'">
    <Reference Include="System.Memory">
      <HintPath>../support/System.Memory.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
</Project>
"@
[System.IO.File]::WriteAllText($projectFile, $projectXml, $utf8NoBom)

$nugetConfig = Join-Path $compileRoot "NuGet.Config"
$nugetXml = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
  </packageSources>
</configuration>
"@
[System.IO.File]::WriteAllText($nugetConfig, $nugetXml, $utf8NoBom)

$restore = Invoke-NativeCaptured "dotnet" @("restore", $projectFile, "--configfile", $nugetConfig)
if ($restore.ExitCode -ne 0)
{
    throw "Offline feasibility restore failed.`n$($restore.Lines -join "`n")"
}

$build = Invoke-NativeCaptured "dotnet" @("build", $projectFile, "--configuration", "Release", "--no-restore")
if ($build.ExitCode -ne 0)
{
    throw "Generated FIX SBE compile matrix failed.`n$($build.Lines -join "`n")"
}

foreach ($framework in $actualFrameworks)
{
    $assemblyPath = Join-Path $compileRoot "bin\Release\$framework\Binance.SBE.Generated.Feasibility.dll"
    if (-not (Test-Path -LiteralPath $assemblyPath))
    {
        throw "Compile output is missing for '$framework'."
    }
}

$inspectionAssemblyPath = Join-Path $compileRoot "bin\Release\net10.0\Binance.SBE.Generated.Feasibility.dll"
$inspectionAssembly = [System.Reflection.Assembly]::LoadFile($inspectionAssemblyPath)
$exportedTypes = @($inspectionAssembly.GetExportedTypes())
if ($exportedTypes.Count -ne [int] $lock.expectedExportedTypeCount)
{
    throw "Expected $($lock.expectedExportedTypeCount) exported generated/runtime types, found $($exportedTypes.Count)."
}

$result = [ordered]@{
    schemaCommit = [string] $schemaLock.sourceCommit
    schemaId = [int] $lock.generation.schemaId
    schemaVersion = [int] $lock.generation.schemaVersion
    java = [string] $lock.java.version
    sbeTool = [string] $lock.sbe.version
    reviewedWarningsPerRun = @($lock.generation.expectedWarnings).Count
    generatedFileCount = [int] $lock.generation.generatedFileCount
    generatedManifestSha256 = [string] $lock.generation.generatedManifestSha256
    deterministicRuns = 2
    internalizedTopLevelTypes = [int] $lock.generation.topLevelAccessibilityReplacements
    compiledTargetFrameworks = $actualFrameworks
    exportedTypeCount = $exportedTypes.Count
    generatedArtifactsCommitted = $false
}
$resultFile = Join-Path $resolvedWorkDirectory "feasibility-result.json"
[System.IO.File]::WriteAllText($resultFile, ($result | ConvertTo-Json -Depth 4), $utf8NoBom)

Write-Output ([pscustomobject] $result)
