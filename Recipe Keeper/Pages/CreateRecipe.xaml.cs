using System;
using Recipe_Keeper.Classes;


namespace Recipe_Keeper.Pages;

public partial class CreateRecipe : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    public CreateRecipe(IServiceProvider serviceProvider, UserSession userSession)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
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