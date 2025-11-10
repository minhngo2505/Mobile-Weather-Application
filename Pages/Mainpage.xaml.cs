using MauiApp1.Models;
using MauiApp1.Service;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages;

public partial class Mainpage : ContentPage
{
    private readonly WeatherService _weatherService = new();

    public Mainpage()
    {
        InitializeComponent();
        _ = LoadWeatherAsync("Perth");
    }

    private async Task LoadWeatherAsync(string city)
    {
        try
        {
            TempLabel.Text = "Loading...";
            FeelsLikeLabel.Text = HumidityLabel.Text = WindLabel.Text = "-";

            var weather = await _weatherService.GetWeatherAsync(city);

            if (weather == null)
            {
                await DisplayAlert("Error", "Unable to load weather data.", "OK");
                return;
            }

            CityLabel.Text = weather.City;
            TempLabel.Text = $"{weather.TempC}°C | {weather.Condition}";
            FeelsLikeLabel.Text = $"{weather.FeelsLikeC}°C";
            HumidityLabel.Text = $"{weather.Humidity}%";
            WindLabel.Text = $"{weather.WindKph} km/h";
            WeatherImage.Source = weather.IconUrl;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // 🔍 SEARCH BAR EVENTS
    private async void OnSearchPressed(object sender, EventArgs e)
    {
        string city = CitySearchBar.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(city))
        {
            await DisplayAlert("Input Required", "Please enter a city name.", "OK");
            return;
        }
        await LoadWeatherAsync(city);
    }


    private async void OnChangeLocationClicked(object sender, EventArgs e)
    {
        if (sender is MenuFlyoutItem item)
        {
            string city = item.Text.Split(',')[0];
            await LoadWeatherAsync(city);
        }
    }


    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadWeatherAsync(CityLabel.Text ?? "Perth");
    }
}
