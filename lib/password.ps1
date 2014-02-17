function new_password ($plainText) {
	$obj = New-Object PSObject -Property @{ PlainText = $plainText }
	$obj | Add-Member -Type ScriptProperty -Name SecureString -Value { 
		ConvertTo-SecureString $this.PlainText -AsPlainText -Force
	}
	$obj
}

function new_password_from_struct ($struct) {
	new_Password $struct.PlainText
}
