using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace QuickstartWeatherServer
{
    [McpServerToolType]
    public sealed class WeatherTools
    {

        private static readonly HttpClient _geoClient = new() { BaseAddress = new Uri("https://geocoding-api.open-meteo.com") };
        private static readonly HttpClient _weatherClient = new() { BaseAddress = new Uri("https://api.open-meteo.com") };


        [McpServerTool, Description("Get weather forecast for a City.")]
        public static async Task<string> GetForecast(
            [Description("city")] string city,
            [Description("country")] string country = "")
        {
            
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City must not be empty", nameof(city));


            var response = await _geoClient.GetAsync($"/v1/search?name={city}&count=10&language=en&format=json");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar la API");
            }
            
            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            var root = document.RootElement;

            if (!root.TryGetProperty("results", out JsonElement results) || results.GetArrayLength() == 0)
            {
                throw new Exception("No city found");
            }

            var firstResult = results[0];

            if (!string.IsNullOrEmpty(country))
            {
                firstResult = results.EnumerateArray()
                   .FirstOrDefault(e =>
                       e.TryGetProperty("country_code", out JsonElement code) &&
                       code.GetString()?.ToUpperInvariant() == country.ToUpperInvariant());

                if (firstResult.ValueKind == JsonValueKind.Undefined)
                    throw new Exception($"No matching city found in country: {country}");
            }

            var latitude = firstResult.GetProperty("latitude").GetDouble();
            var longitude = firstResult.GetProperty("longitude").GetDouble();

            var pointUrl = string.Create(CultureInfo.InvariantCulture, $"/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,rain,visibility,precipitation_probability,apparent_temperature");
            var forecastResponse = await _weatherClient.GetAsync(pointUrl);

            if (!forecastResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar la API");
            }

            using var forecastStream = await forecastResponse.Content.ReadAsStreamAsync();
            using var forecastReader = new StreamReader(forecastStream);
            return await forecastReader.ReadToEndAsync();

        }
    }
}
