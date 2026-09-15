@echo off
cd %~dp0

for /f "tokens=4" %%a in ('route print^|findstr 0.0.0.0.*0.0.0.0') do (
 set IP=%%a
)

set port=8004

echo ÄãµÄ¾ÖÓòÍøIPÊÇ£º
echo %IP% : %port%

::start "" "http://localhost:%port%"

VgAutoDrill.Agent.JinLu.exe "port=%port%"
::;urls=http://localhost:port;http://%IP%:%port%

pause