$ErrorActionPreference = "stop"

function new_cmd_line ($isoDirectory, $installerDirectory) {
	$obj = New-Object psobject -Property @{ IsoDirectory = $isoDirectory; InstallerDirectory = $installerDirectory }
	$obj | Add-Member -Type ScriptMethod -Name InstallerExecute -Value { param($fileName, $arguments, $expectedExitCode) 
		$this.Execute("$($this.InstallerDirectory)\$fileName", $arguments, $expectedExitCode) 
	}
	$obj | Add-Member -Type ScriptMethod -Name IsoExecute -Value { param($fileName, $arguments, $expectedExitCode) 
		$this.Execute("$($this.IsoDirectory)\$fileName", $arguments, $expectedExitCode) 
	}
	$obj | Add-Member -Type ScriptMethod -Name AddToPath -Value { param($pathToAdd)
		$env:path += ";$pathToAdd"
		[Environment]::SetEnvironmentVariable("PATH", $env:path, "MACHINE")
	}
	$obj | Add-Member -Type ScriptMethod -Name _execute -Value { param ($fileName, $arguments, $expectedExitCode, $beforeStart)
		$concatenatedArguments = ""
		$arguments | % { $concatenatedArguments += "$_ " }
		
		Write-Host "Command: $fileName"
		Write-Host "Arguments: $concatenatedArguments"

		$process = New-Object Diagnostics.Process 
		$setup = $process.StartInfo
		$setup.FileName = $fileName
		$setup.Arguments = $concatenatedArguments

		$setup.UseShellExecute = $false
		$setup.RedirectStandardError = $true
		$setup.RedirectStandardOutput = $true
		$setup.RedirectStandardInput = $false

		$errEvent = Register-ObjectEvent -InputObj $process -Event "ErrorDataReceived" -Action { param([System.Object] $sender, [System.Diagnostics.DataReceivedEventArgs] $e)
			if ($e.Data) {
				Write-Host $e.Data
			}
			else {
				New-Event -SourceIdentifier "LastMsgReceived"
			}
		}

		$outEvent = Register-ObjectEvent -InputObj $process -Event "OutputDataReceived" -Action { param([System.Object] $sender, [System.Diagnostics.DataReceivedEventArgs] $e)
			Write-Host $e.Data
		}
		
		if($beforeStart -ne $null) {
			& $beforeStart $process
		}
	  
		Write-Host "Working Directory"
		Write-Host $setup.WorkingDirectory

		$exitCode = -1
		if ($process.Start()) {
			$process.BeginOutputReadLine()
			$process.BeginErrorReadLine()

			$process.WaitForExit()
			$exitCode = [int]$process.ExitCode
			Wait-Event -SourceIdentifier "LastMsgReceived" -Timeout 60 | Out-Null

			$process.CancelOutputRead()
			$process.CancelErrorRead()
			$process.Close()
		}
		
		if ($exitCode -ne $expectedExitCode) { throw $exitCode }

		Write-Host "**** End Command Line ****"		
	}
	$obj | Add-Member -Type ScriptMethod Execute { param($fileName, $arguments, $expectedExitCode)
		$this._execute($fileName, $arguments, $expectedExitCode, $null)
	}
	$obj
}

function new_impersonation_cmd_line ($user, $isoDirectory, $installerDirectory) {
	$obj = new_cmd_line $isoDirectory $installerDirectory
	$obj | Add-Member -Type NoteProperty User $user
	$obj | Add-Member -Type ScriptMethod -Force Execute { param($fileName, $arguments, $expectedExitCode, $userBeforeStart)
		$this._execute($fileName, $arguments, $expectedExitCode, { param($process)
			if($userBeforeStart -ne $null) { & $userBeforeStart $process }
			$setup = $process.StartInfo
			$setup.Password = $this.User.Password.SecureString
			$setup.UserName = $this.User.Name
			$setup.Domain = $this.User.NetBiosName
			$setup.Verb = "runas"
		})
	}
	$obj
}
