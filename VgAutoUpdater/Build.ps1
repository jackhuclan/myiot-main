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

$conf = "Release"
$artifacts = ".\artifacts"
$version = "v2.3.3.0940"


if (($args.Count -gt 0) -and ($args[0].ToLower() -eq "release")) {
    $conf = "Release"
}
write-host "build with "$conf
#if(Test-Path $artifacts) { Remove-Item $artifacts -Force -Recurse }

#rem build VgAutoDrillFundation.sln...
exec { & dotnet clean .\src\VgAutoUpdater.sln -c $conf }
exec { & dotnet build .\src\VgAutoUpdater.sln -c $conf }

#rem build VgAutoUpdaterHub image
exec { & dotnet publish .\src\VgAutoUpdaterHub\VgAutoUpdaterHub.csproj -c Release --os linux }
exec { & docker build -t vegagroup.tech/vgautoupdaterhub:$version -f ./src/VgAutoUpdaterHub/Dockerfile . }
exec { & docker save vegagroup.tech/vgautoupdaterhub:$version -o  $artifacts\vgautoupdaterhub.tar.gz }
exec { & docker image rm  vegagroup.tech/vgautoupdaterhub:$version }

#exec { & docker run -e TZ=Asia/Shanghai -d --restart always --name vgautoupdaterhub -v /root/updates:/app/wwwroot --network vega_network -p 5258:5258 vegagroup.tech/vgautoupdaterhub:$version}
