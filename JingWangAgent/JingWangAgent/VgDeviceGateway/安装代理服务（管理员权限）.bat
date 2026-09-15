@echo off
set port=8004
set exeFile=VgDeviceGateway.exe
set serviceName="VegaAuto.DeviceAgent"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc stop %serviceName%
sc delete %serviceName%
sc create %serviceName% start=delayed-auto BinPath="%~dp0%exeFile% --port=%port%" 
sc failure %serviceName% reset=0 actions=restart/60000/restart/60000/restart/60000
sc description %serviceName% %serviceName%
sc start %serviceName%

pause