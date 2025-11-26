using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Service
{
    public class HourlyService
    {
        private readonly HttpClient _client = new HttpClient();
        private const string API_KEY = "f6a8d7f356d14380bb1234225250911";

        public async Task<List<HourlyInfo>> GetHourlyInfoAsync(string city)
        {
            string url = $"https://api.weatherapi.com/v1/forecast.json?key={API_KEY}&q={city}&days=2";
            string json = await _client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var today = doc.RootElement
                              .GetProperty("forecast")
                              .GetProperty("forecastday")[0]
                              .GetProperty("hour");
            var tomorrow = doc.RootElement
                              .GetProperty("forecast")
                              .GetProperty("forecastday")[1]
                              .GetProperty("hour");
            var allhours = today.EnumerateArray().Concat(tomorrow.EnumerateArray()).ToList();


            List<HourlyInfo> list = new();

            foreach (var h in allhours)
            {
                list.Add(new HourlyInfo
                {
                    Time = DateTime.Parse(h.GetProperty("time").GetString()!).ToString("HH:mm"),
                    TempC = h.GetProperty("temp_c").GetDouble(),
                    IconUrl = "https:" + h.GetProperty("condition").GetProperty("icon").GetString() ?? ""
                });
            }
            return list;

        }

    }
}

