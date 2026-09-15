#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["server/VgAutoDrill.Central.WebApi/VgAutoDrill.Central.WebApi.csproj", "server/VgAutoDrill.Central.WebApi/"]
COPY ["VgAutoDrill.Fundation/VgAutoDrill.Fundation.csproj", "VgAutoDrill.Fundation/"]
COPY ["server/VgAutoDrill.Admin.Application/VgAutoDrill.Admin.Application.csproj", "server/VgAutoDrill.Admin.Application/"]
COPY ["server/VgAutoDrill.Admin.Common/VgAutoDrill.Admin.Common.csproj", "server/VgAutoDrill.Admin.Common/"]
COPY ["server/VgAutoDrill.Admin.Domain/VgAutoDrill.Admin.Domain.csproj", "server/VgAutoDrill.Admin.Domain/"]
COPY ["server/VgAutoDrill.Admin.Model/VgAutoDrill.Admin.Model.csproj", "server/VgAutoDrill.Admin.Model/"]
COPY ["server/VgAutoDrill.Admin.Repository/VgAutoDrill.Admin.Repository.csproj", "server/VgAutoDrill.Admin.Repository/"]
COPY ["server/VgAutoDrill.DataCollect.Application/VgAutoDrill.DataCollect.Application.csproj", "server/VgAutoDrill.DataCollect.Application/"]
RUN dotnet restore "server/VgAutoDrill.Central.WebApi/VgAutoDrill.Central.WebApi.csproj"
COPY . .
WORKDIR "/src/server/VgAutoDrill.Central.WebApi"
RUN dotnet build "VgAutoDrill.Central.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VgAutoDrill.Central.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VgAutoDrill.Central.WebApi.dll"]
