namespace Recipe_Keeper.Pages;

public partial class LandingPage : ContentPage
{
    private IServiceProvider ServiceProvider;
    public LandingPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
    }
	private async void onClick_CreateProfile(object sender, EventArgs e) 
	{
		var CreateProfile_page = ServiceProvider.GetService<CreateProfile>();
		await Navigation.PushAsync(CreateProfile_page);
	}
}