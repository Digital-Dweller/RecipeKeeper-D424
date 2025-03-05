using Recipe_Keeper.Classes;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class LandingPage : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly DatabaseService databaseService;

    public LandingPage(IServiceProvider serviceProvider, DatabaseService databaseService)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.databaseService = databaseService;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //Check if the database is empty. If so add the default tables.
        bool databaseIsEmpty = await databaseService.Database_isEmptyAsync();
        if (databaseIsEmpty)
        { await databaseService.CreateDefaultTables(); }
        //Check if the dbUser table is empty, if not nav to login.
        bool dbUserIsEmpty = await databaseService.dbUser_isEmptyAsync();
        if (!dbUserIsEmpty) 
        {
            //Pop all the pages from the stack and add the login page to the top.
            var login_page = ServiceProvider.GetService<Login>();
            await Application.Current.MainPage.Navigation.PopToRootAsync();
            await Application.Current.MainPage.Navigation.PushAsync(login_page);
        }
    }


    private async void onClick_CreateProfile(object sender, EventArgs e) 
	{
		var CreateProfile_page = ServiceProvider.GetService<CreateProfile>();
		await Navigation.PushAsync(CreateProfile_page);
	}
}