@echo off
set port=5258
set exeFile=VgAutoUpdaterHub.exe
set serviceName="VgAutoUpdaterHub"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc stop %serviceName%
sc delete %serviceName%
sc create %serviceName% start=delayed-auto BinPath="%~dp0%exeFile% --port=%port%"
sc failure %serviceName% reset=0 actions=restart/60000/restart/60000/restart/60000
sc description %serviceName% %serviceName%
sc start %serviceName%

pause
