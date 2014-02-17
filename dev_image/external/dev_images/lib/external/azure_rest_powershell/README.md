Azure-SharedKey-Powershell
==========================

UploadFile
==========================
```powershell
$storageAccount =
$storageAccountKey =
$container =

. .\ChunkFile.ps1
. .\SharedKey.ps1 $storageAccountKey

$fileName =
$filePath =

$script:blockListBody = '<?xml version="1.0" encoding="utf-8"?>
<BlockList>'

ChunkFile $filePath { param($buffer, $blockId, $hash)
	$script:blockListBody += "`n`t<Latest>$blockId</Latest>"
	$url = "http://$storageAccount.blob.core.windows.net/$container/$fileName`?comp=block&blockid=$blockId"
	$request = SharedKey PUT $url $buffer | Close
}

$script:blockListBody += "`n</BlockList>"

$url = "http://$storageAccount.blob.core.windows.net/$container/$script:fileName`?comp=blocklist"

$blockListBytes = [Text.Encoding]::UTF8.GetBytes($script:blockListBody)

SharedKey PUT $url $blockListBytes "text/plain; charset=UTF-8" | Close
```
