<#
Deploy di Messenger.xap su device o emulatore WP8.1.

Usa AppDeployCmd.exe dell'SDK (percorso variabile per versione e localizzazione),
elenca le destinazioni e installa per indice. Senza certificato il pacchetto viene
installato non firmato: funziona su device sbloccato per sviluppo e sull'emulatore.

  .\tools\deploy.ps1 -List
  .\tools\deploy.ps1 -DeviceIndex 0 -Launch
  .\tools\deploy.ps1 -DeviceIndex 0 -Cert dev.pfx -CertPassword segreta
  .\tools\deploy.ps1 -Uninstall -DeviceIndex 0
#>
[CmdletBinding()]
param(
    [string]$Xap = "Messenger\bin\Debug\Messenger.xap",
    [int]$DeviceIndex = -1,
    [switch]$List,
    [switch]$Uninstall,
    [switch]$Launch,
    [string]$Cert,
    [string]$CertPassword,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

$sdkRoots = @(
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Phone\v8.1\Tools",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Phone\v8.0\Tools",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\WindowsPhone\v8.0\Tools"
)

$deployTool = $null
foreach ($root in $sdkRoots) {
    $candidate = Join-Path $root "AppDeploy\AppDeployCmd.exe"
    if (Test-Path $candidate) { $deployTool = $candidate; break }
}
$signTool = $null
foreach ($root in $sdkRoots) {
    $candidate = Join-Path $root "XapSignTool\XapSignTool.exe"
    if (Test-Path $candidate) { $signTool = $candidate; break }
}

if (-not $deployTool) {
    throw "AppDeployCmd.exe non trovato: installa i tool dell'SDK Windows Phone 8.1."
}
Write-Host "tool: $deployTool"

$manifest = Join-Path (Split-Path $Xap -Parent) "..\..\Properties\WMAppManifest.xml"
$productId = $null
if (Test-Path $manifest) {
    $productId = ([xml](Get-Content $manifest)).Deployment.App.ProductID
}

if ($List) {
    & $deployTool /EnumerateDevices
    exit 0
}

if (-not (Test-Path $Xap)) {
    throw "pacchetto assente: $Xap (crealo con python tools/pack-xap.py)"
}
Write-Host "pacchetto: $Xap ($([math]::Round((Get-Item $Xap).Length / 1MB, 2)) MB)"

if ($Cert) {
    if (-not $signTool) { throw "XapSignTool.exe non trovato: impossibile firmare." }
    if (-not (Test-Path $Cert)) { throw "certificato assente: $Cert" }
    $signArgs = @("sign", "/f", (Resolve-Path $Cert), "/p", $CertPassword, (Resolve-Path $Xap))
    Write-Host "firma: XapSignTool sign /f ... /p ..."
    if (-not $DryRun) { & $signTool @signArgs; if ($LASTEXITCODE -ne 0) { throw "firma fallita ($LASTEXITCODE)" } }
} else {
    Write-Host "nessun certificato: il pacchetto resta non firmato (serve un device sbloccato per sviluppo)."
}

if ($DeviceIndex -lt 0) {
    Write-Host ""
    Write-Host "Indici delle destinazioni: esegui con -List, poi ripeti con -DeviceIndex <n>."
    & $deployTool /EnumerateDevices
    exit 0
}

if ($Uninstall) {
    if (-not $productId) { throw "ProductID non trovato in WMAppManifest.xml" }
    $verb = "/uninstall"
    $operand = $productId
} else {
    $verb = if ($Launch) { "/installlaunch" } else { "/install" }
    $operand = (Resolve-Path $Xap)
}

$arguments = @($verb, $operand, "/targetdevice:$DeviceIndex")
Write-Host ("comando: AppDeployCmd " + ($arguments -join " "))
if ($DryRun) { exit 0 }

& $deployTool @arguments
if ($LASTEXITCODE -ne 0) { throw "deploy fallito ($LASTEXITCODE)" }
Write-Host "fatto: $verb su targetdevice $DeviceIndex"
