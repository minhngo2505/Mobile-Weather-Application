using System.Threading.Tasks;

namespace MauiApp1.Pages;

public partial class Settingpage : ContentPage
{
    public Settingpage()
    {
        InitializeComponent();

        ThemeSwitch.IsToggled = Preferences.Get("isDarkTheme", true);
        NotificationSwitch.IsToggled = Preferences.Get("notify", false);
        ContrastSwitch.IsToggled = Preferences.Get("contrast", false);

        UnitsButton.Text = Preferences.Get("units", "C") == "C"
                           ? "Celsius (°C)"
                           : "Fahrenheit (°F)";
    }


    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        bool isDark = e.Value;
        Preferences.Set("isDarkTheme", isDark);
        (Application.Current as App)?.SetTheme(isDark ? "dark" : "light");
    }


    private void OnUnitsClicked(object sender, EventArgs e)
    {
        string unit = Preferences.Get("units", "C");

        if (unit == "C")
        {
            Preferences.Set("units", "F");
            UnitsButton.Text = "Fahrenheit (°F)";
        }
        else
        {
            Preferences.Set("units", "C");
            UnitsButton.Text = "Celsius (°C)";
        }
    }


    private void OnFontClicked(object sender, EventArgs e)
    {
        if (sender == FontSmall) Preferences.Set("font_size", 14);
        if (sender == FontMedium) Preferences.Set("font_size", 20);
        if (sender == FontLarge) Preferences.Set("font_size", 30);

        int fontSize = Preferences.Get("font_size", 20);
        (Application.Current as App)?.SetFontSize(fontSize);
    }


    private void OnNotificationsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("notify", e.Value);

    }


    private void OnContrastToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("contrast", e.Value);
        (Application.Current as App)?.SetContrast(e.Value);
    }
}
