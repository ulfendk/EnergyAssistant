# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
# Add-on
ARG BUILD_FROM
FROM $BUILD_FROM as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["src/EnergyAssistantGui/EnergyAssistantGui.csproj", "src/EnergyAssistantGui"]
RUN dotnet restore "src/EnergyAssistantGui"
COPY . .
WORKDIR "/src/src/EnergyAssistantGui"
RUN dotnet build -c Release -o /app/build

ARG BUILD_ARCH
FROM build AS publish
# RUN dotnet publish -c Release --self-contained false -o /app/publish
RUN dotnet publish --no-restore -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=publish /app/publish /app

RUN apk add --no-cache aspnetcore7-runtime

COPY run.sh /
RUN chmod a+x /run.sh


#Add nginx and create the run folder for nginx.
RUN \
  apk --no-cache add \
    nginx \
  \
  && mkdir -p /run/nginx

#Copy our conf into the nginx http.d folder.
COPY ingress.conf /etc/nginx/http.d/

#Launch nginx with debug options.
#CMD [ "nginx","-g","daemon off;error_log /dev/stdout debug;" ]

CMD [ "/run.sh" ]