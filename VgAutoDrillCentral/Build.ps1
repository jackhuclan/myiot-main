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

$artifacts = ".\artifacts"

$conf = "Release"
$version = "v3.10.25.1510-dev-central"

# v2.6.1.1932-preview

if (($args.Count -gt 0) -and ($args[0].ToLower() -eq "debug")) {
  $conf = "debug"
}
write-host "build with "$conf
#if(Test-Path $artifacts) { Remove-Item $artifacts -Force -Recurse }

# rem build VgAutoDrillCentral.sln...
 exec { & dotnet clean .\src\VgAutoDrillCentral.sln -c $conf }
 exec { & dotnet build .\src\VgAutoDrillCentral.sln -c $conf }

#exec { & dotnet clean .\src\VgAutoDrillCentral.sln -c Release }
#exec { & dotnet build .\src\VgAutoDrillCentral.sln -c Release }
exec { & dotnet publish .\src\VgAutoDrill.Central.WebApi\VgAutoDrill.Central.WebApi.csproj -c Release --os linux }
exec { & docker build -t vegagroup.tech/vgautodrill_central:$version -f ./docs/CentralDockerfile . }
exec { & docker save vegagroup.tech/vgautodrill_central:$version -o  vgautodrill_central.tar.gz }
exec { & docker image rm  vegagroup.tech/vgautodrill_central:$version }

#exec { & dotnet test .\test\UnitTest.VgAutoDrill.Fundation\UnitTest.VgAutoDrill.Fundation.csproj -c Release --no-build -l trx --verbosity=normal }
#exec { & docker run -d --name vgautodrill_central --network vega_network -p 8001:80 -p 1883:1883 -v vol_central_conf:/app/conf -v vol_central_data:/app/data -v vol_central_logs:/app/logs vegagroup.tech/vgautodrill_central:$version}
