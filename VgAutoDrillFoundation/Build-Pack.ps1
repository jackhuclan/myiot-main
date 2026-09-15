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

if (($args.Count -gt 0) -and ($args[0].ToLower() -eq "debug")) {
  $conf = "Debug"
}
write-host "build with "$conf
#if(Test-Path $artifacts) { Remove-Item $artifacts -Force -Recurse }

# rem build VgAutoDrillFoundation.sln...
exec { & dotnet clean .\src\VgAutoDrillFoundation.sln -c $conf }
exec { & dotnet build .\src\VgAutoDrillFoundation.sln -c $conf }

exec { & dotnet pack .\src\tools\VgAutoDrill.OpenAPI\VgAutoDrill.OpenAPI.csproj -c $conf -o $artifacts --no-build}
exec { & dotnet pack .\src\VgAutoDrill.Fundation\VgAutoDrill.Fundation.csproj -c $conf -o $artifacts --no-build}
exec { & dotnet pack .\src\VgAutoDrill.Fundation.CNC\VgAutoDrill.Fundation.CNC.csproj -c $conf -o $artifacts --no-build}
exec { & dotnet pack .\src\VgAutoDrill.Fundation.Iot\VgAutoDrill.Fundation.Iot.csproj -c $conf -o $artifacts --no-build}
exec { & dotnet pack .\src\VgAutoDrill.Fundation.Utils\VgAutoDrill.Fundation.Utils.csproj -c $conf -o $artifacts --no-build}
exec { & dotnet pack .\src\VgAutoDrill.Infrastructure\VgAutoDrill.Infrastructure.csproj -c $conf -o $artifacts --no-build}
exec { & dotnet pack .\src\VegaIot.External.AgvEntity\VegaIot.External.AgvEntity.csproj -c $conf -o $artifacts --no-build}
