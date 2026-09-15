@echo off
set exeFile=%1
set serviceName=%2
set port=%3
:: 启动类型：<boot|system|auto|demand|disabled|delayed-auto>

sc stop %serviceName%
sc delete %serviceName%
sc create %serviceName% start=auto  BinPath="%~dp0%exeFile% --port=%port%"
sc description %serviceName% "current-version-%5"
sc start %serviceName%
