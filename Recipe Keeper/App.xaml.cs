using Recipe_Keeper.Classes;
using Recipe_Keeper.Pages;
using Recipe_Keeper.Database;

namespace Recipe_Keeper
{
    public partial class App : Application
    {
        private IServiceProvider ServiceProvider;
        //Service to handle database operations.
        private readonly DatabaseService databaseService;

        public App(IServiceProvider serviceProvider, DatabaseService databaseService)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
            this.databaseService = databaseService;
            CheckDefaultData();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var landingPage = ServiceProvider.GetService<LandingPage>();
            var nav_landingPage = new NavigationPage(landingPage);
            return new Window(nav_landingPage);
        }
        private async Task CheckDefaultData()
        {
            bool databaseIsEmpty = await databaseService.Database_isEmptyAsync();
            if (databaseIsEmpty)
            { await databaseService.CreateDefaultTables(); }
        }
    }
}