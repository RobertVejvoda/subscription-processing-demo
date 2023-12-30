FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/ODSService/ODSService.csproj", "services/ODSService/"]
COPY ["blocks/Core/Core.csproj", "blocks/Core/"]
COPY ["blocks/HealthChecks/HealthChecks.csproj", "blocks/HealthChecks/"]
RUN dotnet restore "services/ODSService/ODSService.csproj"
COPY . .
WORKDIR "/src/services/ODSService"
RUN dotnet build "ODSService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ODSService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ODSService.dll"]
