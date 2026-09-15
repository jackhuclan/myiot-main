@echo off
set port=5051
set exeFile=VgCNCServer.exe
set serviceName="VgCNCServer"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc start %serviceName%

pause
