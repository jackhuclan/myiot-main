@echo off
set port=15051
set exeFile=VgCNCWeb.exe
set serviceName="VgCNCWeb"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc stop %serviceName%
sc delete %serviceName%
sc create %serviceName% start=delayed-auto BinPath="%~dp0%exeFile% --urls=http://*:%port% --environment=Production" 
sc failure %serviceName% reset=0 actions=restart/60000/restart/60000/restart/60000
sc description %serviceName% %serviceName%
sc start %serviceName%

start "" "http://localhost:%port%/index.html"

pause
