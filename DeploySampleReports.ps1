param($server = $env:computerName, $virtualDirectory = "ReportServer", $rootDirectory = ".\Sample Reports")

$ErrorActionPreference = "Stop"

$script:reportServer = New-WebServiceProxy -Uri "http://$server/$virtualDirectory/ReportService2005.asmx" -UseDefaultCredential
$script:sourceDirectory = Resolve-Path $rootDirectory
$script:server = $server

function createDataSource {
	$script:dataSourceName = "AdventureWorksOLAP"

	$existingDataSource = $script:reportServer.ListChildren($script:dataSourcesPath, "false") | where {$_.Name -eq $script:dataSourceName}

	if($existingDataSource -eq $null) {
		$definition = newSSRSObject "DataSourceDefinition"
		$definition.CredentialRetrieval = 2
		$definition.ConnectString = "Data Source=$script:server;Initial Catalog=AdventureWorksDW2012Multidimensional-EE"
		$definition.Extension = "OLEDB-MD"
		$script:reportServer.CreateDataSource($script:dataSourceName, "$script:dataSourcesPath", "false", $definition, $NULL)
	}
}

function setupDirectories {
	$reportsName = "Reports"
	$reportsPath = "/$reportsName"
	$promptReportsName = "Prompt Reports"
	$script:promptReportsPath = "/$promptReportsName"
	$sampleReportsName = "Sample Reports"
	$script:sampleReportsPath = "$reportsPath/$sampleReportsName"
	$dataSourcesName = "Data Sources"
	$script:dataSourcesPath = "/$dataSourcesName"

	createReportServerFolderIfItDoesNotExist "/" $reportsName
	createReportServerFolderIfItDoesNotExist "/" $dataSourcesName
	createReportServerFolderIfItDoesNotExist "/" $promptReportsName
	
	$existingSampleReportsFolder = $script:reportServer.ListChildren($reportsPath, $True) | Where { $_.Name -eq $sampleReportsName }
	
	if($existingSampleReportsFolder -ne $NULL) {
		$script:reportServer.DeleteItem($script:sampleReportsPath)
	}
	
	$script:reportServer.CreateFolder($sampleReportsName, $reportsPath, $null)
}

function createReportServerFolderIfItDoesNotExist ($parentFolder, $name) {
	$folder = $script:reportServer.ListChildren($parentFolder, $True) | Where { $_.Name -eq $name }
	
	if($folder -eq $NULL) {
		$script:reportServer.CreateFolder($name, $parentFolder, $null)
	}
}

function createReports($sourceFolder, $reportServerFolder) {
	Get-ChildItem "$sourceFolder\*.rdl" | % {
		$definition = [System.IO.File]::ReadAllBytes($_.FullName)
		$reportServer.CreateReport($_.BaseName, $reportServerFolder, "true", $definition, $null)
		setDataSourceReference $_.BaseName $reportServerFolder 
	}
}

function setDataSourceReference($reportName, $reportServerFolder) {
	$reportPath = "$reportServerFolder/$reportName"

	$dataSourceReferences = $script:reportServer.GetItemDataSources($reportPath)
	
	$dataSourceReference = newSSRSObject DataSourceReference
	$dataSourceReference.Reference = "$script:dataSourcesPath/$script:dataSourceName"
	$dataSource = newSSRSObject "DataSource"
	$dataSource.Name = $dataSourceReferences[0].Name
	$dataSource.Item = $dataSourceReference
	
	$reportServer.SetItemDataSources($reportPath, @($dataSource))
}

function newSSRSObject($name) {
	$proxyNamespace = $script:reportServer.GetType().Namespace
	New-Object ("$proxyNamespace.$name")
}

setupDirectories
createDataSource
createReports "$rootDirectory\Reports" $script:sampleReportsPath
createReports "$rootDirectory\Prompt Reports" $script:promptReportsPath