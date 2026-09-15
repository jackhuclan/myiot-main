#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["server/VgAutoDrill.DataCollect.WebApi/VgAutoDrill.DataCollect.WebApi.csproj", "server/VgAutoDrill.DataCollect.WebApi/"]
COPY ["server/VgAutoDrill.DataCollect.Application/VgAutoDrill.DataCollect.Application.csproj", "server/VgAutoDrill.DataCollect.Application/"]
COPY ["VgAutoDrill.Fundation/VgAutoDrill.Fundation.csproj", "VgAutoDrill.Fundation/"]
RUN dotnet restore "server/VgAutoDrill.DataCollect.WebApi/VgAutoDrill.DataCollect.WebApi.csproj"
COPY . .
WORKDIR "/src/server/VgAutoDrill.DataCollect.WebApi"
RUN dotnet build "VgAutoDrill.DataCollect.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VgAutoDrill.DataCollect.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VgAutoDrill.DataCollect.WebApi.dll"]