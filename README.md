***Weather Mobile Application***

      This project is a simple cross-platform weather application built with .NET MAUI.
      The app retrieves real-time weather data from WeatherAPI and displays the current weather, hourly forecast, and multi-day forecast for any selected city. It also includes basic features such as favourites, unit conversion, and app settings.

Features

  - Search for a city and view its current weather information

  - Display temperature, condition, humidity, wind speed, and weather icon

  - 24-hour hourly forecast

  - 10-day forecast

  - Add or remove favourite locations

  - Temperature unit toggle (Celsius / Fahrenheit)

  - Basic settings page (theme, font size, unit preferences)

  - Simple map redirection using latitude and longitude

  - Persistent storage using Preferences

***Technology Stack***

 - .NET MAUI

 - C#

 - WeatherAPI (REST API)

 - Preferences for local storage

 - Code-behind page structure using MVC pattern
  ***

***Models + Service classes for separation of concerns***
Project Structure
```project-root/
|-----Models/
|        | 
|        |-----WeatherInfo.cs
|        |-----HourlyInfo.cs
|        |-----TenDayInfo.cs
|         
|
|-----Service/
|        |-----WeatherService.cs
|	 |-----HourlyService.cs
|        |-----TenDayService.cs
|        |-----FavoriteService.cs
|
|
|-----Pages/
|	|-----Mainpage.xaml
|       |-----Favouritepage.xaml
|	|-----Settingpage.xaml
|	|-----Weathermap.xaml
|-----Resources/
|         |-----Images, Styles, App Icons
|
|
```
***How It Works***

- Mainpage calls WeatherService, HourlyService, and TenDayService to load all weather data.

- The search bar triggers a request to reload weather for the selected city.

- The star button saves the city name to the favourites list using FavoriteService.

- Favourite cities are displayed as items in Favouritepage.

- Clicking a favourite sends the city parameter back to Mainpage.

- Temperature units are converted manually in code-behind using a simple formula.

- Settings (unit, theme, font size) are stored with Preferences.

***API***

    The app uses free endpoints from:

    https://www.weatherapi.com/

    Each service sends HTTP GET requests and parses the JSON response manually.

***Setup***

    Install .NET 8 or .NET 9 with MAUI workload

    Clone the repository

    Add your WeatherAPI key in service files

    Build and run the project on Android, iOS, or Windows

***Author***

    Minh Ngo
    North Metropolitan TAFE
    Perth, Western Australia