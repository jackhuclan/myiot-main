@echo off

set port=18001
title 中转位程序 WH01--%port%
set ASPNETCORE_ENVIRONMENT=Production
::set ASPNETCORE_ENVIRONMENT=Development
dotnet VgDeviceGateway.dll --port=%port%

pause