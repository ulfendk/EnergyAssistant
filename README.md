# Energy Assistant (for carnot.dk)

**NB! This add-on is only useful when connected to the Danish electricity grid!**

This add-on makes it easy to create automations in Home Assistant using the grid spot price data and predictions from [CARNOT](https://www.carnot.dk).

This add-on has been written in F# as a hobby project for fun and use - but could easily be created in a similar fashion as a Home Assistant integration.  However, that's beyond my current scope.

It runs in F# and integrates with Home Assistant throgh MQTT (including discovery).

## Installing

1. Go to Settings -> Add-ons -> Store for Add-ons
2. Click the `...` button in the top
3. 

## Getting the API key

1. Visit [carnot.dk](https://www.carnot.dk/profile/create) to create a profile.
1. Open your profile and generate an API key

## Configuration

The add-on requires som configuration.

### MQTT

The following defaults are provided.  If you are running the Mosquitto add-on, you will need to add a user for Energy Assistant in the module's configuration, which can then be used here.

```yaml
server: "core-mosquitto"
port: 1883
user: ""
pwd: ""
client_id: "Energy Assistant"
use_tls: false
```

### Carnot

The following defaults are provided.

You can use region `dk1` (West Denamrk) or `dk2` (East Denmark).  `user` is the email address you used to create your carnot.dk account and `api_key` is the key you generated above.

```yaml
region: "dk2"
user: ""
api_key: ""
```

### Tariffs and fees

Additional costs can be added to the raw price.

Due to a limitation in add-on configuration, the tariffs are split into date periods `tariffPeriods` and daily time periods `tariffs` in the following format, containing my particular fees.  The tariffs defined must refer to a defined tariffPeriod.  These should be excl. VAT.

#### tariffPeriods
```yaml
- name: 2022Q4
  startDate: "2022-10-01"
  endDate: "2022-12-31"
- name: 2023Q1
  startDate: "2023-01-01"
  endDate: "2023-03-31"
- name: 2023Sommer
  startDate: "2023-04-01"
  endDate: "2023-09-30"
- name: 2023Vinter
  startDate: "2023-10-01"
  endDate: "2024-03-31"
```

#### tariffs

```yaml
- period: 2022Q4
  startTime: "00:00:00"
  endTime: "17:00:00"
  fixedCost: 0.3323
- period: 2022Q4
  startTime: "17:00:00"
  endTime: "20:00:00"
  fixedCost: 0.7971
- period: 2022Q4
  startTime: "20:00:00"
  endTime: "23:59:59"
  fixedCost: 0.3323
- period: 2023Q1
  startTime: "00:00:00"
  endTime: "06:00:00"
  fixedCost: 0.19504
- period: 2023Q1
  startTime: "06:00:00"
  endTime: "17:00:00"
  fixedCost: 0.52136
- period: 2023Q1
  startTime: "17:00:00"
  endTime: "21:00:00"
  fixedCost: 1.49888
- period: 2023Q1
  startTime: "21:00:00"
  endTime: "23:59:59"
  fixedCost: 0.52136
- period: 2023Sommer
  startTime: "00:00:00"
  endTime: "06:00:00"
  fixedCost: 0.19504
- period: 2023Sommer
  startTime: "06:00:00"
  endTime: "17:00:00"
  fixedCost: 0.27648
- period: 2023Sommer
  startTime: "17:00:00"
  endTime: "21:00:00"
  fixedCost: 0.6676
- period: 2023Sommer
  startTime: "21:00:00"
  endTime: "23:59:59"
  fixedCost: 0.27648
- period: 2023Vinter
  startTime: "00:00:00"
  endTime: "06:00:00"
  fixedCost: 0.2278
- period: 2023Vinter
  startTime: "06:00:00"
  endTime: "17:00:00"
  fixedCost: 0.6357
- period: 2023Vinter
  startTime: "17:00:00"
  endTime: "21:00:00"
  fixedCost: 1.8576
- period: 2023Vinter
  startTime: "21:00:00"
  endTime: "23:59:59"
  fixedCost: 0.6357

```

### Spans

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

### Levels

Different price levels can be configured to easily allow for automations based on the current spot price.  A good example is setting the thermostat set points to lower / higher temperatures accordingly.

Low is anything below medium and thus is not set:

```yaml
medium: 1.5
high: 2.5
extreme: 4
```

### VAT

VAT is always added onto the final cost - but can be set to any percentage, incl. 0 to avoid VAT.

```yaml
vat: 0.25
```

## Home Assistant

When configured correctly the entities will appear automatically in HA, under the MQTT integration.

#### Basic

The following entities are always there:

| Entity | Description |
|--------|-------------|
| sensor.spotprice | The current spot price |
| sensor.spotprice_level | The current price level |
| sensor.spotprice_minimum | The lowest spot price in the period |
| sensor.spotprice_minimum_time | The timestamp when the lowest spot price occurs |
| sensor.spotprice_maximum | The highest spot price in the period |
| sensor.spotprice_maximum_time | The timestamp when the highest spot price occurs |
| sensor.spotprice_average | The average spot price |
| sensor.spotprice_median | The median spot price |

### Graph

To display a graph like this in Home Assistant:

![Graph](img/graph.png "Spot prices graph")

You can install ApexChart using HACS and add the following to Lovelace:

```yaml
type: custom:apexcharts-card
experimental:
  color_threshold: true
header:
  show: true
  title: Elpriser
now:
  show: true
  label: Nu
span:
  start: hour
graph_span: 144h
yaxis:
  - min: 0
    max: ~4
series:
  - entity: sensor.spotprice
    type: area
    show:
      extremas: true
    stroke_width: 0
    data_generator: |
      return entity.attributes.prices.map((start, index) => {
        return [new Date(start["hour"]).getTime(), entity.attributes.prices[index]["price"]];
      });
    color_threshold:
      - value: 0
        color: green
        opacity: 1
      - value: 1.5
        color: yellow
      - value: 2.5
        color: pink
      - value: 4
        color: red
```

### Entities

You can also show the basic entities like this:

![Entities](img/entities.png "Spot price entities")

Using the following:

```yaml
type: entities
entities:
  - entity: sensor.spotprice
  - entity: sensor.spotprice_level
  - entity: sensor.spotprice_average
  - entity: sensor.spotprice_median
  - type: divider
  - entity: sensor.spotprice_minimum
  - entity: sensor.spotprice_minimum_time
  - type: divider
  - entity: sensor.spotprice_maximum
  - entity: sensor.spotprice_maximum_time
title: Elpriser
show_header_toggle: false
```

**Note: The entity names can be changed by clicking the entities, like in the above where the names have been translated.**

## Add-on configuration example

```yaml
carnot:
  region: dk2
  user: ####
  api_key: ####
mqtt:
  server: core-mosquitto
  port: 1883
  user: ####
  pwd: ####
  client_id: Energy Assistant
  use_tls: false
tariffs:
  - period: 2022Q4
    startTime: "00:00:00"
    endTime: "17:00:00"
    fixedCost: 0.3323
  - period: 2022Q4
    startTime: "17:00:00"
    endTime: "20:00:00"
    fixedCost: 0.7971
  - period: 2022Q4
    startTime: "20:00:00"
    endTime: "23:59:59"
    fixedCost: 0.3323
  - period: 2023Q1
    startTime: "00:00:00"
    endTime: "06:00:00"
    fixedCost: 0.19504
  - period: 2023Q1
    startTime: "06:00:00"
    endTime: "17:00:00"
    fixedCost: 0.52136
  - period: 2023Q1
    startTime: "17:00:00"
    endTime: "21:00:00"
    fixedCost: 1.49888
  - period: 2023Q1
    startTime: "21:00:00"
    endTime: "23:59:59"
    fixedCost: 0.52136
  - period: 2023Sommer
    startTime: "00:00:00"
    endTime: "06:00:00"
    fixedCost: 0.19504
  - period: 2023Sommer
    startTime: "06:00:00"
    endTime: "17:00:00"
    fixedCost: 0.27648
  - period: 2023Sommer
    startTime: "17:00:00"
    endTime: "21:00:00"
    fixedCost: 0.6676
  - period: 2023Sommer
    startTime: "21:00:00"
    endTime: "23:59:59"
    fixedCost: 0.27648
  - period: 2023Vinter
    startTime: "00:00:00"
    endTime: "06:00:00"
    fixedCost: 0.2278
  - period: 2023Vinter
    startTime: "06:00:00"
    endTime: "17:00:00"
    fixedCost: 0.6357
  - period: 2023Vinter
    startTime: "17:00:00"
    endTime: "21:00:00"
    fixedCost: 1.8576
  - period: 2023Vinter
    startTime: "21:00:00"
    endTime: "23:59:59"
    fixedCost: 0.6357
vat: 0.25
levels:
  medium: 1.5
  high: 2.5
  extreme: 4
spans:
  - title: Daglig bilopladning
    hours: 2
    max_hours_future: 18
    hours_of_day: 0|1|2|3|4|5|17|18|19|20|21|22|23
  - title: Daglig vask
    hours: 2
    max_hours_future: 14
    hours_of_day: ""
  - title: Storvask (3 dage)
    hours: 4
    max_hours_future: 72
    hours_of_day: ""
  - title: Storvask (6 dage)
    hours: 4
    max_hours_future: 144
    hours_of_day: ""
tariffPeriods:
  - name: 2022Q4
    startDate: "2022-10-01"
    endDate: "2022-12-31"
  - name: 2023Q1
    startDate: "2023-01-01"
    endDate: "2023-03-31"
  - name: 2023Sommer
    startDate: "2023-04-01"
    endDate: "2023-09-30"
  - name: 2023Vinter
    startDate: "2023-10-01"
    endDate: "2024-03-31"
```