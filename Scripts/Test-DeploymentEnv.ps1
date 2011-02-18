param([string]$Env)

$ErrorActionPreference="stop"
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

Add-Type -Path $PSScriptRoot\..\DemoSite\bin\DemoSite.dll
$cl = New-Object DemoSite.Services.ConfigLoader -ArgumentList $Env,"$PSScriptRoot\..\DemoSite"
$cl.DemoConfig

$diag = [DemoSite.Services.EnvEvaluator]::Evaluate($cl.DemoConfig)
$diag
$diag.StatusItems | ?  { ! $_.IsOK }