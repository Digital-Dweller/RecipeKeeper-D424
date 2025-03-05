using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class Login : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly DatabaseService databaseService;
    private readonly UserSession userSession;
    public Login(IServiceProvider serviceProvider, DatabaseService databaseService, UserSession userSession)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.databaseService = databaseService;
        this.userSession = userSession;
        input_Pass.InnerEntry.IsPassword = true;
    }

    private async void onClick_NewUser(object sender, EventArgs e)
    {
        var createProfile_page = ServiceProvider.GetService<CreateProfile>();
        await Application.Current.MainPage.Navigation.PopToRootAsync();
        await Application.Current.MainPage.Navigation.PushAsync(createProfile_page);
    }


    private async void onClick_Login(object sender, EventArgs e)
    {
        string passHash = PasswordHandler.HashPassword(input_Pass.InnerEntry.Text);
        bool userIsUser = await databaseService.IsUser(input_Username.InnerEntry.Text);
        if (userIsUser) { 
            bool validLogin = await databaseService.UserLogin(input_Username.InnerEntry.Text, passHash);
            if (validLogin) 
            {
                dbUser user = await databaseService.GetUserFromUsername(input_Username.InnerEntry.Text);
                userSession.Login(user);
                var favorites_page = ServiceProvider.GetService<Favorites>();
                await Application.Current.MainPage.Navigation.PushAsync(favorites_page);
            }
        }
        else { await DisplayAlert("Login Error", "The credentials provided were invalid.", "Confirm"); }
    }

}