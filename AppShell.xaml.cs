namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("MapPage", typeof(Pages.Weathermap));
            Routing.RegisterRoute("Mainpage", typeof(Pages.Mainpage));

        }
    }
}
