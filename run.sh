#!/usr/bin/with-contenv bashio
echo "String nginx..."
nginx -g "daemon off;error_log /dev/stdout debug;" &

echo "Starting Energy Assistant..."
dotnet exec EnergyAssistant.dll 
