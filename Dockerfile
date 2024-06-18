FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /source

# Node.js
RUN apt-get update -yq
RUN apt-get install curl gnupg -yq
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash -
RUN apt-get install -y nodejs
RUN rm -rf /var/lib/apt/lists/*

# Restore
COPY *.sln .
COPY src/Meowy/*.csproj ./src/Meowy/
RUN dotnet restore -a "$TARGETARCH"

COPY src/Meowy/. ./src/Meowy/
WORKDIR /source/src/Meowy

# NPM
RUN npm install

# Publish
RUN dotnet publish -c Release -a "$TARGETARCH" --no-restore -o /app

# Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app .

RUN chown -R "$APP_UID" /app

RUN mkdir -p /home/app/.aspnet/DataProtection-Keys
RUN chown "$APP_UID" /home/app/.aspnet/DataProtection-Keys

VOLUME /home/app/.aspnet/DataProtection-Keys

USER $APP_UID

EXPOSE 8080/tcp
EXPOSE 8080/udp

ENTRYPOINT [ "dotnet", "./Meowy.dll" ]
