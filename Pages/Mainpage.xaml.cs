using System;
using MauiApp1.Models;
using MauiApp1.Service;
using Microsoft.Maui.Controls;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Platform;
using System.Threading.Tasks;

namespace MauiApp1.Pages;
[QueryProperty(nameof(CityQuery), "city")] // Query property to receive city name from navigation
public partial class Mainpage : ContentPage // Main page displaying weather information
{
    private readonly WeatherService _weatherService = new(); // Service to fetch weather data

    private WeatherInfo? _lastWeather; // Store the last loaded weather info
    private bool isCelsius => Preferences.Get("units", "C") == "C"; // Check user preference for temperature unit

    public string CityQuery // Property to receive city name from navigation
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value))// Validate the city name
            {
                Dispatcher.Dispatch(async () => // Use Dispatcher to ensure UI thread access
                {
                    await LoadWeatherAsync(value); // Load weather for the specified city
                    await LoadTenDaysAsync(value);
                });
            }
        }
    }

    public Mainpage()
    {
        InitializeComponent(); // Initialize UI components
    }
    
    protected override async void OnAppearing() // Override OnAppearing to load default weather data
    {
        base.OnAppearing(); // Call base method

        if (_lastWeather == null)// Load default city weather if none is loaded
        {
            await LoadWeatherAsync("Perth");// Default city
            await LoadTenDaysAsync("Perth");

        }
    }


    private async Task LoadWeatherAsync(string city) // Method to load weather data for a specified city
    {
        try// Try-catch for error handling
        {
            TempLabel.Text = "Loading...";// Indicate loading state
            FeelsLikeLabel.Text = HumidityLabel.Text = WindLabel.Text = "-";// Reset other labels

            var weather = await _weatherService.GetWeatherAsync(city);// Fetch weather data


            if (weather == null)// Handle null response
            {
                await DisplayAlert("Error", "Unable to load weather data.", "OK");
                return;
            }
            _lastWeather = weather; // Store the last loaded weather info
            CityLabel.Text = weather.City; // Update UI with fetched data
            if (isCelsius )
            {
                double t = weather.TempC;
                double f = weather.FeelsLikeC;
                TempLabel.Text = $"{t}°C | {weather.Condition}";
                FeelsLikeLabel.Text = $"{f}°C";
            }
            else
            {
                double tF = (weather.TempC * 9 / 5) + 32;
                double fF = (weather.FeelsLikeC * 9 / 5) + 32;
                TempLabel.Text = $"{tF:0.#}°F | {weather.Condition}";
                FeelsLikeLabel.Text = $"{fF:0.#}°F";
            }
            HumidityLabel.Text = $"{weather.Humidity}%";
            WindLabel.Text = $"{weather.WindKph} km/h"; 
            WeatherImage.Source = weather.IconUrl;
            
            await LoadHourlyAsync(city, weather.LocalTime); // Load hourly forecast

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        
    }
    private async Task LoadHourlyAsync(string city, string LocalTime) // Method to load hourly forecast data
    {
        try
        {
            var service = new HourlyService(); // Service to fetch hourly data
            var hourlyList = await service.GetHourlyInfoAsync(city); // Fetch hourly data

            if (hourlyList == null || hourlyList.Count == 0) // Handle null or empty response
                return;

            var locTime = DateTime.Parse(LocalTime); // Parse local time
            int currentHour = locTime.Hour;            // Get current hour
            var rotated = hourlyList // Rotate the list to start from current hour
                .Skip(currentHour) // Skip hours before current hour
                .Concat(hourlyList.Take(currentHour)) // Append skipped hours to the end
                .Take(24) // Take only the next 24 hours
                .ToList(); // Convert to list

            HourlyForecastView.ItemsSource = rotated; // Update UI with hourly data
        }
        catch (Exception ex)
        {
            await DisplayAlert("error", ex.Message, "ok");
        }


    }



    // 🔍 SEARCH BAR EVENTS
    private async void OnSearchPressed(object sender, EventArgs e)
    {
        string city = CitySearchBar.Text?.Trim() ?? string.Empty; // Get city name from search bar
        if (string.IsNullOrWhiteSpace(city)) // Validate input
        {
            await DisplayAlert("Input Required", "Please enter a city name.", "OK");
            return;
        }
        await LoadWeatherAsync(city); // Load weather for the specified city
        await LoadTenDaysAsync(city); // Load 10-day forecast for the specified city



    }
    private async void OnSearchUnfocused(object sender, FocusEventArgs e)
{
    string city = CitySearchBar.Text?.Trim() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(city))
    {
        await DisplayAlert("Input Required", "Please enter a city name.", "OK");
        return;
    }
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
        String lon = _lastWeather.Lon.ToString(CultureInfo.InvariantCulture); // Convert to string with invariant culture
        await Shell.Current.GoToAsync($"MapPage?lat={lat}&lon={lon}");
        
    }
    private async void OnstarTapped(object sender, TappedEventArgs e)
    {
        if (_lastWeather == null)
            return;
        string city = _lastWeather.City;

        if (FavoriteService.Exits(city))
        {
            await DisplayAlert("info", $"{city} is already in your favorites.","ok");
            return;
        }
        
         FavoriteService.Add(city);
         await DisplayAlert ("Success", $"{_lastWeather.City} added to favorites.", "OK");
    }
}
