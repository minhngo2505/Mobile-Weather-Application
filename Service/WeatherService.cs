using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Service
{
    internal class WeatherService
    {
        private readonly HttpClient _client;
        private const string ApiKey = "f6a8d7f356d14380bb1234225250911";

        public WeatherService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
        }

        public async Task<weatherInfo?> GetWeatherAsync(string city )
        {
            if (string.IsNullOrWhiteSpace(city))
                return null;

            try
            {
                string url = $"https://api.weatherapi.com/v1/current.json?key={ApiKey}&q={city.Trim()}";
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var location = root.GetProperty("location");
                var current = root.GetProperty("current");
                var condition = current.GetProperty("condition");

                return new weatherInfo
                {
                    City = location.GetProperty("name").GetString() ?? city,
                    TempC = current.GetProperty("temp_c").GetDouble(),
                    FeelsLikeC = current.GetProperty("feelslike_c").GetDouble(),
                    Humidity = current.GetProperty("humidity").GetInt32(),
                    WindKph = current.GetProperty("wind_kph").GetDouble(),
                    Condition = condition.GetProperty("text").GetString() ?? "",
                    IconUrl = "https:" + (condition.TryGetProperty("icon", out var iconProp) ? iconProp.GetString() : "")
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
