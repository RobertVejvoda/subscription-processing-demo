# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
# https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.alpine-non-root

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src
EXPOSE 8080

# copy csproj and restore as distinct layers
COPY ["blocks/Core/Core.csproj", "src/blocks/Core/"]
COPY ["blocks/EventBus/EventBus.csproj", "src/blocks/EventBus/"]
COPY ["blocks/HealthChecks/HealthChecks.csproj", "src/blocks/HealthChecks/"] 
COPY ["services/ODSService/ODSService.csproj", "src/services/ODSService/"]
RUN dotnet restore "src/services/ODSService/ODSService.csproj" -a $TARGETARCH

# copy everything else and build app
COPY . .
WORKDIR "/src/services/ODSService"
RUN dotnet build "ODSService.csproj" -c Release -o /app/build -a $TARGETARCH

FROM build AS publish
RUN dotnet publish "ODSService.csproj" -a $TARGETARCH -o /app/publish --no-restore 

# Enable globalization and time zones:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=publish /app/publish .

USER $APP_UID
ENTRYPOINT ["./ODSService"]