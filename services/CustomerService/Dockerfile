FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/CustomerService/CustomerService.csproj", "services/CustomerService/"]
COPY ["blocks/Core/Core.csproj", "blocks/Core/"]
COPY ["blocks/HealthChecks/HealthChecks.csproj", "blocks/HealthChecks/"]
RUN dotnet restore "services/CustomerService/CustomerService.csproj"
COPY . .
WORKDIR "/src/services/CustomerService"
RUN dotnet build "CustomerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CustomerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.dll"]
