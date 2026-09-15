# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec {
  [CmdletBinding()]
  param(
    [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
    [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
  )
  & $cmd
  if ($lastexitcode -ne 0) {
    throw ("Exec: " + $errorMessage)
  }
}

[xml]$xmlContent = Get-Content "Directory.Build.props"
$conf = "Release"
$version = $xmlContent.Project.PropertyGroup.Version
$artifacts = ".\deploy\VgCNCServer\v$version"

if (($args.Count -gt 0) -and ($args[0].ToLower() -eq "release")) {
  $conf = "Release"
}
write-host "build with "$conf
if(Test-Path $artifacts) {
    Remove-Item $artifacts -Force -Recurse
}
else {
    New-Item -ItemType "directory" -Path $artifacts
}

# rem build JingWangAgent.sln...

exec { & dotnet clean .\src\VgCNCServer.sln -c Release }
exec { & dotnet build .\src\VgCNCServer.sln -c Release }
exec { & dotnet publish .\src\VgCNCServer\VgCNCServer.csproj -c Release --os win  -o "$artifacts"}
