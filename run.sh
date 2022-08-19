#!/usr/bin/with-contenv bashio

USER_NAME=$(bashio::config 'user')
API_KEY=$(bashio::config 'api_key')

echo "Hello world!"