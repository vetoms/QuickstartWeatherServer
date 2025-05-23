The “Hello World” of MCPs — I haven’t seen many C# examples out there, so I decided to create one.
This code implements an MCP Server Tool in C# called WeatherTools, which exposes a tool (GetForecast) to retrieve the weather forecast for a city using two public APIs from Open-Meteo.

What does GetForecast specifically do?
Input parameters:

city (required): Name of the city to query.

country (optional): Country code in ISO 3166-1 alpha-2 format (e.g., "US" or "DE").

Step 1: Geocoding
It calls the API https://geocoding-api.open-meteo.com to get the latitude and longitude of the specified city.

Step 2: Validation
If a country is specified, it filters the results to find the matching city for that country.

Step 3: Weather forecast query
Using the retrieved latitude and longitude, it calls the API https://api.open-meteo.com to get the weather forecast.

It retrieves data such as:

2-meter temperature

Rain

Visibility

Precipitation probability

Apparent temperature

Result
It returns the full JSON weather forecast as a string — so that any AI can use it.
