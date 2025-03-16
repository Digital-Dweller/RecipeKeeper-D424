using Microsoft.Extensions.DependencyInjection;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class CreateProfile : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly DatabaseService databaseService;
    private readonly UserSession userSession;

    public CreateProfile(IServiceProvider serviceProvider, DatabaseService databaseService, UserSession userSession)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.databaseService = databaseService;
        this.userSession = userSession;
        //Obscure the password input characters.
        input_Pass.InnerEntry.IsPassword = true;
        input_ConfirmPass.InnerEntry.IsPassword = true;
        
    }
    private async void onClick_Cancel(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void onClick_Create(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(input_Username.InnerEntry.Text) && !string.IsNullOrEmpty(input_Pass.InnerEntry.Text) && !string.IsNullOrEmpty(input_ConfirmPass.InnerEntry.Text) && !string.IsNullOrEmpty(input_Email.InnerEntry.Text))
        {
            if (input_Pass.InnerEntry.Text == input_ConfirmPass.InnerEntry.Text) 
            {
                bool userExists = await databaseService.IsUser(input_Username.InnerEntry.Text);
                if (!userExists) 
                {
                    string passHash = PasswordHandler.HashPassword(input_Pass.InnerEntry.Text);
                    dbUser newUser = new dbUser(input_Username.InnerEntry.Text, input_Email.InnerEntry.Text, passHash);
                    await databaseService.AddUser(newUser);
                    dbUser addedUser = await databaseService.GetUserFromUsername(input_Username.InnerEntry.Text);
                    userSession.Login(addedUser);
                    //Pop all the pages from the stack and add the favorites page to the top.
                    var favorites_page = ServiceProvider.GetService<Favorites>();
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(favorites_page);
                }
                else { await DisplayAlert("Username Error", "The username selected already exists. Please use a different one.", "Confirm"); }               
            }
            else { await DisplayAlert("Password Error", "The passwords don't match. Please try again.", "Confirm"); }
        }
        else { await DisplayAlert("Incomplete Information","Please make sure to complete each input field","Confirm"); }
        var Favorites_page = ServiceProvider.GetService<Favorites>();
        await Navigation.PushAsync(Favorites_page);
    }
}