FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ThreeLayers.WebApi/ThreeLayers.WebApi.csproj", "src/ThreeLayers.WebApi/"]
COPY ["src/ThreeLayers.Business/ThreeLayers.Business.csproj", "src/ThreeLayers.Business/"]
COPY ["src/ThreeLayers.Data/ThreeLayers.Data.csproj", "src/ThreeLayers.Data/"]
COPY ["src/ThreeLayers.Contracts/ThreeLayers.Contracts.csproj", "src/ThreeLayers.Contracts/"]
RUN dotnet restore "src/ThreeLayers.WebApi/ThreeLayers.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ThreeLayers.WebApi"
RUN dotnet build "./ThreeLayers.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ThreeLayers.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThreeLayers.WebApi.dll"]
