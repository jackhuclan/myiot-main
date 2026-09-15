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

# v2.5.30.2008-preview
# rem build VgAutoDrill.sln...

exec { & dotnet clean .\src\VgAutoDrill.sln -c Release}

#exec { & dotnet build .\src\VgAutoDrill.sln -c Release}
#exec { & dotnet pack .\src\server\VgAutoDrill.DataCollect.Application\VgAutoDrill.DataCollect.Application.csproj -c Release -o $artifacts --no-build }


#exec { & dotnet build .\src\server\VgAutoDrill.DataCollect.WebApi\VgAutoDrill.DataCollect.WebApi.csproj -c Release --os linux}
exec { & dotnet publish .\src\server\VgAutoDrill.Admin.WebApi\VgAutoDrill.Admin.WebApi.csproj -c Release --os linux}
exec { & dotnet publish .\src\server\VgAutoDrill.External.WebApi\VgAutoDrill.External.WebApi.csproj -c Release --os linux}

exec { & dotnet pack .\src\server\VgAutoDrill.Admin.Application\VgAutoDrill.Admin.Application.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\server\VgAutoDrill.Admin.Model\VgAutoDrill.Admin.Model.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\server\VgAutoDrill.Admin.Common\VgAutoDrill.Admin.Common.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\server\VgAutoDrill.Admin.Domain\VgAutoDrill.Admin.Domain.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\server\VgAutoDrill.Admin.Repository\VgAutoDrill.Admin.Repository.csproj -c Release -o $artifacts --no-build }

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
#exec { & docker build -t vegagroup.tech/vgautodrill_collect:$version -f ./docs/CollectDockerfile . }
exec { & docker build -t vegagroup.tech/vgautodrill_admin:$version -f ./docs/AdminDockerfile . }
exec { & docker build -t vegagroup.tech/vgautodrill_external:$version -f ./docs/ExternalDockerfile . }

write-host "build with "$conf $version
exec { & docker build -t vegagroup.tech/vgautodrill_admin_front:$version -f ./docs/AdminFrontendDockerfile . }
#write-host "build with "$conf $version
#exec { & docker build -t vegagroup.tech/vgautodrill_dashboard:$version -f ./docs/AdminDashboardDockerfile . }

write-host "build with "$conf $version
#exec { & docker save vegagroup.tech/vgautodrill_collect:$version -o  vgautodrill_collect.tar.gz }
exec { & docker save vegagroup.tech/vgautodrill_admin:$version -o  vgautodrill_admin.tar.gz }
exec { & docker save vegagroup.tech/vgautodrill_external:$version -o  vgautodrill_external.tar.gz }
exec { & docker save vegagroup.tech/vgautodrill_admin_front:$version -o  vgautodrill_admin_front.tar.gz }
#exec { & docker save vegagroup.tech/vgautodrill_dashboard:$version -o  vgautodrill_dashboard.tar.gz }

exec { & docker image rm  vegagroup.tech/vgautodrill_admin:$version }
exec { & docker image rm  vegagroup.tech/vgautodrill_external:$version }
exec { & docker image rm  vegagroup.tech/vgautodrill_admin_front:$version }
#exec { & docker image rm  vegagroup.tech/vgautodrill_dashboard:$version }

#exec { & docker run -e TZ=Asia/Shanghai -d --restart always --name vgautodrill_collect --network vega_network -p 8002:80 vegagroup.tech/vgautodrill_collect:$version}
#exec { & docker run -e TZ=Asia/Shanghai -d --restart always --name vgautodrill_admin --network vega_network -p 8003:80 vegagroup.tech/vgautodrill_admin:$version}
#exec { & docker run -e TZ=Asia/Shanghai -d --restart always --name vgautodrill_admin_front --network vega_network -p 8005:80 vegagroup.tech/vgautodrill_admin_front:$version}

# exec { & docker login https://vegagroup.tech -u hujinping --password-stdin Hujinping123}
# exec { & docker push vegagroup.tech/vgautodrill_admin_front:v1.0.1}