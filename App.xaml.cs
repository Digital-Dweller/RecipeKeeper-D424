using Recipe_Keeper.Classes;
using Recipe_Keeper.Pages;
using Recipe_Keeper.Database;

namespace Recipe_Keeper
{
    public partial class App : Application
    {
        private IServiceProvider ServiceProvider;
        private readonly DatabaseService databaseService;


        public App(IServiceProvider serviceProvider, DatabaseService databaseService)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
            this.databaseService = databaseService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loadingPage = ServiceProvider.GetService<Loading>();
            var nav_loadingPage = new NavigationPage(loadingPage);
            return new Window(nav_loadingPage);
        }
    }
}