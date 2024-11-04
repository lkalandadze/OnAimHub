#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Admin/OnAim.Admin.API/OnAim.Admin.API.csproj", "Admin/OnAim.Admin.API/"]
COPY ["Admin/OnAim.Admin.APP/OnAim.Admin.APP.csproj", "Admin/OnAim.Admin.APP/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]
COPY ["Shared/Shared.Domain/Shared.Domain.csproj", "Shared/Shared.Domain/"]
COPY ["Shared/Shared.Lib/Shared.Lib.csproj", "Shared/Shared.Lib/"]
COPY ["Shared/Shared.IntegrationEvents/Shared.IntegrationEvents.csproj", "Shared/Shared.IntegrationEvents/"]
COPY ["Admin/OnAim.Admin.CrossCuttingConcerns/OnAim.Admin.CrossCuttingConcerns.csproj", "Admin/OnAim.Admin.CrossCuttingConcerns/"]
COPY ["Admin/OnAim.Admin.Contracts/OnAim.Admin.Contracts.csproj", "Admin/OnAim.Admin.Contracts/"]
COPY ["Admin/OnAim.Admin.Infrasturcture/OnAim.Admin.Infrasturcture.csproj", "Admin/OnAim.Admin.Infrasturcture/"]
COPY ["Admin/OnAim.Admin.Domain/OnAim.Admin.Domain.csproj", "Admin/OnAim.Admin.Domain/"]
RUN dotnet restore "./Admin/OnAim.Admin.API/OnAim.Admin.API.csproj"
COPY . .
WORKDIR "/src/Admin/OnAim.Admin.API"
RUN dotnet build "./OnAim.Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OnAim.Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnAim.Admin.API.dll"]