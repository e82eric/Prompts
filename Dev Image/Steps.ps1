param($stepNumber)

$ErrorActionPreference = "Stop"
$script:steps = $null
$script:createScript = $null
$script:scriptPath = $MyInvocation.MyCommand.path
$script:stepNumber = $stepNumber
$script:winLogonPath = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\winlogon'
$script:scriptDirectory = $(Split-Path $script:MyInvocation.MyCommand.Path)

function Invoke-Steps($createScript) {
	$script:steps = @{}
	$script:createScript = $createScript

	. $createScript -scriptDirectory $script:scriptDirectory

	if($script:stepNumber -eq $null) {
		$script:stepNumber = 1
	} else {
		ClearRestartRegistrySettings
	}
	
	if($script:steps.ContainsKey($script:stepNumber ) -eq $true){
		& $script:steps.get_item($script:stepNumber)
		$script:stepNumber ++
		RestartAndRun
	} else {
		ClearRestartRegistrySettings
	}
}

function ConfigureSteps ($userName, $password) {
	$script:userName = $userName
	$script:password = $password
}

function Step ($stepNumber, $action) {
	$script:steps.add($stepNumber, $action)
}

function ClearRestartRegistrySettings() {
	Set-ItemProperty -Path $script:winLogonPath -Name AutoAdminLogon -Value 0
	
	_DeleteRegistryEntry $script:winLogonPath DefaultUserName
	_DeleteRegistryEntry $script:winLogonPath DefaultPassword
	_DeleteRegistryEntry $script:winLogonPath ForceAutoLogon
	_DeleteRegistryEntry "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" "temp_run"
}

function _DeleteRegistryEntry ($path, $name) {
	$exists = Get-ItemProperty $path $name -ErrorAction SilentlyContinue
	if($exists -ne $null) {
		Remove-ItemProperty $path -Name $name
	}
}

function RestartAndRun() {
	$commandText = "cmd /k powershell . $script:scriptPath -stepNumber $script:stepNumber; Invoke-Steps $script:createScript" 
	
	Set-ItemProperty -Path $script:winLogonPath -Name AutoAdminLogon -Value 1
	New-ItemProperty -Path $script:winLogonPath -Name DefaultUserName -Value $script:userName -Force
	New-ItemProperty -Path $script:winLogonPath -Name DefaultPassword -Value $script:password -Force
	New-ItemProperty -Path $script:winLogonPath -PropertyType DWORD -Name ForceAutoLogon -Value 1 -Force
	New-ItemProperty HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run -Name temp_run -Value $commandText -Force

	Restart-Computer
}