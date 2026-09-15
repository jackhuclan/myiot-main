@echo off
set serviceName="VgDrillFileHub"
sc stop %serviceName%
::sc delete %serviceName%

pause
