@echo off
set serviceName="VgAutoUpdater"
sc stop %serviceName%
::sc delete %serviceName%

pause
