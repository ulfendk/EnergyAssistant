FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EnergyAssistant/EnergyAssistant.fsproj", "src/EnergyAssistant/"]
RUN dotnet restore "src/EnergyAssistant/EnergyAssistant.fsproj"
COPY . .
WORKDIR "/src/src/EnergyAssistant"
RUN dotnet build "EnergyAssistant.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnergyAssistant.fsproj" -c Release --self-contained true -o /app/publish

# Add-on
ARG BUILD_FROM
FROM $BUILD_FROM

## Copy data for add-on
#COPY run.sh /
#RUN chmod a+x /run.sh
#
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["EnergyAssistant"]

# CMD [ "/run.sh" ]