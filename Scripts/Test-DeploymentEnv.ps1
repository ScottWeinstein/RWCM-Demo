param([string]$Env)

$ErrorActionPreference="stop"
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

Add-Type -Path $PSScriptRoot\..\DemoSite\bin\DemoSite.dll
New-Object DemoSite.Services.ConfigLoader -ArgumentList $Env,"$PSScriptRoot\..\DemoSite"