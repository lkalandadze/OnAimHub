#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OnAim.Admin.API/OnAim.Admin.API.csproj", "OnAim.Admin.API/"]
COPY ["OnAim.Admin.APP/OnAim.Admin.APP.csproj", "OnAim.Admin.APP/"]
COPY ["OnAim.Admin.Infrasturcture/OnAim.Admin.Infrasturcture.csproj", "OnAim.Admin.Infrasturcture/"]
COPY ["OnAim.Admin.Shared/OnAim.Admin.Shared.csproj", "OnAim.Admin.Shared/"]
RUN dotnet restore "./OnAim.Admin.API/OnAim.Admin.API.csproj"
COPY . .
WORKDIR "/src/OnAim.Admin.API"
RUN dotnet build "./OnAim.Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OnAim.Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnAim.Admin.API.dll"]