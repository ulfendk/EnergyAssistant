#!/usr/bin/with-contenv bashio

CONFIG="setting.config"

bashio::log.info "Creating Energy Assistant configuration..."

# Create main config
USER_NAME=$(bashio::config 'user')
API_KEY=$(bashio::config 'api_key')
LOWEST_SPANS=$(bashio::config 'highest_spans')
HIGHEST_SPANS=$(bashio::config 'lowest_spans')

# DEFAULT_LEASE=$(bashio::config 'default_lease')
# DNS=$(bashio::config 'dns|join(", ")')
# DOMAIN=$(bashio::config 'domain')
# MAX_LEASE=$(bashio::config 'max_lease')

{
    echo "{"
    echo "  \"@username\": \"${USER_NAME}\","
    echo "  \"apikey\": \"${API_KEY},"
    echo "  \"apikey\": \"${LOWEST_SPANS},"
    echo "  \"apikey\": \"${HIGHEST_SPANS}"
    echo "}"
} > "${CONFIG}"

echo "Wrote the following config in `pwd`"
cat ${CONFIG}