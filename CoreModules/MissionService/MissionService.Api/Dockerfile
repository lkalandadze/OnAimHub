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
COPY ["CoreModules/MissionService/MissionService.Api/MissionService.Api.csproj", "CoreModules/MissionService/MissionService.Api/"]
COPY ["CoreModules/MissionService/MissionService.Application/MissionService.Application.csproj", "CoreModules/MissionService/MissionService.Application/"]
COPY ["BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "BuildingBlocks/Shared.Infrastructure/"]
COPY ["BuildingBlocks/Shared.Domain/Shared.Domain.csproj", "BuildingBlocks/Shared.Domain/"]
COPY ["BuildingBlocks/Shared.Lib/Shared.Lib.csproj", "BuildingBlocks/Shared.Lib/"]
COPY ["Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/OnAim.Lib.EntityExtension.GlobalAttributes.csproj", "Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/"]
COPY ["BuildingBlocks/Shared.IntegrationEvents/Shared.IntegrationEvents.csproj", "BuildingBlocks/Shared.IntegrationEvents/"]
COPY ["CoreModules/Leaderboard/Leaderboard.Application/Leaderboard.Application.csproj", "CoreModules/Leaderboard/Leaderboard.Application/"]
COPY ["Core/Hub/Hub.Domain/Hub.Domain.csproj", "Core/Hub/Hub.Domain/"]
COPY ["Helpers/Lib/OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing/OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.csproj", "Helpers/Lib/OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing/"]
COPY ["CoreModules/Leaderboard/Leaderboard.Infrastructure/Leaderboard.Infrastructure.csproj", "CoreModules/Leaderboard/Leaderboard.Infrastructure/"]
COPY ["CoreModules/Leaderboard/Leaderboard.Domain/Leaderboard.Domain.csproj", "CoreModules/Leaderboard/Leaderboard.Domain/"]
COPY ["CoreModules/MissionService/MissionService.Infrastructure/MissionService.Infrastructure.csproj", "CoreModules/MissionService/MissionService.Infrastructure/"]
COPY ["CoreModules/MissionService/MissionService.Domain/MissionService.Domain.csproj", "CoreModules/MissionService/MissionService.Domain/"]
RUN dotnet restore "./CoreModules/MissionService/MissionService.Api/MissionService.Api.csproj"
COPY . .
WORKDIR "/src/CoreModules/MissionService/MissionService.Api"
RUN dotnet build "./MissionService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MissionService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MissionService.Api.dll"]