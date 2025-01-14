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
COPY ["Core/Hub/Hub.Api/Hub.Api.csproj", "Core/Hub/Hub.Api/"]
COPY ["BuildingBlocks/Shared.IntegrationEvents/Shared.IntegrationEvents.csproj", "BuildingBlocks/Shared.IntegrationEvents/"]
COPY ["BuildingBlocks/Shared.Lib/Shared.Lib.csproj", "BuildingBlocks/Shared.Lib/"]
COPY ["Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/OnAim.Lib.EntityExtension.GlobalAttributes.csproj", "Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/"]
COPY ["Core/Hub/Hub.Application/Hub.Application.csproj", "Core/Hub/Hub.Application/"]
COPY ["BuildingBlocks/Shared.Application/Shared.Application.csproj", "BuildingBlocks/Shared.Application/"]
COPY ["BuildingBlocks/Shared.Domain/Shared.Domain.csproj", "BuildingBlocks/Shared.Domain/"]
COPY ["BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "BuildingBlocks/Shared.Infrastructure/"]
COPY ["Core/Hub/Hub.Domain/Hub.Domain.csproj", "Core/Hub/Hub.Domain/"]
COPY ["Helpers/Lib/OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing/OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.csproj", "Helpers/Lib/OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing/"]
COPY ["Core/Hub/Hub.IntegrationEvents/Hub.IntegrationEvents.csproj", "Core/Hub/Hub.IntegrationEvents/"]
COPY ["Core/Hub/Hub.Infrastructure/Hub.Infrastructure.csproj", "Core/Hub/Hub.Infrastructure/"]
RUN dotnet restore "./Core/Hub/Hub.Api/Hub.Api.csproj"
COPY . .
WORKDIR "/src/Core/Hub/Hub.Api"
RUN dotnet build "./Hub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hub.Api.dll"]