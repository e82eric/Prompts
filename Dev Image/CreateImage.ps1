#cmd /k powershell set-executionpolicy remotesigned;. C:\Steps.ps1; Invoke-Steps C:\CreateImage.ps1

param($scriptDirectory)

$ErrorActionPreference = "Stop"

$script:workingDirectory = $scriptDirectory 

. "$script:workingDirectory\CreateImage.config.ps1"
. "$script:workingDirectory\CreateImage.user.config.ps1"
. "$script:workingDirectory\Steps.Cmd.ps1"

ConfigureSteps $script:adminAccount $script:password

Step 1 {
	SetComputerName
	ConfigureExplorer
	DisableServerManagerScreensAtStartup
	DisableInternetExploererEnhancedSecurity
	RunCMD $script:installers.get_item("DameonTools") "/S" 0
}

Step 2 {
	InstallDotNet35
	InstallSQLServer
}

Step 3 {
	InstallDotNet45
}

Step 4 {
	InstallVisualStudio
}

Step 5 {
	InstallVisualStudioWebTools
	InstallGit
	InstallRuby
	InstallKDiff
	InstallWIX
	AttachAdventureWorksDW
	DeployAdventureWorksCube
	GetSource
}

Step 6 {
	InstallWebServer
}

function MountISO($isoName) {
	$isoArguments = @(
		"-mount", 
		"0,", 
		"$isoName"
	)
	RunCMD "C:\Program Files (x86)\Daemon Tools Lite\DTLite.exe" $isoArguments 0
}

function RunISOCMD($commandName, $arguments, $expectedExitCode) {
	RunCMD "$script:isoDrive\$commandName" $arguments $expectedExitCode
}

function AddToPath ($pathToAdd) {
	$env:path += ";$pathToAdd\"
	[Environment]::SetEnvironmentVariable("PATH", $env:path, "MACHINE")
}

function CreateADUser($name) {
	$NON_EXPIRE_FLAG = 0x10000

	$domain = New-Object System.DirectoryServices.DirectoryEntry("WinNT://WORKGROUP/$env:computername,computer")
	$user = $domain.Children.Add("$name", "user")
	$user.CommitChanges() | Out-Null

	$user.Invoke("SetPassword", "$global:password") | Out-Null
	$user.CommitChanges() | Out-Null

	$uac = $user.Properties["UserFlags"].Value
	$user.Properties["UserFlags"].Value = $uac -bor $NON_EXPIRE_FLAG

	$user.CommitChanges() | Out-Null

	$psUser = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "$env:computername\$name",$global:secureStringPassword

	$psUser
}

function SetComputerName {
	$computer = Get-WmiObject Win32_ComputerSystem
	$computer.Rename($script:computerName)
}

function ConfigureExplorer {
	$key = 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced'
	Set-ItemProperty $key Hidden 1
	Set-ItemProperty $key HideFileExt 0
	Set-ItemProperty $key ShowSuperHidden 1
}

function DisableServerManagerScreensAtStartup {
	New-ItemProperty -Path HKCU:\Software\Microsoft\ServerManager -Name DoNotOpenServerManagerAtLogon -Value 1 -PropertyType dword
	Set-ItemProperty -Path HKLM:\Software\Microsoft\ServerManager\Oobe -Name DoNotOpenInitialConfigurationTasksAtLogon -Value 1 
}

function DisableInternetExploererEnhancedSecurity {
	Set-ItemProperty `
		-Path "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}" `
		-Name "IsInstalled" `
		-Value 0
}

function DisableLoopbackCheck {
	New-ItemProperty HKLM:\System\CurrentControlSet\Control\Lsa -Name "DisableLoopbackCheck" -Value 1 -PropertyType dword
}

function InstallDotNet35 {
	Import-Module ServerManager 
	Add-WindowsFeature as-net-framework
}

function InstallSQLServer {
	MountISO $script:installers.get_item("SQLServer2012")
	$serviceAccount = CreateADUser $script:sqlServiceAccount

	$logPath = "C:\Log"

	New-Item $script:dataPath -type directory
	New-Item $logPath -type directory

	$serviceAccountUserName = $serviceAccount.UserName
	$adminUserName = "$script:computerName\$script:adminAccount"
	
	$arguments = @(
		"/QUIET=False",
		"/QUIETSIMPLE=True",
		"/FEATURES=SQLENGINE,SSMS,CONN,ADV_SSMS,AS,RS,IS,BIDS",
		"/ACTION=Install",
		"/X86=False",
		"/ERRORREPORTING=False",
		'/INSTALLSHAREDDIR="C:\Program Files\Microsoft SQL Server"',
		'/INSTALLSHAREDWOWDIR="C:\Program Files (x86)\Microsoft SQL Server"'
		'/INSTANCEDIR="C:\Program Files\Microsoft SQL Server"'
		"/SQMREPORTING=False",
		"/INSTANCENAME=MSSQLSERVER",
		"/AGTSVCACCOUNT=$serviceAccountUserName",
		"/AGTSVCPassword=$script:password",
		"/AGTSVCSTARTUPTYPE=Automatic",
		"/SQLSVCSTARTUPTYPE=Automatic",
		"/SQLSVCACCOUNT=$serviceAccountUserName",
		"/SQLSVCPassword=$script:password",
		"/SQLUSERDBDIR=`"$dataPath`"",
		"/SQLUSERDBLOGDIR=`"$logPath`"",
		"/SQLTEMPDBDIR=`"$dataPath`"",
		"/SQLTEMPDBLOGDIR=`"$logPath`"",
		"/TCPENABLED=1",
		"/NPENABLED=0",
		"/BROWSERSVCSTARTUPTYPE=Automatic",
		"/IACCEPTSQLSERVERLICENSETERMS=TRUE",
		"/SQLSYSADMINACCOUNTS=$adminUserName",
		"/ASSVCACCOUNT=$serviceAccountUserName",
		"/ASSVCPASSWORD=$script:password",
		"/ASSYSADMINACCOUNTS=$adminUserName",
		"/ISSVCACCOUNT=$serviceAccountUserName"
		"/ISSVCPASSWORD=$script:password"
		"/RSSVCACCOUNT=$serviceAccountUserName"
		"/RSSVCPASSWORD=$script:password"
	)

	RunISOCMD "Setup.exe" $arguments 3010
}

function InstallDotNet45 {
	MountISO $global:installers.get_item("VisualStudio")
	RunISOCMD "packages\dotNetFramework\dotNetFx45_Full_x86_x64.exe" @("/passive", "/norestart", "/Log C:\net45.log") 3010
}

function InstallVisualStudio {
	$adminFilePath = "c:\VisualStudioAdminFile.xml"

'<?xml version="1.0" encoding="utf-8"?>
<AdminDeploymentCustomizations xmlns="http://schemas.microsoft.com/wix/2011/AdminDeployment">
   <BundleCustomizations TargetDir="default" NoWeb="default"/>

   <SelectableItemCustomizations>
     <SelectableItemCustomization Id="WebTools" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="OfficeTools" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="SharepointTools" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="LightSwitch" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="SilverLight_Developer_Kit" Hidden="no" Selected="yes" />
     <SelectableItemCustomization Id="SQL" Hidden="no" Selected="yes" />
     <SelectableItemCustomization Id="VC_MFC_Libraries" Hidden="no" Selected="no" />
     <SelectableItemCustomization Id="Blend" Hidden="no" Selected="no" />

     <SelectableItemCustomization Id="BlissHidden" Selected="yes" />
     <SelectableItemCustomization Id="HelpHidden" Selected="yes" />
     <SelectableItemCustomization Id="IntelliTraceUltimateHidden" Selected="yes" />
     <SelectableItemCustomization Id="LocalDBHidden" Selected="yes" />
     <SelectableItemCustomization Id="NetFX4Hidden" Selected="yes" />
     <SelectableItemCustomization Id="NetFX45Hidden" Selected="yes" />
     <SelectableItemCustomization Id="PortableDTPHidden" Selected="yes" />
     <SelectableItemCustomization Id="PreEmptiveDotfuscatorHidden" Selected="yes" />
     <SelectableItemCustomization Id="PreEmptiveAnalyticsHidden" Selected="yes" />
     <SelectableItemCustomization Id="ProfilerHidden" Selected="yes" />
     <SelectableItemCustomization Id="ReportingHidden" Selected="yes" />
     <SelectableItemCustomization Id="RIAHidden" Selected="no" />
     <SelectableItemCustomization Id="SDKTools3Hidden" Selected="yes" />
     <SelectableItemCustomization Id="SDKTools4Hidden" Selected="yes" />
     <SelectableItemCustomization Id="Silverlight5DRTHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLCEHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLCEToolsHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLCLRTypesHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLDACHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLDbProviderHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLDOMHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLSharedManagementObjectsHidden" Selected="yes" />
     <SelectableItemCustomization Id="StoryboardingHidden" Selected="yes" />
     <SelectableItemCustomization Id="TSQLHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCCompilerHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCCoreHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCDebugHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCDesigntimeHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCExtendedHidden" Selected="yes" />
     <SelectableItemCustomization Id="WCFDataServicesHidden" Selected="yes" />
     <SelectableItemCustomization Id="WinJSHidden" Selected="yes" />
     <SelectableItemCustomization Id="WinSDKHidden" Selected="yes" />
  </SelectableItemCustomizations>

</AdminDeploymentCustomizations>' | Out-File -Encoding "UTF8" $adminFilePath

	MountISO $global:installers.get_item("VisualStudio")
	RunISOCMD "vs_ultimate.exe" @("/passive", "/norestart", "/AdminFile $adminFilePath", "/Log C:\vs.log") 0
	RunCMD "MSIExec" @("/i", $global:installers.get_item("Resharper"), "/passive", "/norestart") 0
}

function InstallVisualStudioWebTools {
	$adminFilePath = "c:\VisualStudioAdminFile.xml"

'<?xml version="1.0" encoding="utf-8"?>
<AdminDeploymentCustomizations xmlns="http://schemas.microsoft.com/wix/2011/AdminDeployment">
   <BundleCustomizations TargetDir="default" NoWeb="default"/>

   <SelectableItemCustomizations>
     <SelectableItemCustomization Id="WebTools" Hidden="no" Selected="yes"/>
     <SelectableItemCustomization Id="OfficeTools" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="SharepointTools" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="LightSwitch" Hidden="no" Selected="no"/>
     <SelectableItemCustomization Id="SilverLight_Developer_Kit" Hidden="no" Selected="yes" />
     <SelectableItemCustomization Id="SQL" Hidden="no" Selected="yes" />
     <SelectableItemCustomization Id="VC_MFC_Libraries" Hidden="no" Selected="no" />
     <SelectableItemCustomization Id="Blend" Hidden="no" Selected="no" />

     <SelectableItemCustomization Id="BlissHidden" Selected="yes" />
     <SelectableItemCustomization Id="HelpHidden" Selected="yes" />
     <SelectableItemCustomization Id="IntelliTraceUltimateHidden" Selected="yes" />
     <SelectableItemCustomization Id="LocalDBHidden" Selected="yes" />
     <SelectableItemCustomization Id="NetFX4Hidden" Selected="yes" />
     <SelectableItemCustomization Id="NetFX45Hidden" Selected="yes" />
     <SelectableItemCustomization Id="PortableDTPHidden" Selected="yes" />
     <SelectableItemCustomization Id="PreEmptiveDotfuscatorHidden" Selected="yes" />
     <SelectableItemCustomization Id="PreEmptiveAnalyticsHidden" Selected="yes" />
     <SelectableItemCustomization Id="ProfilerHidden" Selected="yes" />
     <SelectableItemCustomization Id="ReportingHidden" Selected="yes" />
     <SelectableItemCustomization Id="RIAHidden" Selected="no" />
     <SelectableItemCustomization Id="SDKTools3Hidden" Selected="yes" />
     <SelectableItemCustomization Id="SDKTools4Hidden" Selected="yes" />
     <SelectableItemCustomization Id="Silverlight5DRTHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLCEHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLCEToolsHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLCLRTypesHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLDACHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLDbProviderHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLDOMHidden" Selected="yes" />
     <SelectableItemCustomization Id="SQLSharedManagementObjectsHidden" Selected="yes" />
     <SelectableItemCustomization Id="StoryboardingHidden" Selected="yes" />
     <SelectableItemCustomization Id="TSQLHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCCompilerHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCCoreHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCDebugHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCDesigntimeHidden" Selected="yes" />
     <SelectableItemCustomization Id="VCExtendedHidden" Selected="yes" />
     <SelectableItemCustomization Id="WCFDataServicesHidden" Selected="yes" />
     <SelectableItemCustomization Id="WinJSHidden" Selected="yes" />
     <SelectableItemCustomization Id="WinSDKHidden" Selected="yes" />
  </SelectableItemCustomizations>

</AdminDeploymentCustomizations>' | Out-File -Encoding "UTF8" $adminFilePath

	MountISO $global:installers.get_item("VisualStudio")
	RunISOCMD "vs_ultimate.exe" @("/passive", "/norestart", "/AdminFile $adminFilePath", "/Log C:\vs.log") 3010
}

function InstallGit {
	RunCMD $script:installers.get_item("Git") @("/silent") 0
	AddToPath "C:\Program Files (x86)\Git\cmd"
}

function AttachAdventureWorksDW {
	$databaseMdf = Get-ChildItem $script:installers.get_item("AdventureWorksDW")

	Copy-Item $script:installers.get_item("AdventureWorksDW") $script:dataPath

	$copiedDatabaseMdf = Get-ChildItem "$script:dataPath\$($databaseMdf.Name)"
	
	[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO')
	$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server 
	
	$stringCollection = new-object System.Collections.Specialized.StringCollection
	$stringCollection.Add($copiedDatabaseMdf)
	
	$server.AttachDatabase("AdventureWorksDW2012", $stringCollection)
}

function DeployAdventureWorksCube {
	$name = "AdventureWorksDW2012Multidimensional-EE"

	$deployWizard = "C:\Program Files (x86)\Microsoft SQL Server\110\Tools\Binn\ManagementStudio\Microsoft.AnalysisServices.Deployment.exe"
	$bids = "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"
	$ssasDirectory = $installers.get_item("AdventureWorksSSASDirectory")
	$solution = "$ssasDirectory\$name.sln"
	
	RunCMD "`"$bids`"" @("`"$solution`"",  "/build") 0
	
	$asdatabaseFile = "$ssasDirectory\$name\bin\$name.asdatabase"
	
	RunCMD "`"$deployWizard`"" @("`"$asdatabaseFile`"", "/S") 0
	
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.AnalysisServices")
	$server = New-Object Microsoft.AnalysisServices.Server
	$server.connect($script:computerName)
	$database = $server.databases[$name]
	$database.Process()
}

function InstallRuby {
	$rubyBin = "C:\Ruby200-x64\bin"

	RunCMD $script:installers.get_item("Ruby") "/Silent" 0
	AddToPath $rubyBin
	RunCMD "Ruby" "$rubyBin\gem","install","albacore" 0
	RunCMD "Ruby" "$rubyBin\gem","install","sass" 0
}

function GetSource {
	RunCMD $script:installers.get_item("GitCredentialWinStore") "-s" 0
	
	RunCMD `
	"C:\Program Files (x86)\Git\bin\git.exe" `
	@("clone", "https://github.com/e82eric/Prompts.git", "C:\Sources") `
	0

	RunCMD `
	"C:\Program Files (x86)\Git\bin\git.exe" `
	@("config", "--global", "user.name", $script:userFullName) `
	0

	RunCMD `
	"C:\Program Files (x86)\Git\bin\git.exe" `
	@("config", "--global", "user.email", $script:userEmail) `
	0
}

function InstallKDiff {
	RunCMD $script:installers.get_item("KDiff3") @("/S") 0
	RunCMD "git" "config","--global","merge.tool","kdiff3" 0
	RunCMD "git" "config","--global","mergetool.kdiff3.path","`"C:\Program Files (x86)\KDiff3\kdiff3.exe`"" 0
}

function InstallWIX {
	RunCMD $script:installers.get_item("WIX") "/Silent" 0
}

function InstallWebServer {
	Import-Module ServerManager
	Add-WindowsFeature Web-Server
	Add-WindowsFeature Web-Asp-Net
	Add-WindowsFeature Web-Windows-Auth
	Add-WindowsFeature Web-Metabase
	Add-WindowsFeature Web-Common-Http
	Add-WindowsFeature Web-Static-Content
	Add-WindowsFeature Web-Default-Doc
	Add-WindowsFeature Web-Dir-Browsing
	Add-WindowsFeature Web-Http-Errors
	Add-WindowsFeature Web-Http-Redirect
	Add-WindowsFeature Web-App-Dev
	Add-WindowsFeature Web-Net-Ext
	Add-WindowsFeature Web-ISAPI-Ext
	Add-WindowsFeature Web-ISAPI-Filter
	Add-WindowsFeature Web-Health
	Add-WindowsFeature Web-Http-Logging
	Add-WindowsFeature Web-Security
	Add-WindowsFeature Web-Basic-Auth
	Add-WindowsFeature Web-Digest-Auth
	Add-WindowsFeature Web-Client-Auth
	Add-WindowsFeature Web-Cert-Auth
	Add-WindowsFeature Web-Url-Auth
	Add-WindowsFeature Web-Filtering
	Add-WindowsFeature Web-IP-Security
	Add-WindowsFeature Web-Performance
	Add-WindowsFeature Web-Stat-Compression
	Add-WindowsFeature Web-Dyn-Compression
	Add-WindowsFeature Web-Mgmt-Tools
	Add-WindowsFeature Web-Mgmt-Console
	Add-WindowsFeature Web-Scripting-Tools
	Add-WindowsFeature Web-Mgmt-Service
	Add-WindowsFeature Web-Mgmt-Compat
	Add-WindowsFeature Web-WMI
	Add-WindowsFeature Web-Lgcy-Scripting
	Add-WindowsFeature Web-Lgcy-Mgmt-Console
}