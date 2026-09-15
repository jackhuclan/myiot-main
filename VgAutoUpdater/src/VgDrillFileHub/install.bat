@echo off
rem install.bat VgAutoUpdaterHub.exe VgAutoUpdaterHub 5258
set exeFile=%1
set serviceName=%2
set port=%3

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc stop %serviceName%
sc delete %serviceName%
sc create %serviceName% start=auto BinPath="%~dp0%exeFile% --port=%port%"
sc failure %serviceName% reset=0 actions=restart/60000/restart/60000/restart/60000
sc description %serviceName% %serviceName%
sc start %serviceName%

pause
