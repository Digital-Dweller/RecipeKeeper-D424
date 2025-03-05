using Recipe_Keeper.Classes;
using Recipe_Keeper.Pages;
using Recipe_Keeper.Database;

namespace Recipe_Keeper
{
    public partial class App : Application
    {
        private IServiceProvider ServiceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var landingPage = ServiceProvider.GetService<LandingPage>();
            var nav_landingPage = new NavigationPage(landingPage);
            return new Window(nav_landingPage);
        }
    }
}