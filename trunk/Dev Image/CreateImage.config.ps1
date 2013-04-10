$script:computerName = "promptsdev"

$script:installersDirectory = "Z:\Downloads"
$script:sqlServiceAccount = "SqlService"
$script:adminAccount = "Administrator"
$script:isoDrive = "G:"
$script:dataPath = "C:\Data"

$script:password = "pass@word1"
$script:secureStringPassword = ConvertTo-SecureString "$script:password" -AsPlainText -Force

$script:installers = @{
	"DameonTools" = "$script:installersDirectory\DTLite4471-0333.exe";
	"SQLServer2012" = "$script:installersDirectory\en_sql_server_2012_developer_edition_with_sp1_x64_dvd_1228540.iso";
	"VisualStudio" = "$script:installersDirectory\en_visual_studio_ultimate_2012_x86_dvd_920947.iso";
	"Resharper" = "$script:installersDirectory\ReSharperSetup.7.1.2000.1478.msi";
	"Git" = "$installersDirectory\Git-1.8.1.2-preview20130201.exe";
	"GitCredentialWinStore" = "$installersDirectory\git-credential-winstore.exe";
	"AdventureWorksDW" = "$installersDirectory\AdventureWorksDW2012_Data.mdf";
	"AdventureWorksSSASDirectory" = "$installersDirectory\AdventureWorks_SSAS_2012";
	"Ruby" = "$installersDirectory\rubyinstaller-2.0.0-p0-x64.exe";
	"KDiff3" = "$installersDirectory\KDiff3-32bit-Setup_0.9.97.exe";
	"WIX" = "$installersDirectory\wix37.exe";
}

function ValidateInstallers() {
	$script:installers.GetEnumerator() | % {
		if(!(Test-Path $_.Value)) {
			throw "Cannot find $($_.Value)"
		}
	}
}

ValidateInstallers