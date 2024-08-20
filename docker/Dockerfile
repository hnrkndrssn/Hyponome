
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
ARG APP_VERSION=0.0.0-local
ARG APP_INFO_VERSION=${APP_VERSION}

WORKDIR /src
COPY src/ .

WORKDIR /src/Hyponome.Web
RUN dotnet publish -c Release -o /app/publish -p:Version=${APP_VERSION} -p:InformationalVersion=${APP_INFO_VERSION}

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "Hyponome.Web.dll" ]