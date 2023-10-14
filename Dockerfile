# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
# Add-on
ARG BUILD_FROM
FROM $BUILD_FROM as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["src/EnergyAssistantConsole/EnergyAssistantConsole.csproj", "src/EnergyAssistantConsole"]
RUN dotnet restore "src/EnergyAssistantConsole/EnergyAssistantConsole.csproj"
COPY . .
WORKDIR "/src/src/EnergyAssistantConsole"
RUN dotnet build "EnergyAssistantConsole.csproj" -c Release -o /app/build

ARG BUILD_ARCH
FROM build AS publish
RUN dotnet publish "EnergyAssistantConsole.csproj" -c Release --self-contained false -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app

RUN apk add --no-cache dotnet7-runtime

COPY run.sh /
RUN chmod a+x /run.sh
CMD [ "/run.sh" ]