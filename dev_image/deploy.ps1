param($vm, $adminUserName, $adminPassword, $gitName, $gitEmail, $workingDirectory, $libDir)
$ErrorPreferenceAction = "stop"

. "$libDir\user.ps1"
. "$libDir\password.ps1"
. "$libDir\remote_session.ps1"

$installers = @{
	Git = "Git-1.8.5.2-preview20131230.exe";
	Kdiff = "KDiff3-32bit-Setup_0.9.97.exe";
	GitCredentialHelper = "git-credential-winstore.exe";
	VisualStudio = "en_visual_studio_ultimate_2013_x86_dvd_3175319.iso";
	DotNet45 = "mu_.net_framework_4.5_r2_x86_x64_1076098.exe";
	NotepadPlusPlus = "npp.6.5.Installer.exe";
	ConEmu = "ConEmuSetup.140214.exe";
	Chrome = "ChromeStandaloneSetup.exe";
	VimPowershellSyntax = "vim-ps1.zip";
	SqlServer = "en_sql_server_2012_enterprise_edition_with_sp1_x64_dvd_1227976.iso"
	AdventureWorksDW = "AdventureWorksDW2012_Data.mdf";
	AdventureWorksSSAS = "AdventureWorks Multidimensional Models SQL Server 2012.zip";
	Ruby = "rubyinstaller-2.0.0-p353-x64.exe";
	WIX = "wix38.exe";
	VirtualCloneDriveCert = "ElaborateBytes.cer";
	VirtualCloneDrive = "SetupVirtualCloneDrive5460.exe";
}

$password = new_password $adminPassword
$adminUser = new_user $adminUserName $password $vm.Name

$vm.SetInstallers($installers)
$vm.SetAdminUser($adminUser)
$vm.SetWorkingDirectory($workingDirectory)
$vm.SetLibDir($libDir)

$vm.Create()

$sqlServiceAccountName = "sql_service"
$dataPath = "C:\sql_data"
$logPath = "C:\sql_log"

$vm.RemoteSession({ param($session)
	$session.Execute({
		Import-Module ServerManager 
		Add-WindowsFeature as-net-framework
	})
})

$vm.Restart()

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context)
		$context.CmdLine.InstallerExecute($context.Installers.DotNet45, @("/passive", "/norestart", "/Log C:\net45.log"), 0)
	})
})

$vm.Restart()

$visualStudioInstallDef = `
'<?xml version="1.0" encoding="utf-8"?>
<AdminDeploymentCustomizations xmlns="http://schemas.microsoft.com/wix/2011/AdminDeployment">
   <BundleCustomizations TargetDir="default" NoWeb="default"/>

   <SelectableItemCustomizations>
	 <SelectableItemCustomization Id="WebTools" Hidden="no" Selected="no"/>
	 <SelectableItemCustomization Id="LightSwitch" Hidden="no" Selected="no"/>
	 <SelectableItemCustomization Id="SilverLight_Developer_Kit" Hidden="no" Selected="no" />
	 <SelectableItemCustomization Id="SQL" Hidden="no" Selected="no" />
	 <SelectableItemCustomization Id="VC_MFC_Libraries" Hidden="no" Selected="no" />
	 <SelectableItemCustomization Id="Blend" Hidden="no" Selected="no" />
	 <SelectableItemCustomization Id="Win8SDK" Hidden="no" Selected="no" />

	 <SelectableItemCustomization Id="BlissHidden" Selected="yes" />
	 <SelectableItemCustomization Id="HelpHidden" Selected="yes" />
	 <SelectableItemCustomization Id="LocalDBHidden" Selected="yes" />
	 <SelectableItemCustomization Id="NetFX4Hidden" Selected="yes" />
	 <SelectableItemCustomization Id="NetFX45Hidden" Selected="yes" />
	 <SelectableItemCustomization Id="PortableDTPHidden" Selected="yes" />
	 <SelectableItemCustomization Id="PreEmptiveDotfuscatorHidden" Selected="yes" />
	 <SelectableItemCustomization Id="PreEmptiveAnalyticsHidden" Selected="no" />
	 <SelectableItemCustomization Id="ProfilerHidden" Selected="yes" />
	 <SelectableItemCustomization Id="ReportingHidden" Selected="yes" />
	 <SelectableItemCustomization Id="SDKTools3Hidden" Selected="yes" />
	 <SelectableItemCustomization Id="SDKTools4Hidden" Selected="yes" />
	 <SelectableItemCustomization Id="Silverlight5DRTHidden" Selected="yes" />
	 <SelectableItemCustomization Id="SQLCEHidden" Selected="yes" />
	 <SelectableItemCustomization Id="SQLCLRTypesHidden" Selected="yes" />
	 <SelectableItemCustomization Id="SQLDACHidden" Selected="yes" />
	 <SelectableItemCustomization Id="SQLDOMHidden" Selected="yes" />
	 <SelectableItemCustomization Id="SQLSharedManagementObjectsHidden" Selected="yes" />
	 <SelectableItemCustomization Id="StoryboardingHidden" Selected="yes" />
	 <SelectableItemCustomization Id="TSQLHidden" Selected="yes" />
	 <SelectableItemCustomization Id="VCCompilerHidden" Selected="yes" />
	 <SelectableItemCustomization Id="VCCoreHidden" Selected="yes" />
	 <SelectableItemCustomization Id="VCDebugHidden" Selected="yes" />
	 <SelectableItemCustomization Id="VCDesigntimeHidden" Selected="yes" />
	 <SelectableItemCustomization Id="VCExtendedHidden" Selected="yes" />
	 <SelectableItemCustomization Id="WinJSHidden" Selected="yes" />
	 <SelectableItemCustomization Id="WinSDKHidden" Selected="yes" />
  </SelectableItemCustomizations>

</AdminDeploymentCustomizations>'

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context, $visualStudioInstallDef)
		$adminFilePath = "$($context.InstallersDirectory)\VisualStudioAdminFile.xml"

		$visualStudioInstallDef | Out-File -Encoding "UTF8" $adminFilePath
		
		$logFile = "$($context.InstallersDirectory)\vs_install_log\vs.log"
		$context.MountIso($context.Installers.VisualStudio)
		$context.CmdLine.IsoExecute("vs_ultimate.exe", @("/passive", "/norestart", "/AdminFile $adminFilePath", "/Log $logFile"), 0)
	},
	@($visualStudioInstallDef))
})

$vm.Restart()

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context, $visualStudioInstallDef)
		$adminFilePath = "$($context.InstallersDirectory)\VisualStudioAdminFile.xml"
		
		$installDef = [xml]$visualStudioInstallDef
		($installDef.AdminDeploymentCustomizations.SelectableItemCustomizations.SelectableItemCustomization | ? { $_.Id -eq "WebTools" }).Selected = "yes"
		$installDef.Save($adminFilePath)
		
		$logFile = "$($context.InstallersDirectory)\vs_install_log\vs.log"
		$context.MountIso($context.Installers.VisualStudio)
		$context.CmdLine.IsoExecute("vs_ultimate.exe", @("/passive", "/norestart", "/AdminFile $adminFilePath", "/Log $logFile"), 3010)
	},
	@($visualStudioInstallDef))
})

$vm.Restart()

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context, $sqlServiceAccountName, $dataPath, $logPath)
		$context.MountIso($context.Installers.SqlServer)

		$serviceAccount = new_user $sqlServiceAccountName $context.AdminUser.Password $context.AdminUser.NetBiosName
		$serviceAccount.Create()
	
		New-Item $dataPath -type directory
		New-Item $logPath -type directory

		$arguments = @(
			"/QUIET=True",
			"/FEATURES=SQLENGINE,SSMS,CONN,ADV_SSMS,AS,RS,IS,BIDS",
			"/ACTION=Install",
			"/X86=False",
			"/ERRORREPORTING=False",
			'/INSTALLSHAREDDIR="C:\Program Files\Microsoft SQL Server"',
			'/INSTALLSHAREDWOWDIR="C:\Program Files (x86)\Microsoft SQL Server"'
			'/INSTANCEDIR="C:\Program Files\Microsoft SQL Server"'
			"/SQMREPORTING=False",
			"/INSTANCENAME=MSSQLSERVER",
			"/AGTSVCACCOUNT=$($serviceAccount.FullName)",
			"/AGTSVCPassword=$($serviceAccount.Password.PlainText)",
			"/AGTSVCSTARTUPTYPE=Automatic",
			"/SQLSVCSTARTUPTYPE=Automatic",
			"/SQLSVCACCOUNT=$($serviceAccount.FullName)",
			"/SQLSVCPassword=$($serviceAccount.Password.PlainText)",
			"/SQLUSERDBDIR=`"$dataPath`"",
			"/SQLUSERDBLOGDIR=`"$logPath`"",
			"/SQLTEMPDBDIR=`"$dataPath`"",
			"/SQLTEMPDBLOGDIR=`"$logPath`"",
			"/TCPENABLED=1",
			"/NPENABLED=0",
			"/BROWSERSVCSTARTUPTYPE=Automatic",
			"/IACCEPTSQLSERVERLICENSETERMS=TRUE",
			"/SQLSYSADMINACCOUNTS=$($context.AdminUser.FullName)",
			"/ASSVCACCOUNT=$($serviceAccount.FullName)",
			"/ASSVCPASSWORD=$($serviceAccount.Password.PlainText)",
			"/ASSYSADMINACCOUNTS=$($context.AdminUser.FullName)",
			"/ISSVCACCOUNT=$($serviceAccount.FullName)"
			"/ISSVCPASSWORD=$($serviceAccount.Password.PlainText)"
			"/RSSVCACCOUNT=$($serviceAccount.FullName)"
			"/RSSVCPASSWORD=$($serviceAccount.Password.PlainText)"
		)

		$context.CmdLine.IsoExecute("Setup.exe", $arguments, 3010)
	},
	@($sqlServiceAccountName, $dataPath, $logPath))
})

$vm.Restart()

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context, $dataPath)
		$databaseMdf = Get-Item "$($context.InstallersDirectory)\$($context.Installers.AdventureWorksDW)"

		Copy-Item $databaseMdf $dataPath

		$copiedDatabaseMdf = Get-Item "$dataPath\$($databaseMdf.Name)"

		[Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO')
		$server = New-Object Microsoft.SqlServer.Management.Smo.Server 

		$stringCollection = New-Object Collections.Specialized.StringCollection
		$stringCollection.Add($copiedDatabaseMdf)

		$server.AttachDatabase("AdventureWorksDW2012", $stringCollection)
	},
	@($dataPath))
})

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context, $gitName, $gitEmail) 
		$context.CmdLine.InstallerExecute($context.Installers.Git, @("/silent"), 0)
		$context.CmdLine.AddToPath("C:\Program Files (x86)\Git\cmd")
		
		$context.CmdLine.InstallerExecute($context.Installers.Kdiff, @("/S"), 0)
		$gitConfigPath = "C:\Users\$($context.AdminUser.Name)\.gitconfig"
		$context.CmdLine.Execute("git", @("config", "--file $gitConfigPath", "core.autocrlf","false"), 0)
		$context.CmdLine.Execute("git", @("config", "--file $gitConfigPath", "merge.tool","kdiff3"), 0)
		$context.CmdLine.Execute("git", @("config", "--file $gitConfigPath", "mergetool.kdiff3.path", "`"C:\Program Files (x86)\KDiff3\kdiff3.exe`""), 0)
		
		$context.CmdLine.Execute("git", @("config", "--file $gitConfigPath", "user.name", "`"$gitName`""), 0)
		$context.CmdLine.Execute("git", @("config", "--file $gitConfigPath", "user.email", $gitEmail), 0)
		
		$context.CmdLine.Execute("git", @("config", "--file $gitConfigPath", "credential.helper", "!'$($context.InstallersDirectory)\$($context.Installers.GitCredentialHelper)'"), 0)

		$context.CmdLine.AddToPath("C:\Program Files (x86)\Git\share\vim\vim73")
		$context.CmdLine.AddToPath("C:\Program Files (x86)\Git\bin")

		$context.CmdLine.Execute("unzip", @("-d `"C:\Program Files (x86)\Git\share\vim\vimfiles`"", "$($context.InstallersDirectory)\$($context.Installers.VimPowershellSyntax)"), 0)
	},
	@($gitName, $gitEmail))
})

$vm.Restart()

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context)
		$context.CmdLine.Execute("unzip", @("-d `"$($context.InstallersDirectory)`"", "`"$($context.InstallersDirectory)\$($context.Installers.AdventureWorksSSAS)`""), 0)

		$deployWizard = "C:\Program Files (x86)\Microsoft SQL Server\110\Tools\Binn\ManagementStudio\Microsoft.AnalysisServices.Deployment.exe"
		$bids = "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

		$name = "AdventureWorksDW2012Multidimensional-EE"
		$solutionDirectory = "$($context.InstallersDirectory)\AdventureWorks Multidimensional Models SQL Server 2012\Enterprise"
		$solution = "$solutionDirectory\$name.sln"

		$context.CmdLine.Execute("`"$bids`"", @("`"$solution`"",  "/build"), 0)

		$asdatabaseFile = "$solutionDirectory\$name\bin\$name.asdatabase"

		$context.CmdLine.Execute("`"$deployWizard`"", @("`"$asdatabaseFile`"", "/S"), 0)

		[Reflection.Assembly]::LoadWithPartialName("Microsoft.AnalysisServices")
		$server = New-Object Microsoft.AnalysisServices.Server
		$server.connect($context.Name)
		$database = $server.databases[$name]
		$database.Process()
	})
})

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context)
		$rubyBin = "C:\Ruby200-x64\bin"

		$context.CmdLine.InstallerExecute($context.Installers.Ruby, @("/Silent"), 0)
		$context.CmdLine.AddToPath($rubyBin)
		$context.CmdLine.Execute("Ruby", @("$rubyBin\gem", "install", "albacore"), 0)
		$context.CmdLine.Execute("Ruby", @("$rubyBin\gem", "install", "sass"), 0)
	})
})

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context)
		$context.CmdLine.InstallerExecute($context.Installers.WIX, @("/Silent"), 0)
	})
})

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context)
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
	})
})

$vm.RemoteSession({ param($session)
	$session.Execute({ param($context)
		$context.CmdLine.InstallerExecute($context.Installers.NotepadPlusPlus, @("/S"), 0)
		$context.CmdLine.AddToPath("C:\Program Files (x86)\Notepad++")
		$context.CmdLine.InstallerExecute($context.Installers.ConEmu, @("/p:x64", "/quiet"), 0)
		$context.CmdLine.InstallerExecute($context.Installers.Chrome, @("/SILENT", "/INSTALL"), 0)
	})
})

$vm.Restart()
