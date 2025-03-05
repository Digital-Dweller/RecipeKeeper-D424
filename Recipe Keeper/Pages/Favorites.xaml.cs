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

    private async void onClick_Recipes(object sender, EventArgs e)
    {
        var recipes_page = ServiceProvider.GetService<Recipes>();
        await Navigation.PushAsync(recipes_page);
    }
    private async void onClick_CreateRecipe(object sender, EventArgs e)
    {
        var createRecipe_page = ServiceProvider.GetService<CreateRecipe>();
        await Navigation.PushAsync(createRecipe_page);
    }
    private async void onClick_Search(object sender, EventArgs e)
    {
        var search_page = ServiceProvider.GetService<Search>();
        await Navigation.PushAsync(search_page);
    }
    private async void onClick_Profile(object sender, EventArgs e)
    {
        var profile_page = ServiceProvider.GetService<Profile>();
        await Navigation.PushAsync(profile_page);
    }



}