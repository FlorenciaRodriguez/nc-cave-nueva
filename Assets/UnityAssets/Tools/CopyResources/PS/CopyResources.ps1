param
(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$ProjectDir,
    [Parameter(Mandatory = $true, Position = 1)]
    [string]$PathToBuildProject
)


Start-Transcript -Path "$PSScriptRoot\Logs\CopyResources_$(Get-Date -Format 'yyyyMMdd_hhmmss').log" -Append

Function PrintErrorMessage([string]$Message, [int]$ErrorCode) {
	Write-Host "ERROR: Exiting with code $ErrorCode - $Message" -ForegroundColor Red
	
    try {
	    Stop-Transcript | Out-Null
	}
	catch [System.InvalidOperationException] {
    }

	Exit $ErrorCode
}

Function CreateFolderIfNotExists([string]$Path) {
	if(-Not(Test-Path -Path $Path)) {
		New-Item -ItemType Directory -Path $Path | Out-Null
	}
}

Function CopyItemInFolder([string]$ItemPath, [string]$FolderDestination) {
    CreateFolderIfNotExists -Path $FolderDestination
    Copy-Item -Path $ItemPath -Destination $FolderDestination -Recurse -Force | Out-Null
}

try {
    $DirectoryExecutable = (Get-Item $PathToBuildProject).DirectoryName
    $DirectoryData =  Join-Path $DirectoryExecutable ((Get-Item $PathToBuildProject).BaseName + "_Data")

    Write-Host
    Write-Host "Project directory: " $ProjectDir
    Write-Host "Path to build project: " $PathToBuildProject
    Write-Host "Directory executable: " $DirectoryExecutable
    Write-Host "Directory data: " $DirectoryData
    Write-Host

    [System.Boolean]$IsError = $false

    [xml]$ConfigFile = Get-Content "$PSScriptRoot\CopyResources.xml"

    if ($ConfigFile.Resources.ExecutableFolder.CopyResources -ne $null)
    {
        foreach($CopyResource in $ConfigFile.Resources.ExecutableFolder.CopyResources.CopyResource) 
        { 
            $Origin = Join-Path $ProjectDir $CopyResource.Origin
            $FolderDestination = Join-Path $DirectoryExecutable $CopyResource.FolderDestination

            if (Test-Path($Origin)) {
                $Origin = Resolve-Path $Origin
                $FolderDestination = [System.IO.Path]::GetFullPath($FolderDestination)

                CopyItemInFolder -ItemPath $Origin -FolderDestination $FolderDestination   
            } else {
                $IsError = $true
            }   
        }
    }

    if ($ConfigFile.Resources.DataFolder.CopyResources -ne $null)
    {
        foreach($CopyResource in $ConfigFile.Resources.DataFolder.CopyResources.CopyResource) 
        {       
            $Origin = Join-Path $projectDir $CopyResource.Origin
            $FolderDestination = Join-Path $DirectoryData $CopyResource.FolderDestination

            if (Test-Path($Origin)) {
                $Origin = Resolve-Path $Origin
                $FolderDestination = [System.IO.Path]::GetFullPath($FolderDestination)

                CopyItemInFolder -ItemPath $Origin -FolderDestination $FolderDestination   
            } else {
                $IsError = $true
            }   
        }
    }
    
    if ($IsError) {
        Exit 2
    }   
} 
catch {
	PrintErrorMessage -Message "Exception in PS Script: $($_.Exception.Message) - $($_.Exception.ItemName)" -ErrorCode 1
}