function new_user ($name, $password, $netBiosName) {
	$obj = New-Object PSObject -Property @{ Name = $name; Password = $password; NetBiosName = $netBiosName }
	$obj | Add-Member -Type ScriptProperty -Name FullName -Value { "{0}\{1}" -f $this.NetBiosName,$this.Name }
	$obj | Add-Member -Type ScriptProperty -Name Credential -Value { New-Object Management.Automation.PSCredential($this.FullName, $this.Password.SecureString) }
	$obj | Add-Member -Type ScriptMethod -Name Create -Value {
		$NON_EXPIRE_FLAG = 0x10000
		$directoryPath = "WinNT://WORKGROUP/{0},computer" -f $this.NetBiosName
		$domain = New-Object System.DirectoryServices.DirectoryEntry($directoryPath)
		$user = $domain.Children.Add($this.Name, "user")
		$user.CommitChanges() | Out-Null
		$user.Invoke("SetPassword", $this.Password.PlainText) | Out-Null
		$user.CommitChanges() | Out-Null

		$uac = $user.Properties["UserFlags"].Value
		$user.Properties["UserFlags"].Value = $uac -bor $NON_EXPIRE_FLAG

		$user.CommitChanges() | Out-Null
	}
	$obj
}

function new_user_from_struct ($struct) {
	$password = new_password_from_struct $struct.Password
	new_user $struct.Name $password $struct.NetBiosName
}
