function RunCMD($commandName, $arguments, $expectedExitCode) {
	$concatenatedArguments = ""
	$arguments | ForEach-Object {$concatenatedArguments += "$_ "}

	Write-Host "Command Name: $commandName"
	Write-Host "Arguments: $concatenatedArguments"

    $startInfo = New-Object Diagnostics.ProcessStartInfo($commandName)
    $startInfo.UseShellExecute = $false
    $startInfo.Arguments = $concatenatedArguments
    $process = [Diagnostics.Process]::Start($startInfo)
    $process.WaitForExit()
    $exitCode = $process.ExitCode
    if($exitCode -ne $expectedExitCode) {
		$errorMessage = "The command exited with a code of: $exitCode"
		throw $errorMessage
	}
}