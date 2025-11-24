using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Service
{
    public class _10daysService
    {
        private readonly HttpClient _client = new();
        private const string API_KEY = "f6a8d7f356d14380bb1234225250911";

        public async Task<List<TenDayInfo>> GetTenDayAsync(string city)
        {
            string url = $"https://api.weatherapi.com/v1/forecast.json?key={API_KEY}&q={city}&days=10";
            string json = await _client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var forecastDays = doc.RootElement.GetProperty("forecast").GetProperty("forecastday");

            List<TenDayInfo> list = new();

            foreach (var day in forecastDays.EnumerateArray())
            {
                var info = new TenDayInfo
                {
                    Day = DateTime.Parse(day.GetProperty("date").GetString()!).ToString("ddd, MMM d"),
                    MaxTemp = day.GetProperty("day").GetProperty("maxtemp_c").GetDouble(),
                    MinTemp = day.GetProperty("day").GetProperty("mintemp_c").GetDouble(),
                    Condition = day.GetProperty("day").GetProperty("condition").GetProperty("text").GetString() ?? "",
                    IconUrl = "https:" + day.GetProperty("day").GetProperty("condition").GetProperty("icon").GetString()
                };
                list.Add(info);
            }

            return list;
        }
    }
}
