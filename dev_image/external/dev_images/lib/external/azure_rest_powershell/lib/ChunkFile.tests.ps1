$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".tests.", ".")
. "$here\$sut"

$script:four_mb = 4096 * 1024

Describe "calculate number of blocks" {
	Context "when it is less than 4mb" {
		It "returns 1" {
			$result = _calculateNumberOfBlocks ($script:four_mb -1)
			$result | should equal 1
		}
	}
	Context "when it equals 4mb" {
		It "returns 1" {
			$result = _calculateNumberOfBlocks $script:four_mb
			$result | should equal 1
		}
	}

	Context "when it is between 4mb and 8mb" {
		It "returns  2" {
			$result = _calculateNumberOfBlocks ($script:four_mb + 1)
			$result | should equal 2
		}
	}

	Context "when it equals 8mb" {
		It "returns 2" {
			$result = _calculateNumberOfBlocks ($script:four_mb * 2)
			$result | should equal 2
		}
	}

	Context "when it is between 8mb and 12mb" {
		It "returns  3" {
			$result = _calculateNumberOfBlocks (($script:four_mb * 2) + 1)
			$result | should equal 3 
		}
	}

	Context "when it equals 12mb" {
		It "returns 3" {
			$result = _calculateNumberOfBlocks ($script:four_mb * 3)
			$result | should equal 3
		}
	}
}

Describe "calculate block" {
	$four_mb = 1024 * 4096
	Context "when the stream is at its start" {
		It "returns the file length when it is less than 4mb" {
			$fileLength = $four_mb - 1
			$file = new-object psobject -property @{ Position = 0; Length = $fileLength }
			$result = _calculateBuffer $file
			$result | should equal $fileLength
		}
		It "returns 4mb when the file length is 4mb" {
			$file = new-object psobject -property @{ Position = 0; Length = $four_mb }
			$result = _calculateBuffer $file
			$result | should equal $four_mb 
		}
		It "returns  4mb when the file length is greater than 4mb" {
			$fileLength = $four_mb + 1
			$file = new-object psobject -property @{ Position = 0; Length = $fileLength }	
			$result = _calculateBuffer $file
			$result | should equal $four_mb
		}
	}
	Context "when the file length is greater than 4mb and the file length - position is less than 4mb" {
		It "returns the file length - position" {
			$fileLength = $four_mb + 5
			$position = 6
			$file = new-object psobject -property @{ Position = $position; Length = $fileLength }
			$result = _calculateBuffer $file
			$result | should equal ($fileLength - 6)
		}
	}
	Context "when the file length is greater than 4mb and the file length - position equals 4mb" {
		It "returns 4mb" {
			$fileLength = $four_mb + 5
			$position = 5
			$file = new-object psobject -property @{ Position = $position; Length = $fileLength }
			$result = _calculateBuffer $file
			$result | should equal $four_mb
		}
	}
	Context "when the file length is greater than 4mb and the file length -position is greater than 4mb" {
		It "returns 4mb" {
			$fileLength = $four_mb + 6
			$position = 5
			$file = new-object psobject -property @{ Position = $position; Length = $fileLength }
			$result = _calculateBuffer $file
			$result | should equal $four_mb
		}
	}
}
