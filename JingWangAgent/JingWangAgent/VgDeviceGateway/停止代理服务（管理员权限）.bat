@echo off
set serviceName="VegaAuto.DeviceAgent"
sc stop %serviceName%
::sc delete %serviceName%

pause