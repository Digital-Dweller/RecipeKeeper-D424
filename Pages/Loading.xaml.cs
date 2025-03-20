using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class Loading : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly DatabaseService databaseService;
    private readonly UserSession userSession;
    private readonly LogHandler logger;

    public Loading(IServiceProvider serviceProvider, DatabaseService databaseService, UserSession userSession, LogHandler logger)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.databaseService = databaseService;
        this.userSession = userSession;
        this.logger = logger;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //Check if the database is empty. If so add the default tables.
        bool databaseIsEmpty = await databaseService.Database_isEmptyAsync();
        if (databaseIsEmpty)
        { await databaseService.CreateDefaultTables(); }
        //Check if the dbUser table is empty, navigate to the landing page if it is or navigate to login if not.
        bool dbUserIsEmpty = await databaseService.dbUser_isEmptyAsync();
        //Check if there is a user with a saved login enabled. 
        var userRemembered = await databaseService.CheckRememberMe();

        //Run unit tests.
        UnitTests runTests = new UnitTests(databaseService);
        await runTests.RunAllTests();

        //Check the logfile.
        //await logger.ReadLogs();


        if (userRemembered != null)
        {
            await userSession.Login(userRemembered);
            var favorites_page = ServiceProvider.GetService<Favorites>();
            await Navigation.PushAsync(favorites_page);
        }
        else if (!dbUserIsEmpty)
        {
            var login_page = ServiceProvider.GetService<Login>();
            await Navigation.PushAsync(login_page);
        }
        else 
        {
            var landing_page = ServiceProvider.GetService<LandingPage>();
            await Navigation.PushAsync(landing_page);
        }
    }
}