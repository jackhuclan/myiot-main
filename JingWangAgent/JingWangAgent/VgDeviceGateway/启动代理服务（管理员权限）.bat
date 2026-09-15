@echo off
set port=8004
set exeFile=VgDeviceGateway.exe
set serviceName="VegaAuto.DeviceAgent"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc start %serviceName%

pause