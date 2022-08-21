FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /source
# copy csproj and restore as distinct layers
COPY src/ .
RUN dotnet restore --use-current-runtime src/EnergyAssistant/

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained true --no-restore src/EnergyAssistant/

ARG BUILD_FROM
FROM $BUILD_FROM

# Copy data for add-on
COPY run.sh /
RUN chmod a+x /run.sh

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["EnergyAssistant"]

# CMD [ "/run.sh" ]