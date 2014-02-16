param($name = "dprompts", $hostInstallersDirectory = "D:\Downloads", $windowsKey = "", $memory = 4096, $cpuCores = 5, $vdiDirectory = "C:\virtual_boxes", $gitName, $gitEmail, $ipAddress = "192.168.56.107")
$ErrorPreferenceAction = "stop"

$workingDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$libDir = "$workingDirectory\external\dev_images\lib"
$toolsDir = "$workingDirectory\external\dev_images\tools"

. "$libDir\vm_base.ps1"
. "$libDir\virtualbox_vm.ps1"

$vm = new_virtualbox_vm `
	-Name $name `
	-HostInstallersDirectory $hostInstallersDirectory `
	-WindowsKey $windowsKey `
	-VdiDirectory $vdiDirectory `
	-IpAddress $ipAddress `
	-HostIpAddress "192.168.56.1" `
	-HostOnlyNetwork "VirtualBox Host-Only Ethernet Adapter" `
	-InstallersDriveLetter "C" `
	-IsoDrive "E:" `
	-OsIso "en_windows_server_2008_r2_with_sp1_x64_dvd_617601.iso" `
	-ToolsDir $toolsDir `
	-Memory $memory `
	-CpuCores $cpuCores

& "$workingDirectory\deploy_base.ps1" `
	-Vm $vm `
	-AdminUserName "administrator" `
	-AdminPassword "pass@word1" `
	-GitName $gitName `
	-GitEmail $gitEmail `
	-WorkingDirectory $workingDirectory `
	-LibDir $libDir
