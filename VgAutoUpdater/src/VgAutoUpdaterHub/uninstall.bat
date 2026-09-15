::@echo off
@REM uninstall.bat test VgAutoUpdaterHub v1.2.0.0
set serviceName=%2
set origin="%~dp0%1\%3"
set backup="%~dp0%1\%3-backup"
echo %origin%
echo %backup%

sc stop %serviceName%
sc delete %serviceName%

if exist %backup% (
    del %backup% /f /y
)

if exist %origin% (
    robocopy %origin% %backup% /move
)

pause
