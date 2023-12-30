FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/SubscriptionService/SubscriptionService.csproj", "services/SubscriptionService/"]
COPY ["blocks/Camunda/Camunda.csproj", "blocks/Camunda/"]
COPY ["blocks/Core/Core.csproj", "blocks/Core/"]
COPY ["blocks/HealthChecks/HealthChecks.csproj", "blocks/HealthChecks/"]
RUN dotnet restore "services/SubscriptionService/SubscriptionService.csproj"
COPY . .
WORKDIR "/src/services/SubscriptionService"
RUN dotnet build "SubscriptionService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SubscriptionService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriptionService.dll"]
