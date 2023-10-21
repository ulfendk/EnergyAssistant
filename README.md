# Energy Assistant (Denmark only)

This add-on provides a simple and complete energy management solution for Danish consumers.

It offers integration to the following external services:
- [CARNOT](https://www.carnot.dk) for spotprice predictions based on weather forecasts, etc.
- [ENERGI DATA SERVICE](https://www.energidataservice.dk) the public Danish API for fetching spotprices
- [NORD POOL](https://www.nordpoolgroup.com/) provided as an optional backup, in case EnergiDataService is down
- [ElOverblik](https://www.eloverblik.dk) for historical energy usage

The assistant offers the following integrations into Home Assistant:

- Spot price sensors
  - Raw prices ex. VAT
  - Actual prices, incl. fees, tariffs and VAT
  - Reduced actual prices, incl. fees, tariffs and VAT for electrically heated homes, when above the 4,000 kWh limit
- Automatic switching to the reduced price, when consumption has passed the limit
- Spot price level sensors
  - Set levels, such as Low, Medium and High
- Energy usage sensors (from ElOverblik)
- Tariff and fees configuration
  - Define fees based on date ranges and daily times
  - Define regular and reduced prices, applicable for homes with electric heating
- Climate entities integration
  - Set temperatures in you home, based on levels
- Scenario sensors
  - Define scenarios for e.g. laundry or EV charging
  - Optimal start time, based on:
    - Duration of task
    - Criticality, defined as maximum offset of the start time from now

Additionally, all data is presented in graphs in the user interface.

## Installing

1. Go to Settings -> Add-ons -> Store for Add-ons
2. Click the `...` button in the top
3. 
