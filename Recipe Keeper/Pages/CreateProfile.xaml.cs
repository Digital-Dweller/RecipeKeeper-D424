using Microsoft.Extensions.DependencyInjection;

namespace Recipe_Keeper.Pages;

public partial class CreateProfile : ContentPage
{
	public CreateProfile()
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
}