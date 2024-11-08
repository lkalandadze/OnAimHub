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
COPY ["LevelService/LevelService.Api/LevelService.Api.csproj", "LevelService/LevelService.Api/"]
COPY ["LevelService/LevelService.Application/LevelService.Application.csproj", "LevelService/LevelService.Application/"]
COPY ["LevelService/LevelService.Infrastructure/LevelService.Infrastructure.csproj", "LevelService/LevelService.Infrastructure/"]
COPY ["Shared/Shared.Domain/Shared.Domain.csproj", "Shared/Shared.Domain/"]
COPY ["Shared/Shared.Lib/Shared.Lib.csproj", "Shared/Shared.Lib/"]
COPY ["LevelService/LevelService.Domain/LevelService.Domain.csproj", "LevelService/LevelService.Domain/"]
RUN dotnet restore "./LevelService/LevelService.Api/LevelService.Api.csproj"
COPY . .
WORKDIR "/src/LevelService/LevelService.Api"
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