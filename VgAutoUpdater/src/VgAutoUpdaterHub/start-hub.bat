@echo off
set port=5258
set exeFile=VgAutoUpdaterHub.exe
set serviceName="VgAutoUpdaterHub"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc start %serviceName%

pause
