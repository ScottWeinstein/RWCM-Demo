param([string]$Env="NA", [Uri]$envFileUri=$null, [string]$EnvFileContent="" )

$ErrorActionPreference="stop"

# Where am I?
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent


Import-Module $PSScriptRoot\DPAPI.psm1 -Force

# Get raw xml string data
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
	

# Get XML object
$envX = ([xml]$envFileContent).Env
	

# Am I a valid env?
$validEnvs = $envX.GetEnumerator() | % {$_.Name}
if ($validEnvs -notcontains $Env)
{
	Write-Error "Env '$Env' is not a valid environment"
}

# select my specific env
$envRaw = $envX.$Env


# Decrypt passwords
if ($envRaw.PasswordProtected)
{
	foreach ($pasF in $envRaw.PasswordProtected.Field)
	{	
		$envRaw.$pasF = Unprotect-Data $envRaw.$pasF
	}
}

# Now build the Config HT
$Config = @{}
$Config.Env 			= $Env

$Config.MarketingASConnectionString 	= "Data Source={0};Catalog=myDataBase;" -f $envRaw.SSASHost
$Config.LoggingDBConnectionString 		= "Data Source={0};Integrated Security=True;Initial Catalog=LoggingDB" -f $envRaw.SqlHost
$Config.ProductEFConnectionString 		= "metadata=res://*/Models.ProductDB.csdl|res://*/Models.ProductDB.ssdl|res://*/Models.ProductDB.msl;provider=System.Data.SqlClient;provider connection string='Data Source={0};Initial Catalog=DestSQLDatabase;Integrated Security=True;Pooling=False;MultipleActiveResultSets=True'" -f  $envRaw.SqlHost

$Config.FileShare 		= $envRaw.FileShare;
$Config.SupportEmail 	= $envRaw.SupportEmail;
$Config.IsProduction	= ($Env -eq "Prod");
$Config.FTPPassword 	= $envRaw.FTPPassword;


$Config


