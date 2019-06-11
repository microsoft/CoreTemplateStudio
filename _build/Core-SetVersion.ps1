
[CmdletBinding()]
Param(

  [Parameter(Mandatory=$True,Position=1)]
  [string]$buildNumber
)

$VersionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)"

if($buildNumber -match $VersionRegEx){

  Write-Output "Parsed Date From Build: $dateFromBuildNumber"

  $revision =  [int]::Parse($matches[4]).ToString()
  
  $versionNumber = [int]::Parse($matches[1]).ToString() + "." + [int]::Parse($matches[2]).ToString() + "." + [int]::Parse($matches[3]).ToString() + "." + $revision
  Write-Host "Version Number" $versionNumber
  
}
else{
	throw "Build format does not match the expected pattern (buildName_w.x.y.z)"
}


## APPLY VERSION TO ASSEMBLY FILES (AssemblyVersion and AssemblyFileVersion)
Write-Host
Write-Host "Applying version to AssemblyInfo files" 
$files = Get-ChildItem -include "*AssemblyInfo.cs" -Recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" }
if($files)
{
    Write-Host "Will apply $versionNumber to $($files.count) files."

    $assemblyVersionRegEx = "\(""$VersionRegex""\)" 
    $assemblyVersionReplacement = "(""$versionNumber"")"

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $assemblyVersionRegEx, $assemblyVersionReplacement | Out-File $file utf8
        Write-Host "$file - version applied"
    }
}
else
{
    Write-Warning "No files found to apply version."
}


## APPLY VERSION TO PROJ FILES 
Write-Host
Write-Host "Applying version to csproj files" 
$files = Get-ChildItem -include "*.csproj" -Recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" }
if($files)
{
    Write-Host "Will apply $versionNumber to $($files.count) files."

    $csprojVersionRegEx = "(<Version>)$VersionRegex(</Version>)" 
    $csprojVersionReplacement = "<Version>$versionNumber</Version>"

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $csprojVersionRegEx, $csprojVersionReplacement | Out-File $file utf8
        Write-Host "$file - version applied"
    }
}
else
{
    Write-Warning "No files found to apply version."
}


