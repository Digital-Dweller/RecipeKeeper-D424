using System;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Microsoft.Maui.Media;


namespace Recipe_Keeper.Pages;

public partial class CreateRecipe : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private string imagePath = string.Empty;
    public List<string> categoriesList { get; set; }
    public CreateRecipe(IServiceProvider serviceProvider, UserSession userSession)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
        categoriesList = RecipeCategories.categories;
        BindingContext = this;
    }

    private async void OnClick_AddPhoto(object sender, EventArgs e)
    {
        try
        {
            var targetImage = await MediaPicker.Default.PickPhotoAsync();
            if (targetImage != null)
            {
                var imageRead = await targetImage.OpenReadAsync();
                imagePath = ImageSource.FromStream(() => imageRead).ToString();
                recipeImage.Source = imagePath;
            }
        }
        catch (Exception exception)
        { await DisplayAlert("Error", $"An error occurred: {exception.Message}", "Ok"); }
    }
    private async void OnClick_TakePhoto(object sender, EventArgs e)
    {
        try
        {
            var targetImage = await MediaPicker.Default.CapturePhotoAsync();
            if (targetImage != null)
            {
                var imageRead = await targetImage.OpenReadAsync();
                imagePath = ImageSource.FromStream(() => imageRead).ToString();
                recipeImage.Source = imagePath;
            }
        }
        catch (Exception exception)
        { await DisplayAlert("Error", $"An error occurred: {exception.Message}", "Ok"); }
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