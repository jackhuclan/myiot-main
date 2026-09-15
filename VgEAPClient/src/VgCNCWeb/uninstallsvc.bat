@echo off
set serviceName="VgCNCWeb"
sc stop %serviceName%
sc delete %serviceName%

pause
