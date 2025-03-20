using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class Login : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly DatabaseService databaseService;
    private readonly UserSession userSession;
    private readonly LogHandler logger;

    public Login(IServiceProvider serviceProvider, DatabaseService databaseService, UserSession userSession, LogHandler logger)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.databaseService = databaseService;
        this.userSession = userSession;
        this.logger = logger;
        input_Pass.InnerEntry.IsPassword = true;
    }

    private async void onClick_NewUser(object sender, EventArgs e)
    {
        var createProfile_page = ServiceProvider.GetService<CreateProfile>();
        await Navigation.PushAsync(createProfile_page);

    }


    private async void onClick_Login(object sender, EventArgs e)
    {
        string passHash = PasswordHandler.HashPassword(input_Pass.InnerEntry.Text);
        bool userIsUser = await databaseService.IsUser(input_Username.InnerEntry.Text);
        if (userIsUser) {
            bool validLogin = await databaseService.UserLogin(input_Username.InnerEntry.Text, input_Pass.InnerEntry.Text);
            if (validLogin) 
            {
                var favorites_page = ServiceProvider.GetService<Favorites>();
                await Navigation.PushAsync(favorites_page);
                dbUser user = await databaseService.GetUserFromUsername(input_Username.InnerEntry.Text);
                if (rememberMeCheckBox.IsChecked)
                { await databaseService.SetRememberMe(user); }
                await userSession.Login(user);
                await logger.LogMessage($"User: {user.Username} logged in successfully");
            }
            else 
            { 
                await DisplayAlert("Login Error", "The credentials provided were invalid.", "Confirm");
                await logger.LogMessage($"Failed login attempt for username: {input_Username.InnerEntry.Text}");
            }
        }
        else 
        { 
            await DisplayAlert("Login Error", "The credentials provided were invalid.", "Confirm");
            await logger.LogMessage($"Failed login attempt for username: {input_Username.InnerEntry.Text}");
        }
    }

}