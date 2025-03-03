using Microsoft.Extensions.DependencyInjection;

namespace Recipe_Keeper.Pages;

public partial class CreateProfile : ContentPage
{
    private IServiceProvider ServiceProvider;
    public CreateProfile(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        //Obscure the password input characters.
        input_Pass.InnerEntry.IsPassword = true;
        input_ConfirmPass.InnerEntry.IsPassword = true;
    }
    private async void onClick_Cancel(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void onClick_Favorites(object sender, EventArgs e)
    {
        var Favorites_page = ServiceProvider.GetService<Favorites>();
        await Navigation.PushAsync(Favorites_page);
    }
}