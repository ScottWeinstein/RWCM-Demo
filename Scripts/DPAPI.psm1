Add-Type -AssemblyName System.Security

$entropy = ( 0x21, 0x05, 0x07, 0x08, 0x27, 0x02, 0x23 )

function Protect-Data($plaintext)
{
	$sensitiveData = [Text.Encoding]::UTF8.GetBytes($plaintext)
	$protectedData = [Security.Cryptography.ProtectedData]::Protect($sensitiveData, $entropy, "CurrentUser")
	$encodedText = [Convert]::ToBase64String($protectedData)
	return $encodedText
}

function Unprotect-Data($b64ProtectedText)
{
	$protectedbytes = [Convert]::FromBase64String($b64ProtectedText)
	$unprotectedBytes = [Security.Cryptography.ProtectedData]::Unprotect($protectedbytes,$entropy, "CurrentUser")
	[Text.Encoding]::UTF8.GetString($unprotectedBytes)
}


Export-ModuleMember -Function *
