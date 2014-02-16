function new_remote_session_context ($adminUser, $cmdLine, $vmStruct) {
	$obj = New-Object psobject -Property @{
		Name = $vmStruct.Name;
		InstallersDirectory = $vmStruct.InstallersDirectory;
		IsoDrive = $vmStruct.IsoDrive;
		AdminUser = $adminUser;
		CmdLine = $cmdLine;
		Installers = $vmStruct.Installers
		IpAddress = $vmStruct.IpAddress
	}
	
	$obj | Add-Member -Type ScriptMethod -Name RunCmdAsScheduledTask -Value { param($jobName, $cmdText, $expectedExitCode)
		$this.CmdLine.Execute(
			"schtasks",
			@("/CREATE", "/TN", $jobName, "/SC ONCE", "/SD 01/01/2020", "/ST 00:00:00", "/RL HIGHEST","/RU $($this.AdminUser.Name)", "/RP $($this.AdminUser.Password.PlainText)", "/TR `"$cmdText`"", "/F"),
			0
		)
		$this.CmdLine.Execute("schtasks", @("/RUN", "/I", "/TN $jobName"), 0)
		
		$jobResult = -1
		
		Start-Sleep -S 5
		
		while($true) {
			Write-Host "Checking job status"
			$schedule = new-object -com("Schedule.Service")
			$schedule.connect($this.Name)
			$tasks = $schedule.getfolder("\").gettasks(0)
			$task = $tasks | ? { $_.Name -Match $jobName }
			$jobResult = $task.LastTaskResult
			
			Write-Host "Task Name: $($task.Name)"
			Write-Host "Task State: $($task.State)"
			Write-Host "Task Last Result: $($task.LastTaskResult)"

			if($task.State -eq 3) {
				break
			}

			Start-Sleep -S 2
		}
		
		if($jobResult -ne $expectedExitCode) { throw "Job exited with: $jobResult" }
	}
	
	$obj | Add-Member -Type ScriptMethod MountIso -Value { param($name)
		Write-Host "**** Starting Mount Iso ****"
		$isoPath = "{0}\{1}" -f $this.InstallersDirectory,$name
		$this.CmdLine.Execute("C:\Program Files (x86)\Elaborate Bytes\VirtualCloneDrive\VCDMount.exe", $isoPath, 0)
		Start-Sleep -s 5
		Write-Host "**** End Mount Iso ****"
	}

	$obj
}
