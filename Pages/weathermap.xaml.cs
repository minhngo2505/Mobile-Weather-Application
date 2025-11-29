
using System.Text;
using Microsoft.Maui.Controls;
using System.Globalization;
using MauiApp1.Service;


namespace MauiApp1.Pages;


[QueryProperty(nameof(Lat), "lat")]
[QueryProperty(nameof(Lon), "lon")]
public partial class Weathermap : ContentPage 
{
    private double _lat;
    private double _lon;

    public double Lat
    {
        get => _lat;
        set
        {
            _lat = value;
            LoadMap();
        }
    }
    public double Lon
    {
        get => _lon;
        set
        {
            _lon = value;
            LoadMap();
        }
    }
    public Weathermap()
    {
        InitializeComponent();
    }

    private void LoadMap()
    {
        if (RadarWebView == null)
            return;
        if (_lat == 0 && _lon == 0)
            return;

        String latStr = Lat.ToString(CultureInfo.InvariantCulture);
        string lonStr = Lon.ToString(CultureInfo.InvariantCulture);
        string url = $"https://www.windy.com/?{latStr},{lonStr},10";
        RadarWebView.Source = url;
    }

}
