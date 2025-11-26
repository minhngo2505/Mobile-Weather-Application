namespace MauiApp1.Pages;

[QueryProperty(nameof(City),"city")]
public partial class Weathermap : ContentPage
{
	public string City { get; set; } = "";
    protected override void OnAppearing()
	{
		RadarWebView.Source = $"https://www.windy.com/?{City}";
    }

}