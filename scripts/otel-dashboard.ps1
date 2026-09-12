<#
.SYNOPSIS
    Starts (or reuses) an Aspire Dashboard container to receive the Api's OpenTelemetry data.

.DESCRIPTION
    Looks for ANY running container based on the Aspire Dashboard image - no matter which project
    or tool created it - and reuses it instead of creating a duplicate. Ports are host = container
    (18888 UI / 18889 OTLP gRPC), which is the endpoint the Api expects via
    OTEL_EXPORTER_OTLP_ENDPOINT.

    NOTE: this file must stay pure ASCII. Windows PowerShell 5.1 reads .ps1 files without a BOM as
    ANSI, so a non-ASCII character (dash, accent, arrow, curly quote) is decoded into bytes that can
    contain a double quote and break the parser several lines below the real problem.

.EXAMPLE
    ./scripts/otel-dashboard.ps1
    Reuses an existing dashboard, or creates one named 'otel-dashboard'.

.EXAMPLE
    ./scripts/otel-dashboard.ps1 -Force
    Creates a dedicated dashboard even if another one is already running.

.EXAMPLE
    ./scripts/otel-dashboard.ps1 -Remove
    Removes only the container created by this script.
#>
[CmdletBinding()]
param(
    [switch]$Force,
    [switch]$Remove
)

$ErrorActionPreference = 'Stop'

$ContainerName = 'otel-dashboard'
$Image = 'mcr.microsoft.com/dotnet/aspire-dashboard:9.0'
$ImageFilter = 'aspire-dashboard'
$UiPort = 18888
$OtlpPort = 18889

function Test-Docker {
    docker version --format '{{.Server.Version}}' 2>$null | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Error 'Docker is not available. Start Docker Desktop and try again.'
    }
}

function Get-DashboardContainers {
    docker ps --all --format '{{.ID}}|{{.Names}}|{{.Image}}|{{.State}}' |
        Where-Object { $_ -like "*$ImageFilter*" } |
        ForEach-Object {
            $parts = $_ -split '\|'
            [pscustomobject]@{
                Id    = $parts[0]
                Name  = $parts[1]
                Image = $parts[2]
                State = $parts[3]
            }
        }
}

function Write-Endpoints {
    param([string]$Name)

    Write-Host ''
    Write-Host "Dashboard container: $Name"
    Write-Host "  UI:   http://localhost:$UiPort"
    Write-Host "  OTLP: http://localhost:$OtlpPort  (OTEL_EXPORTER_OTLP_ENDPOINT)"
    Write-Host ''
}

Test-Docker

$existing = @(Get-DashboardContainers)

if ($Remove) {
    $owned = $existing | Where-Object { $_.Name -eq $ContainerName }
    if (-not $owned) {
        Write-Host "Nothing to remove: no container named '$ContainerName'."
        $others = $existing | Where-Object { $_.Name -ne $ContainerName }
        if ($others) {
            Write-Host 'Dashboards created outside this script are left untouched:'
            $others | ForEach-Object { Write-Host "  $($_.Name) ($($_.State))" }
        }
        return
    }

    docker rm --force $ContainerName | Out-Null
    Write-Host "Removed container '$ContainerName'."
    return
}

if (-not $Force) {
    $running = $existing | Where-Object { $_.State -eq 'running' } | Select-Object -First 1
    if ($running) {
        Write-Host "Reusing the dashboard already running ($($running.Image))."
        Write-Endpoints -Name $running.Name
        return
    }

    $stopped = $existing | Where-Object { $_.Name -eq $ContainerName } | Select-Object -First 1
    if ($stopped) {
        docker start $ContainerName | Out-Null
        Write-Host "Started the existing container '$ContainerName'."
        Write-Endpoints -Name $ContainerName
        return
    }
}

$name = if ($Force) { "$ContainerName-$(Get-Random -Maximum 9999)" } else { $ContainerName }

Write-Host "Creating container '$name' from $Image ..."

docker run --detach `
    --name $name `
    --restart unless-stopped `
    --publish "${UiPort}:18888" `
    --publish "${OtlpPort}:18889" `
    --env DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true `
    $Image | Out-Null

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to create the container. Ports $UiPort/$OtlpPort may already be in use."
}

Write-Endpoints -Name $name
