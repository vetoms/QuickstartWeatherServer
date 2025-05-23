🌤️ WeatherTools — The “Hello World” of MCPs in C#
I noticed there aren’t many examples of MCP (Model Context Protocol) tools written in C#, so I decided to create one.

This project implements a simple MCP Server Tool called WeatherTools, which provides a method — GetForecast — to retrieve weather forecasts for a given city using two public APIs from Open-Meteo.

🛠️ What Does GetForecast Do?
GetForecast is a static method that performs the following steps:

📥 Input Parameters
city (required): Name of the city to query.

country (optional): Country code in ISO 3166-1 alpha-2 format (e.g., "US", "DE").

🧭 Step 1: Geocoding
It sends a request to:
https://geocoding-api.open-meteo.com
to retrieve the latitude and longitude of the specified city.

🔍 Step 2: Validation
If a country code is provided, it filters the results to match the city within the specified country.

🌦️ Step 3: Weather Forecast
Using the coordinates obtained, it calls:
https://api.open-meteo.com
to get the current weather forecast.

Returned data includes:

🌡️ 2-meter temperature

🌧️ Rain

🌫️ Visibility

☔ Precipitation probability

🥵 Apparent temperature

📤 Output
Returns the full JSON weather forecast as a string — ready to be consumed by any AI agent or client.
