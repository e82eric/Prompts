$ErrorActionPreference = "stop"

function new_negotiate_remote_session ($vm) {
	$obj = new_remote_session $vm "negotiate"
	$obj | Add-Member -Type ScriptMethod -Force -Name Execute -Value { param($scriptBlock, $scriptBlockArgs)
		$commandArgs = $this.Vm,$scriptBlock,$scriptBlockArgs
		Invoke-Command -Session $this._session -ArgumentList $commandArgs -ScriptBlock { param($vmStruct, $scriptBlockStr, $scriptBlockArgs)
			$adminUser = new_user_from_struct $vmStruct.AdminUser
			$cmdLine = new_cmd_line $vmStruct.IsoDrive $vmStruct.InstallersDirectory
			$sessionContext = new_remote_session_context $adminUser $cmdLine $vmStruct
			$newArgs = @($sessionContext) + $scriptBlockArgs
			Invoke-Command ([ScriptBlock]::Create($scriptBlockStr)) -ArgumentList $newArgs
		}
	}
	$obj
}

function new_credssp_remote_session ($vm) {
	$obj = new_remote_session $vm "credssp"
	$obj | Add-Member -Type ScriptMethod -Force -Name Execute -Value { param($scriptBlock, $scriptBlockArgs)
		$commandArgs = $this.Vm,$scriptBlock,$scriptBlockArgs
		Invoke-Command -Session $this._session -ArgumentList $commandArgs -ScriptBlock { param($vmStruct, $scriptBlockStr, $scriptBlockArgs)
			$adminUser = new_user_from_struct $vmStruct.AdminUser
			$cmdLine = new_impersonation_cmd_line $adminUser $vmStruct.IsoDrive $vmStruct.InstallersDirectory
			$sessionContext = new_remote_session_context $adminUser $cmdLine $vmStruct
			$newArgs = @($sessionContext) + $scriptBlockArgs
			Invoke-Command ([ScriptBlock]::Create($scriptBlockStr)) -ArgumentList $newArgs
		}
	}
	$obj
}

function new_remote_session ($vm, $authentication) {
	$session = $null
	
	while ($numberOfTries -lt 5) {
		try{
			if($vm.WInRmUri -eq $null) { $vm._setWinRmUri() }
			$session = $vm.CreatePSSession($authentication)
			break
		}
		catch {
			Write-Host $_
			Start-Sleep -s 5
			$numberOfTries++
		}
	}

	if($session -eq $null) { throw "could not create session" }
	
	Invoke-Command -Session $session -FilePath "$($vm.LibDir)\password.ps1"
	Invoke-Command -Session $session -FilePath "$($vm.LibDir)\user.ps1"
	Invoke-Command -Session $session -FilePath "$($vm.LibDir)\cmd_line.ps1"
	Invoke-Command -Session $session -FilePath "$($vm.LibDir)\remote_session_context.ps1"

	$obj = New-Object PSObject -Property @{ _session = $session; Vm = $vm; }

	$obj | Add-Member -Type ScriptMethod -Name Dispose -Value { 
		Remove-PSSession $this._session
		$this._session = $null
	}
	$obj | Add-Member -Type ScriptMethod -Name Execute -Value { param($scriptBlock, $scriptBlockArgs)
		$commandArgs = $this.Vm,$scriptBlock,$scriptBlockArgs
		Invoke-Command -Session $this._session -ArgumentList $commandArgs -ScriptBlock { param($vmStruct, $scriptBlockStr, $scriptBlockArgs)
			$vm = new_remote_vm $vmStruct
			$newArgs = @($vm) + $scriptBlockArgs
			Invoke-Command ([ScriptBlock]::Create($scriptBlockStr)) -ArgumentList $newArgs
		}
	}
	$obj | Add-Member -Type ScriptMethod -Name AddScript -Value { param($path, $scriptArguments)
		Invoke-Command -Session $this._session -FilePath $path -ArgumentList $scriptArguments
	}
	$obj
}
