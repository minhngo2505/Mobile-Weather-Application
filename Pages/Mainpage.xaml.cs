using System;
using MauiApp1.Models;
using MauiApp1.Service;
using Microsoft.Maui.Controls;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MauiApp1.Pages;

public partial class Mainpage : ContentPage
{
    private readonly WeatherService _weatherService = new();

    private WeatherInfo? _lastWeather;

    public Mainpage()
    {
        InitializeComponent();
        _ = LoadWeatherAsync("Perth");
        _ = LoadTenDaysAsync("perth");
       
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
            _lastWeather = weather;
            CityLabel.Text = weather.City;
            TempLabel.Text = $"{weather.TempC}°C | {weather.Condition}";
            FeelsLikeLabel.Text = $"{weather.FeelsLikeC}°C";
            HumidityLabel.Text = $"{weather.Humidity}%";
            WindLabel.Text = $"{weather.WindKph} km/h"; 
            WeatherImage.Source = weather.IconUrl;
            
            await LoadHourlyAsync(city, weather.LocalTime);

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        
    }
    private async Task LoadHourlyAsync(string city, string LocalTime)
    {
        try
        {
            var service = new HourlyService();
            var hourlyList = await service.GetHourlyInfoAsync(city);

            if (hourlyList == null || hourlyList.Count == 0)
                return;

            var locTime = DateTime.Parse(LocalTime);
            int currentHour = locTime.Hour;            
            var rotated = hourlyList
                .Skip(currentHour)
                .Concat(hourlyList.Take(currentHour))
                .Take(24)
                .ToList();

            HourlyForecastView.ItemsSource = rotated;
        }
        catch (Exception ex)
        {
            await DisplayAlert("error", ex.Message, "ok");
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
        await LoadTenDaysAsync(city);
        


    }
    private async Task LoadTenDaysAsync(string city)
    {
        try
        {
            var service = new _10daysService();
            var list = await service.GetTenDayAsync(city);

            TenDayForecast.ItemsSource = list;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


    private async void OnChangeLocationClicked(object sender, EventArgs e)
    {
        if (sender is MenuFlyoutItem item)
        {
            string city = item.Text.Split(',')[0];
            await LoadWeatherAsync(city);
            await LoadTenDaysAsync(city);
        }
    }


    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadWeatherAsync(CityLabel.Text ?? "Perth");
    }

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        if (_lastWeather == null)
            return;
        String lat = _lastWeather.Lat.ToString(CultureInfo.InvariantCulture);
        String lon = _lastWeather.Lon.ToString(CultureInfo.InvariantCulture);
        await Shell.Current.GoToAsync($"MapPage?lat={lat}&lon={lon}");
        
    }
}
