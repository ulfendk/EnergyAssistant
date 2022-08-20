#!/usr/bin/with-contenv bashio

CONFIG="setting.config"

bashio::log.info "Creating Energy Assistant configuration..."

# Create main config
USER_NAME=$(bashio::config 'user')
API_KEY=$(bashio::config 'api_key')

# DEFAULT_LEASE=$(bashio::config 'default_lease')
# DNS=$(bashio::config 'dns|join(", ")')
# DOMAIN=$(bashio::config 'domain')
# MAX_LEASE=$(bashio::config 'max_lease')

{
    echo "{"
    echo "  \"@username\": \"${USER_NAME}\","
    echo "  \"apikey\": \"${API_KEY},"
    echo "}"
} > "${CONFIG}"

./EnergyAssistant