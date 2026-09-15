@echo off
set serviceName="VgCNCServer"
sc stop %serviceName%
sc delete %serviceName%

pause
