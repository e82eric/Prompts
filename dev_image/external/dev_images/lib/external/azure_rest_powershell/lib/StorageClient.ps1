$ErrorActionPreference = "Stop"

function _nameValue ($name, $value) {
	New-Object PsObject -Property @{Name = $name; Value = $value }
}

function new_storage_client ($storageName, $storageKey) {
	$obj = New-Object PSObject -Property @{ StorageName = $storageName; StorageKey = $storageKey }
	$obj | Add-Member -Type ScriptMethod -Name _hash -Value { param ($signature)
		$signatureBytes = [Text.Encoding]::UTF8.GetBytes($signature)
		$sixtyFourString = [Convert]::FromBase64String($this.StorageKey)
		$sha256 = New-Object System.Security.Cryptography.HMACSHA256
		$sha256.Key = $sixtyFourString
		$hash = $sha256.ComputeHash($signatureBytes)
		[Convert]::ToBase64String($hash)
	}
	$obj | Add-Member -Type ScriptMethod -Name _createOperationsString -Value { param ($operations)
		if($operations -eq $null) { return [String]::Empty }
		if($operations.Length -eq 0) { return [String]::Empty }
		[String]::Join("`n", $($operations | %  {"$($_.Name.ToLower()):$($_.Value)" } | Sort))
	}
	$obj | Add-Member -Type ScriptMethod -Name _createCanonicalizedResource -Value { param ($account, $uri, $operations)
		$operationsString= $this._createOperationsString($operations)
		if($operationsString -eq [string]::empty) { return "/$account/$uri" } 
		"/$account/$uri`n$operationsString"	
	}
	$obj | Add-Member -Type ScriptMethod -Name _createCanonicalizedHeaders -Value { param ($msHeaders)
		$concatenatedHeaders = $msHeaders | % { "$($_.Name):$($_.Value)" }
		[string]::join("`n", ($concatenatedHeaders | Sort))
	}
	$obj | Add-Member -Type ScriptMethod -Name _createSignature -Value { param ($verb, $canonicalizedHeaders, $canonicalizedResource, $contentLength, $contentType, $contentHash)
		"$($verb.ToUpper())`n`n`n$contentLength`n$contentHash`n$contentType`n`n`n`n`n`n`n$canonicalizedHeaders`n$canonicalizedResource"
	}
	$obj | Add-Member -Type ScriptMethod -Name _createAuthorizationHeader -Value { param ($account, $signaturehash)
		_nameValue "Authorization" "SharedKey $account`:$signaturehash"
	}
	$obj | Add-Member -Type ScriptMethod -Name _splitParameter -Value { param ($parameterString)
		$firstEqualIndex = $parameterString.IndexOf('=')
		$name = $parameterString.SubString(0, $firstEqualIndex)
		$value = $parameterString.SubString($firstEqualIndex + 1, $parameterString.Length - $firstEqualIndex - 1)
		_nameValue $name $value 
	}
	$obj | Add-Member -Type ScriptMethod -Name _splitParameters -Value { param ($queryString)
		$result = $queryString.Split("&") | % { $this._splitParameter($_) } 
		,@($result)
	}
	$obj | Add-Member -Type ScriptMethod -Name _parseUri -Value { param ($uri)
		$accountStartIndex = 7

		if($uri.SubString(0, 5) -eq "https") { $accountStartIndex = 8 }

		$blobDomain = ".blob.core.windows.net"
		$startOfBlobDomain = $uri.indexof($blobDomain)
		$account = $uri.substring($accountStartIndex, $startOfBlobDomain - $accountStartIndex)
		
		$startOfResource = $startOfBlobDomain + $blobDomain.Length + 1
		$indexOfQuestionMark = $uri.indexof("?")

		$lengthOfResource = $uri.Length - $startOfResource
		
		$operations = $null

		if($indexOfQuestionMark -ne -1) {
			$lengthOfResource = $indexOfQuestionMark - $startOfResource 

			$operationString = $uri.SubString($indexOfQuestionMark + 1, $uri.Length - ($indexOfQuestionMark + 1))
			$operations = $this._splitParameters($operationString) 
			Write-Host $operations
		}

		$resource = $uri.substring($startOfResource, $lengthOfResource)

		New-Object PsObject -Property @{Account = $account; Resource = $resource; Operations = $operations }
	}
	$obj | Add-Member -Type ScriptMethod -Name _createMSHeaders -Value { param ($content)
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
	$obj | Add-Member -Type ScriptMethod -Name _createSignatureElements -Value { param ($verb, $urlElements, $msHeaders, $contentLength, $contentType, $contentHash)
		$canonicalizedHeaders = $this._createCanonicalizedHeaders($msHeaders)
		$canonicalizedResource = $this._createCanonicalizedResource($urlElements.Account, $urlElements.Resource, $urlElements.Operations) 
		$signature = $this._createSignature($verb, $canonicalizedHeaders, $canonicalizedResource, $contentLength, $contentType, $contentHash)
		Write-Host "***Signature***"
		Write-Host $signature
		$signatureHash = $this._hash($signature)
		New-Object PsObject -Property @{
			CanonicalizedHeaders = $canonicalizedHeaders; 
			CanonicalizedResource = $canonicalizedResource;
			Signature = $signature;
			SignatureHash = $signatureHash
		}
	}
	$obj | Add-Member -Type ScriptMethod -Name Request -Value { param ($options)
		if($options.Url -eq $null) {
			$options.Url = "http://$($this.StorageName).blob.core.windows.net/$($options.Resource)"
		}
	
		$content = $options.Content
		$contentType = $null
		$contentLength = $null
		
		if($content -ne $null) {
			$contentLength = $options.Content.Length
		}
		
		$request = [Net.WebRequest]::Create($options.Url )
		
		if($options.ContentType -ne $null) {
			$contentType = $options.contentType
			$request.ContentType = $contentType
		}
		
		$urlElements = $this._parseUri($request.RequestUri.AbsoluteUri)
		$request.Method = $options.Verb
		$msHeaders = $this._createMSHeaders()

		$signatureElements = $this._createSignatureElements($request.Method, $urlElements, $msHeaders, $contentLength, $contentType, $null)
		$authorizationHeader = $this._createAuthorizationHeader($urlElements.Account, $signatureElements.SignatureHash)

		$msHeaders | % { $request.Headers.Add($_.Name, $_.Value) }
		$request.Headers.Add($authorizationHeader.Name, $authorizationHeader.Value) | Out-Null
		
		if($content -ne $null) {
			$request.ContentLength = $options.Content.Length
			$requestStream = $request.GetRequestStream()
			$requestStream.Write($options.Content, 0, $options.Content.Length)
			$requestStream.Close()
		}
		
		$response = $request.GetResponse()
		
		$result = $null
		
		try {
			if($options.ProcessResponse -ne $null) {
				$result = & $options.ProcessResponse $response
			}
		}
		catch { 
			throw $_ 
		} finally {
			$response.Close()
		}
		
		$result
	}
	$obj | Add-Member -Type ScriptMethod -Name Put -Value { param($options)
		$options.Verb = "PUT"
		$this.Request($options)
	}
	$obj | Add-Member -Type ScriptMethod -Name Get -Value { param($options)
		$options.Verb = "GET"
		$this.Request($options)
	}
	$obj | Add-Member -Type ScriptMethod -Name Post -Value { param($options)
		$options.Verb = "POST"
		$this.Request($options)
	}
	$obj | Add-Member -Type ScriptMethod -Name Delete -Value { param($options)
		$options.Verb = "DELETE"
		$this.Request($options)
	}
	$obj
}

$download = { param ($response, $filePath)
	$stream = $response.GetResponseStream()
	$file = [System.IO.File]::Create($filePath)
	$buffer = New-Object Byte[] 1024

	Do {
		$bytesRead = $stream.Read($buffer, 0, $buffer.Length)
		$file.Write($Buffer, 0, $BytesRead)
	} While ($bytesRead -gt 0)

	$stream.Close()

	$file.Flush()
	$file.Close()
	$file.Dispose()
}

$parse_xml = { param ($response)
	$stream = $response.GetResponseStream()
	$reader = New-Object IO.StreamReader($stream)
	$result = $reader.ReadToEnd()
	$stream.Close()
	$reader.Close()
	[xml]$result
}