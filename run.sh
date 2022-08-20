#!/usr/bin/with-contenv bashio

# CONFIG="setting.config"

# bashio::log.info "Creating Energy Assistant configuration..."

# # Create main config
# USER_NAME=$(bashio::config 'user')
# API_KEY=$(bashio::config 'api_key')
# LOWEST_SPANS=$(bashio::config 'lowest_spans|join(", ")')
# HIGHEST_SPANS=$(bashio::config 'highest_spans')

# # DEFAULT_LEASE=$(bashio::config 'default_lease')
# # DNS=$(bashio::config 'dns|join(", ")')
# # DOMAIN=$(bashio::config 'domain')
# # MAX_LEASE=$(bashio::config 'max_lease')

# {
#     echo "{"
#     echo "  \"@username\": \"${USER_NAME}\","
#     echo "  \"apikey\": \"${API_KEY},"
#     echo "  \"lowestspans\": [ ${LOWEST_SPANS} ],"
#     echo "  \"highestspans\": [ ${HIGHEST_SPANS} ]"
#     echo "}"
# } > "${CONFIG}"
# PWD = `pwd`
# bashio::log.info  "Wrote the following config in ${PWD}"
bashio::log.info `cat /data/options.json`