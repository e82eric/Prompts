#.\create_azure_network.ps1 `
#	-thumbprint $thumbprint `
#	-subscriptionId $subscriptionId `
#	-networkName "testnet" `
#	-affinityGroup "testuswest" `
#	-networkAddressSpace "192.168.0.0/24" `
#	-subnetAddressPrefix "192.168.0.0/27" `
#	-gatewaySubnetAddressPrefix "192.168.0.32/27" `
#	-vpnAddressPrefix "172.16.1.0/29" `
#	-clientCertificatePassword "pass@word1"

param($subscriptionId, $thumbprint, $networkName, $affinityGroup, $networkAddressSpace, $subnetAddressPrefix, $gatewaySubnetAddressPrefix, $vpnAddressPrefix, $clientCertificatePassword, $workingDir = (Resolve-Path .\).Path, $toolsDir = (Resolve-Path "$workingDir\..\tools").Path, $outDir = "$workingDir\vpn")
$ErrorActionPreference = "Stop"

. "$workingDir\external\azure_remote_powershell\lib\azure_rest_client.ps1"

$cert = Get-Item Cert:\CurrentUser\My\$thumbprint
$restClient = new_azure_rest_client $subscriptionId $cert

$networkDef = $restClient.Request(@{ Verb = "GET"; Resource = "services/networking/media"; OnResponse = $parse_xml })

$newNetworkDef = `
'<?xml version="1.0" encoding="utf-8"?>
<NetworkConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/ServiceHosting/2011/07/NetworkConfiguration" >
	<VirtualNetworkConfiguration>
		<VirtualNetworkSites>
			<VirtualNetworkSite name="{0}" AffinityGroup="{1}">
				<AddressSpace>
					<AddressPrefix>{2}</AddressPrefix>
				</AddressSpace>
				<Subnets>
					<Subnet name="Subnet-1">
						<AddressPrefix>{3}</AddressPrefix>
					</Subnet>
					<Subnet name="GatewaySubnet">
						<AddressPrefix>{4}</AddressPrefix>
					</Subnet>
				</Subnets>
				<Gateway>
					<VPNClientAddressPool>
						<AddressPrefix>{5}</AddressPrefix>
					</VPNClientAddressPool>
					<ConnectionsToLocalNetwork />
				</Gateway>
			</VirtualNetworkSite>
		</VirtualNetworkSites>
	</VirtualNetworkConfiguration>
</NetworkConfiguration>' -f $networkName,$affinityGroup,$networkAddressSpace,$subnetAddressPrefix,$gatewaySubnetAddressPrefix,$vpnAddressPrefix

$newNetworkDoc = New-Object Xml.XmlDocument
$newNetworkDoc.LoadXml($newNetworkDef)
$newNetworkNode = $networkDef.ImportNode($newNetworkDoc.NetworkConfiguration.VirtualNetworkConfiguration.VirtualNetworkSites.FirstChild, $true)

$networkDef.NetworkConfiguration.VirtualNetworkConfiguration.VirtualNetworkSites.AppendChild($newNetworkNode)
$restClient.ExecuteOperation2(@{ Verb = "PUT"; Resource = "services/networking/media"; Content = $networkDef.OuterXml; ContentType = "text/plain" })

$gatewayDef = `
'<CreateGatewayParameters xmlns="http://schemas.microsoft.com/windowsazure">
  <gatewayType>DynamicRouting</gatewayType>
</CreateGatewayParameters>'

$restClient.ExecuteOperation("POST", "services/networking/$networkName/gateway", $gatewayDef)

while($true) {
	$result = $restClient.Request(@{ Verb = "GET"; Resource = "services/networking/$networkName/gateway"; OnResponse = $parse_xml })
	$state = $result.Gateway.State
	Write-Host $state
	if($state -eq "Provisioned") {
		break
	}
	Start-Sleep -s 1
}

$rootCertName = "$($networkName)_root"
$clientCertName = "$($networkName)_client1"

if(!(Test-Path $outDir)) { New-Item $outDir -Type Directory -Force }
& "$toolsDir\makecert.exe" -sky exchange -r -n "CN=$rootCertName" -pe -a sha1 -len 2048 -ss My "$outDir\$($rootCertName).cer"
if($lastExitCode -ne 0) { throw $lastExitCode }
& "$toolsDIr\makecert.exe" -n "CN=$clientCertName" -pe -sky exchange -m 96 -ss My -in "$rootCertName" -is my -a sha1
if($lastExitCode -ne 0) { throw $lastExitCode }

$clientCert = Get-ChildItem Cert:\CurrentUser\My | ? { $_.Subject -eq "CN=$clientCertName" }
$bytes = $clientCert.export([Security.Cryptography.X509Certificates.X509ContentType]::pfx, $clientCertificatePassword)
[IO.File]::WriteAllBytes("$outDir\$($clientCertName)_client1.pfx", $bytes)

$rootCertDef = "<Binary>-----BEGIN CERTIFICATE-----`n$([convert]::ToBase64String([IO.File]::ReadAllBytes("$outDir\$($rootCertName).cer")))`n-----END CERTIFICATE-----</Binary>"
$restClient.ExecuteOperation2(@{ 
	Verb = "POST"; 
	Resource = "services/networking/$networkName/gateway/clientrootcertificates"; 
	Content = $rootCertDef; 
	ContentType = "application/x-www-form-urlencoded" 
})

$vpnDef = `
"<VpnClientParameters xmlns=`"http://schemas.microsoft.com/windowsazure`">
  <ProcessorArchitecture>Amd64</ProcessorArchitecture>
</VpnClientParameters>"
$vpnClientResult = $restClient.ExecuteOperation("POST", "services/networking/$networkName/gateway/vpnclientpackage", $vpnDef)

$url = $null
$status = $null
while($true) {
	$gatewayOperation = $restClient.Request(@{ Verb = "GET"; Resource = "services/networking/operation/$($vpnClientResult.Operation.ID)"; OnResponse = $parse_xml })
	$status = $gatewayOperation.GatewayOperation.Status
	Write-Host $status
	if($status -ne "InProgress") {
		$url = $gatewayOperation.GatewayOperation.Data
		break
	}
	Start-Sleep -s 1
}

$webclient = New-Object Net.WebClient
$webclient.DownloadFile($url, "$outDir\$($networkName)_vpn.exe")
