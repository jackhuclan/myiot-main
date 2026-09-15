#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

#COPY ["VgAutoDrill.Admin.WebApi/VgAutoDrill.Admin.WebApi.csproj", "VgAutoDrill.Admin.WebApi/"]
#COPY ["VgAutoDrill.Admin.Application/VgAutoDrill.Admin.Application.csproj", "VgAutoDrill.Admin.Application/"]
#COPY ["VgAutoDrill.Admin.Common/VgAutoDrill.Admin.Common.csproj", "VgAutoDrill.Admin.Common/"]
#COPY ["VgAutoDrill.Admin.Domain/VgAutoDrill.Admin.Domain.csproj", "VgAutoDrill.Admin.Domain/"]
#COPY ["VgAutoDrill.Admin.Model/VgAutoDrill.Admin.Model.csproj", "VgAutoDrill.Admin.Model/"]
#COPY ["VgAutoDrill.Admin.Repository/VgAutoDrill.Admin.Repository.csproj", "VgAutoDrill.Admin.Repository/"]
#COPY ["VgAutoDrill.Infrastructure.Authentication/VgAutoDrill.Infrastructure.Authentication.csproj", "VgAutoDrill.Infrastructure.Authentication/"]
#RUN dotnet restore "VgAutoDrill.Admin.WebApi/VgAutoDrill.Admin.WebApi.csproj"

COPY . .

#WORKDIR "/src/VgAutoDrill.Admin.WebApi"
#RUN dotnet build "VgAutoDrill.Admin.WebApi.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "VgAutoDrill.Admin.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VgAutoDrill.Admin.WebApi.dll"]