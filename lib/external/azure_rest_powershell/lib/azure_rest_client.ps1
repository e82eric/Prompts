function new_azure_rest_client ($subscriptionId, $cert) {
	$obj = New-Object PSObject -Property @{
		Cert = $cert;
		SubscriptionId = $subscriptionId

	}
        $obj | Add-Member -Type ScriptMethod -Name Request -Value { param ($options)
		$options.Url = "https://management.core.windows.net/$($this.subscriptionId)/$($options.Resource)"

                $content = $options.Content
                $contentType = $null
                $contentLength = $null
                
                if($content -ne $null) {
                        $contentLength = $options.Content.Length
                }
                
                $request = [Net.WebRequest]::Create($options.Url)
                $request.ClientCertificates.Add($this.Cert) | Out-Null
		$request.Headers.Add("x-ms-version", "2013-08-01") | Out-Null

		if($null -eq $options.ContentType) {
			$request.ContentType = "application/xml"
		} else {
			$request.ContentType = $options.ContentType
		}
                $request.Method = $options.Verb

                if($content -ne $null) {
                        $request.ContentLength = $options.Content.Length
                        $requestStream = $request.GetRequestStream()
			$byteArray = [Text.Encoding]::UTF8.GetBytes($options.Content)
                        $requestStream.Write($byteArray, 0, $byteArray.Length) | Out-Null
                        $requestStream.Close() | Out-Null
                }
                
               	$response = $null 
                $result = $null
                
                try {
			$response = $request.GetResponse()
			if($options.OnResponse -ne $null) {
				$result = & $options.OnResponse $response
			}
                }
		catch [Net.WebException] {
			if($null -ne $_.Exception.Response) {
				$stream = $_.Exception.Response.GetResponseStream()
				$reader = New-Object IO.StreamReader($stream)
				$result = $reader.ReadToEnd()
				$stream.Close()
				$reader.Close()
				Write-Host $result
			}
			throw $_
		}
                catch { 
                        throw $_ 
                } finally {
			if($null -ne $response) {
				$response.Close() | Out-Null
			}
                }
                
                $result
        }
	$obj | Add-Member -Type ScriptMethod ExecuteOperation { param ($verb, $resource, $content)
		$this.ExecuteOperation2(@{ Verb = $verb; Resource = $resource; Content = $content; })
	}
	$obj | Add-Member -Type ScriptMethod ExecuteOperation2 { param ($options)
		$options.Add("OnResponse", $parse_operation_id) | Out-Null
		$serviceResult = $this.Request($options)

		$operationResult = $null	
		$status = $null
		while ($true) {
			$operationResult = $this.Request(@{ Verb = "GET"; Resource = "operations/$($serviceResult.OperationId)"; OnResponse = $parse_xml; })
			$status = $operationResult.Operation.Status
			Write-Host $status
			if($operationResult.Body -ne $null) {
				Write-Host $operationResult.Body
			}
			if($status -ne "InProgress") {
				break
			}
			Start-Sleep -s 3
		}
		if($status -ne "Succeeded") {
			$error = $operationResult.OperationResult.Error
			throw "Status: $status Code: $($error.Code) Message: $($error.Message)"
		}
		$operationResult
	}
	$obj
}
                
$parse_operation_xml = { param ($response)
	$operationId = $response.Headers.Get("x-ms-request-id")
        $stream = $response.GetResponseStream()
        $reader = New-Object IO.StreamReader($stream)
        $result = $reader.ReadToEnd()
        $stream.Close()
        $reader.Close()
        $body = [xml]$result
	@{ Body = $body; OperationId = $operationId }
}

$parse_operation_id = { param ($response)
	$operationId = $response.Headers.Get("x-ms-request-id")
        $stream = $response.GetResponseStream()
        $reader = New-Object IO.StreamReader($stream)
        $result = $reader.ReadToEnd()
        $stream.Close()
        $reader.Close()
        $body = $result
	@{ Body = $body; OperationId = $operationId }
}

$parse_xml = { param ($response)
        $stream = $response.GetResponseStream()
        $reader = New-Object IO.StreamReader($stream)
        $result = $reader.ReadToEnd()
        $stream.Close()
        $reader.Close()
        [xml]$result
}
