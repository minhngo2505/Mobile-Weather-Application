using MauiApp1.Resources.Styles;


namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            LoadUserPreferences();

        }

        public void SetTheme(string theme)
        {
            var root = Application.Current!.Resources;

            // ⚠ Remove old theme ONLY (not Colors or Styles)
            var oldTheme = root.MergedDictionaries
                .FirstOrDefault(d => d is LightTheme || d is DarkTheme);

            if (oldTheme != null)
                root.MergedDictionaries.Remove(oldTheme);

            // normalize
            var t = theme.Trim().ToLowerInvariant();

            if (t == "light")
            {
                root.MergedDictionaries.Add(new LightTheme());
                Application.Current.UserAppTheme = AppTheme.Light;
            }
            else
            {
                root.MergedDictionaries.Add(new DarkTheme());
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
        }
        public void LoadUserPreferences()
        {
            // THEME
            bool isDark = Preferences.Get("isDarkTheme", false);  // default Light
            SetTheme(isDark ? "dark" : "light");

            // FONT SIZE
            int fontSize = Preferences.Get("font_size", 18); // medium default
            SetFontSize(fontSize);

            // CONTRAST MODE
            bool highContrast = Preferences.Get("contrast", false);
            SetContrast(highContrast);
        }
        public void SetFontSize(int fontSize)
        {
            Application.Current.Resources["AppFontSize"] = fontSize;
        }
        public void SetContrast(bool highConstrast)
        {
            Application.Current.Resources["HighContrast"] = highConstrast;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}