# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
# Add-on
ARG BUILD_FROM
FROM $BUILD_FROM as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["src/EnergyAssistant/EnergyAssistant.fsproj", "src/EnergyAssistant/"]
RUN dotnet restore "src/EnergyAssistant/EnergyAssistant.fsproj"
COPY . .
WORKDIR "/src/src/EnergyAssistant"
RUN dotnet build "EnergyAssistant.fsproj" -c Release -o /app/build

ARG BUILD_ARCH
FROM build AS publish
RUN dotnet publish "EnergyAssistant.fsproj" -c Release --self-contained false -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app

RUN apk add --no-cache dotnet6-runtime

COPY run.sh /
RUN chmod a+x /run.sh

CMD [ "/run.sh" ]