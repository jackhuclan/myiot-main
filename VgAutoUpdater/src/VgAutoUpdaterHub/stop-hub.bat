@echo off
set serviceName="VgAutoUpdaterHub"
sc stop %serviceName%
::sc delete %serviceName%

pause
