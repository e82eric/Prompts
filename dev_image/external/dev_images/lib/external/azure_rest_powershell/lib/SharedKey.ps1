param ($storageKey)
$ErrorActionPreference = "Stop"
$script:storageKey = $storageKey

function _nameValue ($name, $value) {
	New-Object PsObject -Property @{Name = $name; Value = $value }
}

function _hash ($signature) {
	$signatureBytes = [Text.Encoding]::UTF8.GetBytes($signature)
	$sixtyFourString = [Convert]::FromBase64String($script:storageKey)
	$sha256 = New-Object System.Security.Cryptography.HMACSHA256
	$sha256.Key = $sixtyFourString
	$hash = $sha256.ComputeHash($signatureBytes)
	[Convert]::ToBase64String($hash)
}

function _createOperationsString ($operations) {
	if($operations -eq $null) { return [String]::Empty }
	if($operations.Length -eq 0) { return [String]::Empty }
	[String]::Join("`n", $($operations | %  {"$($_.Name.ToLower()):$($_.Value)" } | Sort))
}

function _createCanonicalizedResource ($account, $uri, $operations) {
	$operationsString= _createOperationsString $operations
	if($operationsString -eq [string]::empty) { return "/$account/$uri" } 
	"/$account/$uri`n$operationsString"	
}

function _createCanonicalizedHeaders ($msHeaders) {
	$concatenatedHeaders = $msHeaders | % { "$($_.Name):$($_.Value)" }
	[string]::join("`n", ($concatenatedHeaders | Sort))
}

function _createSignature ($verb, $canonicalizedHeaders, $canonicalizedResource, $contentLength, $contentType, $contentHash) {
	"$($verb.ToUpper())`n`n`n$contentLength`n$contentHash`n$contentType`n`n`n`n`n`n`n$canonicalizedHeaders`n$canonicalizedResource"
}

function _createAuthorizationHeader ($account, $signaturehash) {
	_nameValue "Authorization" "SharedKey $account`:$signaturehash"
}

function _splitParameter ($parameterString) {
	$firstEqualIndex = $parameterString.IndexOf('=')
	$name = $parameterString.SubString(0, $firstEqualIndex)
	$value = $parameterString.SubString($firstEqualIndex + 1, $parameterString.Length - $firstEqualIndex - 1)
	_nameValue $name $value 
}

function _splitParameters ($queryString) {
	$result = $queryString.Split("&") | % { _splitParameter $_ } 
	,@($result)
}

function _parseUri ($uri) {
	$accountStartIndex = 7

	if($uri.SubString(0, 5) -eq "https") { $accountStartIndex = 8 }

	$blobDomain = ".blob.core.windows.net"
	$startOfBlobDomain = $uri.indexof($blobDomain);
	$account = $uri.substring($accountStartIndex, $startOfBlobDomain - $accountStartIndex)
	
	$startOfResource = $startOfBlobDomain + $blobDomain.Length + 1
	$indexOfQuestionMark = $uri.indexof("?")

	$lengthOfResource = $uri.Length - $startOfResource
	
	$operations = $null

	if($indexOfQuestionMark -ne -1) {
		$lengthOfResource = $indexOfQuestionMark - $startOfResource 

		$operationString = $uri.SubString($indexOfQuestionMark + 1, $uri.Length - ($indexOfQuestionMark + 1))
		$operations = _splitParameters $operationString 
		Write-Host $operations
	}

	$resource = $uri.substring($startOfResource, $lengthOfResource)

	New-Object PsObject -Property @{Account = $account; Resource = $resource; Operations = $operations }
}

function _createMSHeaders ($content) {
	$now = [DateTime]::UtcNow.ToString("R", [Globalization.CultureInfo]::InvariantCulture)
	$dateHeader = _nameValue "x-ms-date" $now
	$versionHeader = _nameValue "x-ms-version" "2009-09-19"
	$result = @($dateHeader, $versionHeader)
	if($content -ne $null) {
	$blobHeader = _nameValue "x-ms-blob-type" "BlockBlob"
	$result = $result + $blobHeader
	}
	$result
}

function _createSignatureElements ($verb, $urlElements, $msHeaders, $contentLength, $contentType, $contentHash) {
	$canonicalizedHeaders = _createCanonicalizedHeaders $msHeaders
	$canonicalizedResource = _createCanonicalizedResource $urlElements.Account $urlElements.Resource $urlElements.Operations 
	$signature = _createSignature $verb $canonicalizedHeaders $canonicalizedResource $contentLength $contentType $contentHash
	Write-Host $signature
	$signatureHash = _hash $signature
	New-Object PsObject -Property @{
		CanonicalizedHeaders = $canonicalizedHeaders; 
		CanonicalizedResource = $canonicalizedResource;
		Signature = $signature;
		SignatureHash = $signatureHash
	}
}

function SharedKey ($verb, $url, $content, $contentType, $contentHash) {
	$contentLength = $content.Length
	$request = [Net.WebRequest]::Create($url)
	$request.ContentType = $contentType
	$urlElements = _parseUri $request.RequestUri.AbsoluteUri
	$request.Method = $verb
	$msHeaders = _createMSHeaders $request.Content
	
	$signatureElements = _createSignatureElements $verb $urlElements $msHeaders $contentLength $contentType $contentHash
	$authorizationHeader = _createAuthorizationHeader $urlElements.Account $signatureElements.SignatureHash
	
	$msHeaders | % { $request.Headers.Add($_.Name, $_.Value) }
	$request.Headers.Add($authorizationHeader.Name, $authorizationHeader.Value) | Out-Null

	if($contentHash -ne $null) {
		Write-Host "Adding Hash Header"
		$request.Headers.Add("Content-MD5", $contentHash)
	}

	if($content -ne $null) {
		$request.ContentLength = $contentLength 
		$requestStream = $request.GetRequestStream()
		$requestStream.Write($content, 0, $contentLength)
		$requestStream.Close()
	}
	

	$response = $request.GetResponse()
	
	$result = New-Object PsObject -Property @{ 
		WebRequest = $request;
		UrlElements = $urlElements;
		SignatureElements = $signatureElements;
		Response = $response
	}

	$result
}

function AsXml {
	process {
		$stream = $_.Response.GetResponseStream()
		$reader = New-Object IO.StreamReader($stream)
		$result = $reader.ReadToEnd()
		$_.Response.Close()
		$stream.Close()
		$reader.Close()
		[xml]$result
	}
}

function Download ($filePath) {
	process {
		$stream = $_.Response.GetResponseStream()
		$file = [System.IO.File]::Create($filePath)
		$buffer = New-Object Byte[] 1024

		Do {
			$bytesRead = $stream.Read($buffer, 0, $buffer.Length)
			$file.Write($Buffer, 0, $BytesRead)
		} While ($bytesRead -gt 0)

		$_.Response.Close()
		$stream.Close()

		$file.Flush()
		$file.Close()
		$file.Dispose()

	}
}

function Close {
	process {
		Write-Host $_.Signature
		$_.Response.Close()
	}
}
