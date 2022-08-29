#!/usr/bin/with-contenv bashio
echo "Starting Energy Assistant..."

dotnet exec EnergyAssistant.dll /data/options.json

