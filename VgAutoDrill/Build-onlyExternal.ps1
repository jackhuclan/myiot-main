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
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

$artifacts = ".\artifacts"
$version = "v3.6.21.1128"
# rem build VgAutoDrill.sln...

exec { & dotnet clean .\src\VgAutoDrill.sln -c Release}
exec { & dotnet build .\src\VgAutoDrill.sln -c Release}

exec { & dotnet build .\src\server\VgAutoDrill.External.WebApi\VgAutoDrill.External.WebApi.csproj -c Release --os linux}

write-host "build with "$conf $version
exec { & docker build -t vegagroup.tech/vgautodrill_external:$version -f ./docs/ExternalDockerfile . }

write-host "build with "$conf $version
exec { & docker save vegagroup.tech/vgautodrill_external:$version -o  vgautodrill_external.tar.gz }

exec { & docker image rm  vegagroup.tech/vgautodrill_external:$version }
