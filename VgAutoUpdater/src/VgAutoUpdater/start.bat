@echo off
set port=5257
set exeFile=VgAutoUpdater.exe
set serviceName="VgAutoUpdater"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc start %serviceName%

pause
