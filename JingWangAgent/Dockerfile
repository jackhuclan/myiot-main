FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8004
COPY "./JingWangAgent/VgDeviceGateway/bin/Release/net8.0/linux-x64/publish" .
ENTRYPOINT ["dotnet", "VgDeviceGateway.dll"]
