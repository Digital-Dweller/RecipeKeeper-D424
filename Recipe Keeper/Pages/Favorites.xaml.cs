using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class Favorites : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    public Favorites(IServiceProvider serviceProvider, UserSession userSession)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
    }
    protected override async void OnAppearing()
	{
        base.OnAppearing();
    }

    private async void onClick_CreateRecipe(object sender, EventArgs e)
    {
        var createRecipe_page = ServiceProvider.GetService<CreateRecipe>();
        await Application.Current.MainPage.Navigation.PushAsync(createRecipe_page);
    }

}