@echo off
rem foldershare.bat  share log folder

for /f "skip=4 delims= "  %%i  in ('net share') do (
echo %%i
if "%%i"=="logs"  net share %%i /d
)
set folder=%~dp0%1
echo %folder%
echo %2
net share "%2"=%folder% /GRANT:Everyone,FULL /REMARK:"share  to everyone"