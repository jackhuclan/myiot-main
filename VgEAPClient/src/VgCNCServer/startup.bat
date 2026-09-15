@echo off
cd %~dp0

for /f "tokens=4" %%a in ('route print^|findstr 0.0.0.0.*0.0.0.0') do (
 set IP=%%a
)

set port=5051

echo 你的局域网IP是：
echo %IP% : %port%
title 钻机监控大屏-请勿关闭--%port%

start "" "http://localhost:%port%/dashboard/drill"
dotnet VgCNCServer.dll --urls="http://*:%port%" --environment=Production

pause
