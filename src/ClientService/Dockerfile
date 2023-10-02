# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
# https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.alpine-non-root

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src
EXPOSE 8080

# copy csproj and restore as distinct layers
COPY "./ClientService/ClientService.csproj" .
RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY "./ClientService/." .
RUN dotnet publish -a $TARGETARCH -o /app --no-restore 

# Enable globalization and time zones:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app .

USER $APP_UID
ENTRYPOINT ["./ClientService"]

