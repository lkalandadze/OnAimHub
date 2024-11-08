# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoreModules/LevelService/LevelService.Api/LevelService.Api.csproj", "CoreModules/LevelService/LevelService.Api/"]
COPY ["CoreModules/LevelService/LevelService.Application/LevelService.Application.csproj", "CoreModules/LevelService/LevelService.Application/"]
COPY ["BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "BuildingBlocks/Shared.Infrastructure/"]
COPY ["BuildingBlocks/Shared.Domain/Shared.Domain.csproj", "BuildingBlocks/Shared.Domain/"]
COPY ["BuildingBlocks/Shared.Lib/Shared.Lib.csproj", "BuildingBlocks/Shared.Lib/"]
COPY ["Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/OnAim.Lib.EntityExtension.GlobalAttributes.csproj", "Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/"]
COPY ["BuildingBlocks/Shared.IntegrationEvents/Shared.IntegrationEvents.csproj", "BuildingBlocks/Shared.IntegrationEvents/"]
COPY ["CoreModules/LevelService/LevelService.Infrastructure/LevelService.Infrastructure.csproj", "CoreModules/LevelService/LevelService.Infrastructure/"]
COPY ["CoreModules/LevelService/LevelService.Domain/LevelService.Domain.csproj", "CoreModules/LevelService/LevelService.Domain/"]
RUN dotnet restore "./CoreModules/LevelService/LevelService.Api/LevelService.Api.csproj"
COPY . .
WORKDIR "/src/CoreModules/LevelService/LevelService.Api"
RUN dotnet build "./LevelService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LevelService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LevelService.Api.dll"]