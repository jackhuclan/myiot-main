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

$version = "v3.10.27.1625"


#exec { & dotnet clean .\src\VgAutoDrill.sln -c Release}
# # rem build adminui...
exec { & cd .\src\adminui }
exec { & yarn build:prod }
exec { & cd .. }
exec { & cd .. }

# # rem build dashboard...
#exec { & cd .\src\dashboard }
#exec { & yarn build:prod }
#exec { & cd .. }
#exec { & cd .. }

write-host "build with "$conf $version
exec { & docker build -t vegagroup.tech/vgautodrill_admin_front:$version -f ./docs/AdminFrontendDockerfile . }
#write-host "build with "$conf $version
#exec { & docker build -t vegagroup.tech/vgautodrill_dashboard:$version -f ./docs/AdminDashboardDockerfile . }

write-host "build with "$conf $version

exec { & docker save vegagroup.tech/vgautodrill_admin_front:$version -o  vgautodrill_admin_front.tar.gz }
#exec { & docker save vegagroup.tech/vgautodrill_dashboard:$version -o  vgautodrill_dashboard.tar.gz }

exec { & docker image rm  vegagroup.tech/vgautodrill_admin_front:$version }
#exec { & docker image rm  vegagroup.tech/vgautodrill_dashboard:$version }