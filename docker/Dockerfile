FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG APP_VERSION=0.0.0-local
ARG APP_INFO_VERSION=${APP_VERSION}
ARG TARGETARCH

WORKDIR /src
COPY src/Hyponome.Web/*.csproj .
RUN dotnet restore -a ${TARGETARCH}

COPY src/Hyponome.Web/. .
RUN dotnet publish -c Release -o /app/publish -p:Version=${APP_VERSION} -p:InformationalVersion=${APP_INFO_VERSION} -a ${TARGETARCH}

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT [ "./Hyponome.Web" ]