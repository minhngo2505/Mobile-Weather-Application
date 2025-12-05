using MauiApp1.Service;
using MauiApp1.Models;
using Microsoft.Maui.Controls.Shapes;


namespace MauiApp1.Pages;

public partial class Favouritepage : ContentPage
{
	public Favouritepage()
	{
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		LoadFavorites();
    }
	private void LoadFavorites()
	{
		FavContainer.Children.Clear();
		var list = FavoriteService.GetAll();
        foreach (var city in list)
		{
			var box = new Border
			{
				BackgroundColor = (Color)Application.Current.Resources["PrimaryColor"],
				Stroke = (Color)Application.Current.Resources["SecondaryTextColor"],
				StrokeShape = new RoundRectangle { 	CornerRadius = 8 },	
                Margin = new Thickness(0, 0, 0, 10),
				Padding = 12,
			};
			var lable = new Label
			{
				Text = city,
				TextColor = Colors.White,
				FontSize = 20,
			};
			var tap = new TapGestureRecognizer();
			tap.Tapped += async (s, e) =>
			{
                var encoded = Uri.EscapeDataString(city).Trim();
                await Shell.Current.GoToAsync($"//Mainpage?city={encoded}");
            };
			lable.GestureRecognizers.Add(tap);
			box.Content = lable;
			FavContainer.Children.Add(box);
        }
    }
	private async void OnDeleteClicked(object sender, EventArgs e)
	{
		var list = FavoriteService.GetAll();
		if (list.Count == 0)
		{
			await DisplayAlert("Empty", "No favorites to delete.", "OK");
			return;
        }
		String Selection = await DisplayActionSheet("Select a city to delete", "Cancel", null, list.ToArray());
		if (Selection == null || Selection == "Cancel")
			return;
		FavoriteService.Remove(Selection);
		await DisplayAlert("Deleted", $"{Selection} has been removed from favorites.", "OK");
        LoadFavorites();
    }

}