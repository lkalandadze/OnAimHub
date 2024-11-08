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
COPY ["Games/Wheel/Wheel.Api/Wheel.Api.csproj", "Games/Wheel/Wheel.Api/"]
COPY ["Games/GameLib/GameLib.ServiceRegistry/GameLib.ServiceRegistry.csproj", "Games/GameLib/GameLib.ServiceRegistry/"]
COPY ["Games/GameLib/GameLib.Application/GameLib.Application.csproj", "Games/GameLib/GameLib.Application/"]
COPY ["BuildingBlocks/RNG/RNG.csproj", "BuildingBlocks/RNG/"]
COPY ["BuildingBlocks/Shared.Application/Shared.Application.csproj", "BuildingBlocks/Shared.Application/"]
COPY ["BuildingBlocks/Shared.Domain/Shared.Domain.csproj", "BuildingBlocks/Shared.Domain/"]
COPY ["BuildingBlocks/Shared.Lib/Shared.Lib.csproj", "BuildingBlocks/Shared.Lib/"]
COPY ["Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/OnAim.Lib.EntityExtension.GlobalAttributes.csproj", "Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/"]
COPY ["BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "BuildingBlocks/Shared.Infrastructure/"]
COPY ["BuildingBlocks/Shared.IntegrationEvents/Shared.IntegrationEvents.csproj", "BuildingBlocks/Shared.IntegrationEvents/"]
COPY ["Helpers/Lib/CheckMate/Checkmate.csproj", "Helpers/Lib/CheckMate/"]
COPY ["Games/GameLib/GameLib.Domain/GameLib.Domain.csproj", "Games/GameLib/GameLib.Domain/"]
COPY ["Games/GameLib/GameLib.Infrastructure/GameLib.Infrastructure.csproj", "Games/GameLib/GameLib.Infrastructure/"]
COPY ["Games/Wheel/Wheel.Application/Wheel.Application.csproj", "Games/Wheel/Wheel.Application/"]
COPY ["Games/Wheel/Wheel.Domain/Wheel.Domain.csproj", "Games/Wheel/Wheel.Domain/"]
COPY ["Games/Wheel/Wheel.Infrastructure/Wheel.Infrastructure.csproj", "Games/Wheel/Wheel.Infrastructure/"]
RUN dotnet restore "./Games/Wheel/Wheel.Api/Wheel.Api.csproj"
COPY . .
WORKDIR "/src/Games/Wheel/Wheel.Api"
RUN dotnet build "./Wheel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wheel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wheel.Api.dll"]