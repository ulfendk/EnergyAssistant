# Energy Assistant (for carnot.dk)

**NB! This add-on is only useful when connected to the Danish electricity grid!**

This add-on makes it easy to create automations in Home Assistant using the spot price data from carnot.dk.  Had I been more apt in Python and Home Assistant development, this could have been created as an integration (please be inspired).

For now, it was fun for me to code it in F# and package it as an add-on, using MQTT as the integration mechanism into HA.

## Getting the API key
carnot.dk

## Configuration

### MQTT

### Carnot

### Lowest spans (Optional)

The following example will provide start times for:

1. The best time to do the laundry in the next 48 hours, based on a load taking 2 hours and that it can be run any time of day.
1. The best time to charge the EV after work in the next 18 hours, based on a charge taking 3 hours in the span of 17:00 to 06:00.

Examples of how these can be used in Home Assistant to trigger automations will be shown below.

```yaml
- title: Next laundry
  hours: 2
  max_hours_future: 48
- title: Next EV charge
  hours: 3
  max_hours_future: 18
  hours_of_day: "0|1|2|3|4|5|17|18|19|20|21|22|23"
```

## Home Assistant

### Entities

Basic

Spans

### Graph

### Automation examples

#### Configuring to charge the EV when you get home from work