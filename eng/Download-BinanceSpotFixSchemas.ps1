[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectory,

    [ValidateNotNullOrEmpty()]
    [string] $LockFile = (Join-Path $PSScriptRoot "binance-spot-fix-schema-lock.json"),

    [switch] $Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Net.Http

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

function Get-Artifact([object[]] $Artifacts, [string] $FileName)
{
    $matches = @($Artifacts | Where-Object { $_.Artifact.fileName -eq $FileName })
    if ($matches.Count -ne 1)
    {
        throw "Expected exactly one downloaded artifact named '$FileName', found $($matches.Count)."
    }

    return $matches[0]
}

function Get-ArtifactText([object] $DownloadedArtifact)
{
    return [System.Text.Encoding]::UTF8.GetString([byte[]] $DownloadedArtifact.Bytes)
}

function Get-ReviewedLifecycle([object] $DownloadedArtifact)
{
    $raw = Get-ArtifactText $DownloadedArtifact
    $strictJsonAccepted = $true
    try
    {
        $null = $raw | ConvertFrom-Json -ErrorAction Stop
    }
    catch
    {
        $strictJsonAccepted = $false
    }

    if ($DownloadedArtifact.Artifact.strictJson -ne $strictJsonAccepted)
    {
        throw "Lifecycle strict-JSON state for '$($DownloadedArtifact.Artifact.fileName)' no longer matches the reviewed lock."
    }

    if ($strictJsonAccepted)
    {
        throw "Lifecycle '$($DownloadedArtifact.Artifact.fileName)' unexpectedly became strict JSON without a lock promotion."
    }

    $reviewedJson = $raw -replace ',(\s*\])', '$1'
    return $reviewedJson | ConvertFrom-Json -ErrorAction Stop
}

$resolvedLockFile = [System.IO.Path]::GetFullPath($LockFile)
$resolvedOutputDirectory = [System.IO.Path]::GetFullPath($OutputDirectory)
$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
$repositoryPrefix = $repositoryRoot.TrimEnd(
    [System.IO.Path]::DirectorySeparatorChar,
    [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar
if ($resolvedOutputDirectory -ieq $repositoryRoot -or
    $resolvedOutputDirectory.StartsWith($repositoryPrefix, [System.StringComparison]::OrdinalIgnoreCase))
{
    throw "Raw Binance artifacts must be downloaded outside the repository worktree."
}

$lock = Get-Content -Raw -LiteralPath $resolvedLockFile | ConvertFrom-Json -ErrorAction Stop

if ($lock.sourceRepository -cne "https://github.com/binance/binance-spot-api-docs")
{
    throw "The schema source repository is not the reviewed official Binance repository."
}

if ([string] $lock.sourceCommit -cnotmatch '^[0-9a-f]{40}$')
{
    throw "The schema source commit must be a full lowercase Git commit hash."
}

if ($lock.rawArtifactsRedistributed -ne $false)
{
    throw "The schema lock must not claim that raw Binance artifacts are redistributed by this repository."
}

$expectedBaseUrl = "https://raw.githubusercontent.com/binance/binance-spot-api-docs/$($lock.sourceCommit)/"
if ($lock.sourceRawBaseUrl -cne $expectedBaseUrl)
{
    throw "The schema source URL is not pinned to sourceCommit."
}

$expectedPaths = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($expectedPath in @(
    "fix/schemas/spot-fix-oe.xml",
    "fix/schemas/spot-fix-md.xml",
    "sbe/schemas/spot-fixsbe-1_1.xml",
    "sbe/schemas/spot_fix_prod_latest.xml",
    "sbe/schemas/spot_fix_testnet_latest.xml",
    "sbe/schemas/sbe_fix_schema_lifecycle_prod.json",
    "sbe/schemas/sbe_fix_schema_lifecycle_testnet.json"))
{
    $null = $expectedPaths.Add($expectedPath)
}

$artifacts = @($lock.artifacts)
if ($artifacts.Count -ne $expectedPaths.Count)
{
    throw "Expected exactly $($expectedPaths.Count) reviewed FIX artifacts, found $($artifacts.Count)."
}

$actualPaths = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($artifact in $artifacts)
{
    $upstreamPath = [string] $artifact.upstreamPath
    $fileName = [string] $artifact.fileName
    if (-not $expectedPaths.Contains($upstreamPath) -or -not $actualPaths.Add($upstreamPath))
    {
        throw "Artifact path '$upstreamPath' is unexpected or duplicated."
    }

    if ($fileName -cne [System.IO.Path]::GetFileName($upstreamPath))
    {
        throw "Artifact filename '$fileName' does not match reviewed path '$upstreamPath'."
    }
}

$downloaded = @()
$httpClient = [System.Net.Http.HttpClient]::new()
$httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Binance.Api-FIX-schema-verifier")
try
{
    foreach ($artifact in $artifacts)
    {
        $uri = [System.Uri]::new([System.Uri] $lock.sourceRawBaseUrl, [string] $artifact.upstreamPath)
        $bytes = $httpClient.GetByteArrayAsync($uri).GetAwaiter().GetResult()
        if ($bytes.LongLength -ne [long] $artifact.size)
        {
            throw "Size mismatch for '$($artifact.upstreamPath)': expected $($artifact.size), received $($bytes.LongLength)."
        }

        $actualHash = Get-Sha256Hex $bytes
        if ($actualHash -cne [string] $artifact.sha256)
        {
            throw "SHA-256 mismatch for '$($artifact.upstreamPath)': expected $($artifact.sha256), received $actualHash."
        }

        $downloaded += [pscustomobject]@{ Artifact = $artifact; Bytes = $bytes }
    }
}
finally
{
    $httpClient.Dispose()
}

$oe = Get-Artifact $downloaded "spot-fix-oe.xml"
$md = Get-Artifact $downloaded "spot-fix-md.xml"
[xml] $oeXml = Get-ArtifactText $oe
[xml] $mdXml = Get-ArtifactText $md
$oeMessages = @($oeXml.SelectNodes("/fix/messages/message"))
$mdMessages = @($mdXml.SelectNodes("/fix/messages/message"))
if ($oeMessages.Count -ne [int] $oe.Artifact.messageCount -or $mdMessages.Count -ne [int] $md.Artifact.messageCount)
{
    throw "QuickFIX message counts no longer match the reviewed lock."
}

$messageTypes = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($message in @($oeMessages + $mdMessages))
{
    $null = $messageTypes.Add([string] $message.msgtype)
}
if ($messageTypes.Count -ne 25)
{
    throw "Expected 25 case-sensitive FIX message types, found $($messageTypes.Count)."
}

$fixSbe = Get-Artifact $downloaded "spot-fixsbe-1_1.xml"
[xml] $fixSbeXml = Get-ArtifactText $fixSbe
$fixSbeRoot = $fixSbeXml.DocumentElement
$fixSbeMessages = @($fixSbeRoot.SelectNodes("*[local-name()='message']"))
if ($fixSbeRoot.GetAttribute("id") -cne [string] $fixSbe.Artifact.schemaId -or
    $fixSbeRoot.GetAttribute("version") -cne [string] $fixSbe.Artifact.schemaVersion -or
    $fixSbeMessages.Count -ne [int] $fixSbe.Artifact.messageCount)
{
    throw "FIX SBE identity or message count no longer matches the reviewed lock."
}

foreach ($aliasFile in @("spot_fix_prod_latest.xml", "spot_fix_testnet_latest.xml"))
{
    $alias = Get-Artifact $downloaded $aliasFile
    if ((Get-ArtifactText $alias).Trim() -cne [string] $alias.Artifact.expectedTarget)
    {
        throw "Alias '$aliasFile' no longer targets '$($alias.Artifact.expectedTarget)'."
    }
}

foreach ($lifecycleFile in @("sbe_fix_schema_lifecycle_prod.json", "sbe_fix_schema_lifecycle_testnet.json"))
{
    $lifecycleArtifact = Get-Artifact $downloaded $lifecycleFile
    $lifecycle = Get-ReviewedLifecycle $lifecycleArtifact
    if ($lifecycle.environment -cne [string] $lifecycleArtifact.Artifact.environment -or
        [int] $lifecycle.latestSchema.id -ne [int] $lifecycleArtifact.Artifact.latestSchemaId -or
        [int] $lifecycle.latestSchema.version -ne [int] $lifecycleArtifact.Artifact.latestSchemaVersion)
    {
        throw "Lifecycle '$lifecycleFile' no longer matches the reviewed environment or latest schema."
    }
}

if (-not (Test-Path -LiteralPath $resolvedOutputDirectory))
{
    $null = New-Item -ItemType Directory -Path $resolvedOutputDirectory
}

foreach ($item in $downloaded)
{
    $destination = Join-Path $resolvedOutputDirectory ([string] $item.Artifact.fileName)
    if ((Test-Path -LiteralPath $destination) -and -not $Force)
    {
        throw "Destination '$destination' already exists. Use -Force to replace only the locked artifact files."
    }
}

foreach ($item in $downloaded)
{
    $destination = Join-Path $resolvedOutputDirectory ([string] $item.Artifact.fileName)
    [System.IO.File]::WriteAllBytes($destination, [byte[]] $item.Bytes)
}

Write-Output "Verified and wrote $($downloaded.Count) Binance Spot FIX schema artifacts from commit $($lock.sourceCommit)."
