# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
# Add-on
ARG BUILD_FROM
FROM $BUILD_FROM as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["src/EnergyAssistantLib/EnergyAssistantGui.csproj", "src/EnergyAssistantGui"]
RUN dotnet restore "src/EnergyAssistantGui"
COPY . .
WORKDIR "/src/src/EnergyAssistantGui"
RUN dotnet build -c Release -o /app/build

ARG BUILD_ARCH
FROM build AS publish
# RUN dotnet publish -c Release --self-contained false -o /app/publish
RUN dotnet publish -a $TARGETARCH --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app

RUN apk add --no-cache dotnet7-runtime

COPY run.sh /
RUN chmod a+x /run.sh
CMD [ "/run.sh" ]