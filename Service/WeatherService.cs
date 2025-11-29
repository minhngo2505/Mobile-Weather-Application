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
    public class WeatherService
    {
        private readonly HttpClient _client = new HttpClient();
        private const string API_KEY = "f6a8d7f356d14380bb1234225250911";

        public async Task<WeatherInfo?> GetWeatherAsync(string city)
        {
            try
            {
                string url = $"https://api.weatherapi.com/v1/current.json?key={API_KEY}&q={city}";
                var json = await _client.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var location = root.GetProperty("location");
                var current = root.GetProperty("current");
                var cond = current.GetProperty("condition");

                return new WeatherInfo
                {
                    City = location.GetProperty("name").GetString() ?? "",
                    TempC = current.GetProperty("temp_c").GetDouble(),
                    FeelsLikeC = current.GetProperty("feelslike_c").GetDouble(),
                    Humidity = current.GetProperty("humidity").GetInt32(),
                    WindKph = current.GetProperty("wind_kph").GetDouble(),
                    Condition = cond.GetProperty("text").GetString() ?? "",
                    IconUrl = "https:" + cond.GetProperty("icon").GetString(),
                    LocalTime = location.GetProperty("localtime").GetString()??"",
                    Lat = location.GetProperty("lat").GetDouble(), 
                    Lon = location.GetProperty("lon").GetDouble()
                };
            }
            catch
            {
                return null;
            }
        }
    }
}

