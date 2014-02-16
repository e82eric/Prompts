$script:bufferSize = 4096 * 1024
function _calculateNumberOfBlocks ($contentLength) {
	if($contentLength % $script:bufferSize -eq 0) {
		$contentLength / $script:bufferSize
	} else {
		$toTruncate = $contentLength / $script:bufferSize
		[Math]::Truncate($toTruncate) + 1	
	}
}

function _calculateBuffer ($stream) {
	$remaining = $stream.Length - $stream.Position
	if($remaining -lt $script:bufferSize) { 
		$remaining
	} else {
		$script:bufferSize
	}
}

function _createBlockId ($blockNumber) {
	$longBlockId = $blockNumber.ToString("d10")
	$utf8BlockId = [Text.Encoding]::UTF8.GetBytes($longBlockId)
	[Convert]::ToBase64String($utf8BlockId)
}

function ChunkFile ($filePath, $chunkFunc) {
	$file = Get-Item $filePath
	$file = new-object IO.FileStream($file.FullName,[IO.FileMode]::Open,[IO.FileAccess]::Read)
	$numberOfBlocks = _calculateNumberOfBlocks $file.Length

	for($i = 0; $i -le $numberOfBlocks - 1; $i++) {
		$blockId = _createBlockId $i
		$bufferLength = _calculateBuffer $file 
		$buffer = new-object byte[] $bufferLength 
		$file.Read($buffer, 0, $bufferLength) | out-null
		$md5 = [Security.Cryptography.MD5]::Create()
		$hashBytes = $md5.ComputeHash($buffer)
		$hash = [Convert]::ToBase64String($hashBytes)

		& $chunkFunc $buffer $blockId $hash
	}	
}
