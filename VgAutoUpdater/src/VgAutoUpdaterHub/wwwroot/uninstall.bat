::@echo off
set serviceName=%2
set curdir="%~dp0%1"

sc stop %serviceName%
sc delete %serviceName%

for /f  %%i in ('dir /b /ad "%curdir%"') do (
   echo %%i
     echo %%i| findstr "backup" >nul && (
      echo %%i include"backup"
     ) || (
    echo %%i no include"backup"
    robocopy "%curdir%\%%i" "%curdir%\%%i-backup" /move /s 
    rmdir /q /s "%curdir%\%%i"
    )
)

