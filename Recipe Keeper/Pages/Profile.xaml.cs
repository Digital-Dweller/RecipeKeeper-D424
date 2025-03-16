namespace Recipe_Keeper.Pages;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;


public partial class Profile : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private readonly DatabaseService databaseService;

    public Profile(IServiceProvider serviceProvider, UserSession userSession, DatabaseService databaseService)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
        this.databaseService = databaseService;
        usernamePassword.InnerEntry.IsPassword = true;
        emailPassword.InnerEntry.IsPassword = true;
        currPassword.InnerEntry.IsPassword = true;
        confirmCurrPassword.InnerEntry.IsPassword = true;
        newPassword.InnerEntry.IsPassword = true;
        confirmNewPassword.InnerEntry.IsPassword = true;
        BindingContext = this;
    }
    private async void onClick_UpdatePassword(object sender, EventArgs e)
    {
        bool response = await DisplayAlert("Change Confirmation", "Are you sure you want to update the password?", "Yes", "No");
        if (response)
        {
            if (currPassword.InputValue != string.Empty || confirmCurrPassword.InputValue != string.Empty || newPassword.InputValue != string.Empty || confirmNewPassword.InputValue != string.Empty)
            {
                if (currPassword.InputValue == confirmCurrPassword.InputValue)
                {
                    bool validLogin = await databaseService.UserLogin(userSession.username, currPassword.InputValue);
                    if (validLogin)
                    {
                        if (newPassword.InputValue == confirmNewPassword.InputValue)
                        {
                            string newPassHash = PasswordHandler.HashPassword(newPassword.InputValue);
                            await databaseService.UpdatePassword(userSession.id, newPassHash);
                            await DisplayAlert("Update Success", "The password was updated successfully.", "Confirm");
                            ClearAllFields();
                        }
                        else { await DisplayAlert("Password Error", "The new passwords don't match. Please try again.", "Confirm"); }
                    }
                    else { await DisplayAlert("Login Error", "The credentials provided were invalid.", "Confirm"); }
                }
                else { await DisplayAlert("Password Error", "The current passwords don't match. Please try again.", "Confirm"); }
            }
            else { await DisplayAlert("Input Error", "Password is missing. Make sure to complete all password fields to update the password.", "Confirm"); }
        }
    }
    private async void onClick_UpdateUsername(object sender, EventArgs e)
    {
        bool response = await DisplayAlert("Change Confirmation", "Are you sure you want to update the username?", "Yes", "No");
        if (response)
        {
            if (username.InputValue != string.Empty || usernamePassword.InputValue != string.Empty)
            {
                bool validLogin = await databaseService.UserLogin(userSession.username, usernamePassword.InputValue);
                if (validLogin)
                {
                    await databaseService.UpdateUsername(userSession.id, username.InputValue);
                    await DisplayAlert("Update Success", "The username was updated successfully.", "Confirm");
                    ClearAllFields();
                }
                else { await DisplayAlert("Login Error", "The credentials provided were invalid.", "Confirm"); }
            }
            else { await DisplayAlert("Input Error", "Incomplete information. Make sure to complete username and password fields to update the username.", "Confirm"); }
        }
    }

    private async void onClick_UpdateEmail(object sender, EventArgs e)
    {
        bool response = await DisplayAlert("Change Confirmation", "Are you sure you want to update the email?", "Yes", "No");
        if (response)
        {
            if (email.InputValue != string.Empty || emailPassword.InputValue != string.Empty)
            {
                bool validLogin = await databaseService.UserLogin(userSession.username, emailPassword.InputValue);
                if (validLogin)
                {
                    await databaseService.UpdateEmail(userSession.id, email.InputValue);
                    await DisplayAlert("Update Success", "The email was updated successfully.", "Confirm");
                    ClearAllFields();
                }
                else { await DisplayAlert("Login Error", "The credentials provided were invalid.", "Confirm"); }
            }
            else { await DisplayAlert("Input Error", "Incomplete information. Make sure to complete email and password fields to update the email.", "Confirm"); }
        }
    }

    private void ClearAllFields()
    {
        username.InputValue = string.Empty;
        usernamePassword.InputValue = string.Empty;
        email.InputValue = string.Empty;
        emailPassword.InputValue = string.Empty;
        currPassword.InputValue = string.Empty;
        confirmCurrPassword.InputValue = string.Empty;
        newPassword.InputValue = string.Empty;
        confirmNewPassword.InputValue = string.Empty;
    }

    private async void onClick_Logout(object sender, EventArgs e)
    {
        await userSession.Logout();
        await Navigation.PopToRootAsync();
    }
    private async void onClick_Favorites(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void onClick_Recipes(object sender, EventArgs e)
    {
        var recipes_page = ServiceProvider.GetService<Recipes>();
        Navigation.InsertPageBefore(recipes_page, Navigation.NavigationStack.Last());
        await Navigation.PopAsync();
    }
    private async void onClick_CreateRecipe(object sender, EventArgs e)
    {
        var createRecipe_page = ServiceProvider.GetService<CreateRecipe>();
        Navigation.InsertPageBefore(createRecipe_page, Navigation.NavigationStack.Last());
        await Navigation.PopAsync();
    }
    private async void onClick_Search(object sender, EventArgs e)
    {
        var search_page = ServiceProvider.GetService<Search>();
        Navigation.InsertPageBefore(search_page, Navigation.NavigationStack.Last());
        await Navigation.PopAsync();
    }
    private async void onClick_Profile(object sender, EventArgs e)
    {
        var profile_page = ServiceProvider.GetService<Profile>();
        Navigation.InsertPageBefore(profile_page, Navigation.NavigationStack.Last());
        await Navigation.PopAsync();
    }
}