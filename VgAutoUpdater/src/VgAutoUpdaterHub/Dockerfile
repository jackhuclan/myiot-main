FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5258
COPY "./src/VgAutoUpdaterHub/bin/Release/net6.0/linux-x64/publish" .
ENTRYPOINT ["dotnet", "VgAutoUpdaterHub.dll"]
