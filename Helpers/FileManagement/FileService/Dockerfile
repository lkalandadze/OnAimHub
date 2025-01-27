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
COPY ["Helpers/FileManagement/FileService/FileService.csproj", "Helpers/FileManagement/FileService/"]
COPY ["BuildingBlocks/Shared.Application/Shared.Application.csproj", "BuildingBlocks/Shared.Application/"]
COPY ["Helpers/Lib/CheckMate/Checkmate.csproj", "Helpers/Lib/CheckMate/"]
COPY ["BuildingBlocks/Shared.Domain/Shared.Domain.csproj", "BuildingBlocks/Shared.Domain/"]
COPY ["BuildingBlocks/Shared.Lib/Shared.Lib.csproj", "BuildingBlocks/Shared.Lib/"]
COPY ["Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/OnAim.Lib.EntityExtension.GlobalAttributes.csproj", "Helpers/Lib/OnAim.Lib.EntityExtension.GlobalAttributes/"]
COPY ["BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "BuildingBlocks/Shared.Infrastructure/"]
COPY ["BuildingBlocks/Shared.IntegrationEvents/Shared.IntegrationEvents.csproj", "BuildingBlocks/Shared.IntegrationEvents/"]
RUN dotnet restore "./Helpers/FileManagement/FileService/FileService.csproj"
COPY . .
WORKDIR "/src/Helpers/FileManagement/FileService"
RUN dotnet build "./FileService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileService.dll"]