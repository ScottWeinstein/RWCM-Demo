param([string]$Env="NA")
$ErrorActionPreference="stop"
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

$envFile = Join-Path $PSScriptRoot  "..\DemoSite\Env.xml"
$envX = ([xml](Get-Content $envFile)).Env

$validEnvs = $envX.GetEnumerator() | % {$_.Name}
if ($validEnvs -notcontains $Env)
{
	Write-Error "Env '$Env' is not a valid environment"
}

$envRaw = $envX.$Env

$Config = @{}
$Config.Env 			= $Env
$Config.SqlCS 			= "Data Source={0};Integrated Security=True;Initial Catalog=SourceSQLDatabase" -f $envRaw.SqlHost
$Config.ASCS 			= "Data Source={0};Catalog=myDataBase;" -f $envRaw.SSASHost
$Config.SqlEFCS 		= "metadata=.\AdventureWorks.csdl|.\AdventureWorks.ssdl|.\AdventureWorks.msl;provider=System.Data.SqlClient;provider connection string='Data Source={0};Initial Catalog=AdventureWorks;Integrated Security=True;Connection Timeout=60; multipleactiveresultsets=true'" -f  $envRaw.SqlHost
$Config.FileShare 		= $envRaw.FileShare;
$Config.SupportEmail 	= $envRaw.SupportEmail;
$Config


