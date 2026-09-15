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
        # throw ("Exec: " + $errorMessage)
    }
}

$version = "v3.8.19.1618"
#v3.5.30.1304-dnc-pad

#exec { & dotnet clean .\src\VgAutoDrill.sln -c Release}
# # rem build adminui...
exec { & cd .\src\DNC-pad }
exec { & yarn build:prod }
exec { & cd .. }
exec { & cd .. }


write-host "build with "$conf $version
exec { & docker build -t vegagroup.tech/vgautodrill_dnc_pad:$version -f ./docs/DncPadDockerfile . }

write-host "build with "$conf $version

exec { & docker save vegagroup.tech/vgautodrill_dnc_pad:$version -o  vgautodrill_dnc_pad.tar.gz }

exec { & docker image rm  vegagroup.tech/vgautodrill_dnc_pad:$version }