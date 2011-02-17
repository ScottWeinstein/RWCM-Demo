param([string]$Env="NA", [Uri]$envFileUri=$null, [string]$EnvFileContent="" )
$ErrorActionPreference="stop"
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

Import-Module $PSScriptRoot\DPAPI.psm1 -Force

if ($EnvFileContent -eq "")
{
	if ($envFileUri -eq $null )
	{
		$envFile = Join-Path $PSScriptRoot  "..\DemoSite\Env.xml"
		$envFileContent = Get-Content $envFile
	}
	elseif ($envFileUri.IsFile)
	{
		$envFileContent = Get-Content $envFileUri.LocalPath
	}
	elseif ($envFileUri.Scheme -eq "http")
	{
		$wc = New-Object Net.WebClient
		$envFileContent = $wc.DownloadString($envFileUri)
	}
}

if ($EnvFileContent -eq "")
{
	Write-Error "unable to get EnvFileContent"
}
	

$envX = ([xml]$envFileContent).Env
	

$validEnvs = $envX.GetEnumerator() | % {$_.Name}
if ($validEnvs -notcontains $Env)
{
	Write-Error "Env '$Env' is not a valid environment"
}

$envRaw = $envX.$Env

if ($envRaw.PasswordProtected)
{
	foreach ($pasF in $envRaw.PasswordProtected.Field)
	{	
		Write-Host $pasF -ForegroundColor Gray
		$envRaw.$pasF = Unprotect-Data $envRaw.$pasF
	}
}

$Config = @{}
$Config.Env 			= $Env
$Config.SqlCS 			= "Data Source={0};Integrated Security=True;Initial Catalog=SourceSQLDatabase" -f $envRaw.SqlHost
$Config.ASCS 			= "Data Source={0};Catalog=myDataBase;" -f $envRaw.SSASHost
$Config.SqlEFCS 		= "metadata=.\AdventureWorks.csdl|.\AdventureWorks.ssdl|.\AdventureWorks.msl;provider=System.Data.SqlClient;provider connection string='Data Source={0};Initial Catalog=AdventureWorks;Integrated Security=True;Connection Timeout=60; multipleactiveresultsets=true'" -f  $envRaw.SqlHost
$Config.FileShare 		= $envRaw.FileShare;
$Config.SupportEmail 	= $envRaw.SupportEmail;
$Config.IsProduction	= ($Env -eq "Prod");
$Config.FTPPassword 	= $envRaw.FTPPassword;
$Config


