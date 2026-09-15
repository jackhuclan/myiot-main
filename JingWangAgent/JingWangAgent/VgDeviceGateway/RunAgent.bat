@echo off
cd %~dp0

for /f "tokens=4" %%a in ('route print^|findstr 0.0.0.0.*0.0.0.0') do (
 set IP=%%a
)

set port=8004

echo 你的局域网IP是：
echo %IP% : %port%
title 中转位程序 WH01--%port%

start "" "http://localhost:%port%"

VgDeviceGateway.exe "port=%port%"
::;urls=http://localhost:port;http://%IP%:%port%

pause