@echo off
set port=5051
set exeFile=VgCNCWeb.exe
set serviceName="VgCNCWeb"

:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>
sc start %serviceName%

pause
