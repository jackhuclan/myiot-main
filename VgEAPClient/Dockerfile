FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8004
COPY "./JingWangAgent/VgDeviceGateway/bin/Release/net6.0/linux-x64/publish" .
ENTRYPOINT ["dotnet", "VgDeviceGateway.dll"]
