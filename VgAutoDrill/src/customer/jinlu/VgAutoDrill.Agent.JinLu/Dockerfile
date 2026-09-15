#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/src/VgAutoDrill.Agent/VgAutoDrill.Agent.csproj", "/src/code/VgAutoDrill.Agent/"]
COPY ["/src/VgAutoDrill.Fundation/VgAutoDrill.Fundation.csproj", "/src/code/VgAutoDrill.Fundation/"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY "/src/VgAutoDrill.Agent/" "./code/VgAutoDrill.Agent/"
COPY "/src/VgAutoDrill.Fundation" "./code/VgAutoDrill.Fundation/"
RUN dotnet restore "./code/VgAutoDrill.Agent/VgAutoDrill.Agent.csproj"

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./code/VgAutoDrill.Agent/VgAutoDrill.Agent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VgAutoDrill.Agent.dll"]
