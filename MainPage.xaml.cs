using MauiApp1.Models;
using MauiApp1.Service; 
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly WeatherService _weatherService;

        public MainPage()
        {
            InitializeComponent();
            _weatherService = new WeatherService();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string city = CityEntry.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(city))
            {
                ResultLabel.Text = "⚠️ Please enter a city name.";
                return;
            }

            ResultLabel.Text = "⏳ Fetching weather...";
            WeatherIcon.IsVisible = false;

            var info = await _weatherService.GetWeatherAsync(city);

            if (info != null)
            {
                ResultLabel.Text = $"{info.City}\n" +
                                   $"🌡 Temp: {info.TempC}°C (Feels like {info.FeelsLikeC}°C)\n" +
                                   $"💧 Humidity: {info.Humidity}%\n" +
                                   $"🌬 Wind: {info.WindKph} km/h\n" +
                                   $"☁️ Condition: {info.Condition}";

                if (!string.IsNullOrEmpty(info.IconUrl))
                {
                    WeatherIcon.Source = ImageSource.FromUri(new Uri(info.IconUrl));
                    WeatherIcon.IsVisible = true;
                }
            }
            else
            {
                ResultLabel.Text = "❌ Could not fetch weather data. Try again.";
            }
        }
    }
}
