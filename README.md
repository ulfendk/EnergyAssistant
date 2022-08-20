# EnergyAssistant

**NB! This add-on is only useful when connected to the Danish electricity grid!**

This add-on makes it easy to create automations in Home Assistant using the spot price data from carnot.dk.  Had I been more apt in Python and Home Assistant development, this could have been created as an integration (please be inspired).

For now, it was fun for me to code it in F# and package it as an add-on, using MQTT as the integration mechanism into HA.

## Getting the API key
carnot.dk

## Configuration

### MQTT

### Carnot

### Lowest spans (Optional)

```yaml
- title: Next laundry
  hours: 2
  max_hours_future: 48
- title: Next EV charge
  hours: 3
  max_hours_future: 18
  hours_of_day: "0|1|2|3|4|5|17|18|19|20|21|22|23"
```

### Highest spans

## Usage in Home Assistant

### Entities

Basic

Spans

### Graph

### Automation examples

#### Configuring to charge the EV when you get home from work