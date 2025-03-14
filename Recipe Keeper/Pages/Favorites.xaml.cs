using Recipe_Keeper.Classes;
using Recipe_Keeper.Controls;
using Recipe_Keeper.Database;
using System.Windows.Input;
using Recipe_Keeper.Classes.Utilities;

namespace Recipe_Keeper.Pages;

public partial class Favorites : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private readonly DatabaseService databaseService;
    public Favorites(IServiceProvider serviceProvider, UserSession userSession, DatabaseService databaseService)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
        this.databaseService = databaseService;
        BindingContext = this;
    }
    protected override void OnAppearing()
	{
        base.OnAppearing();
        ContentSection.Clear();
        if (userSession.UserRecipes.Count > 0)
        {
            foreach (Recipe recipe in userSession.UserRecipes)
            {
                if (recipe.Favorited)
                {
                    var newRecipeCard = ServiceProvider.GetService<RecipeCard>();
                    if (recipe.ImagePath == string.Empty)
                    { newRecipeCard.ImageSourceBinding = "food.svg"; }
                    else
                    { newRecipeCard.ImageSourceBinding = recipe.ImagePath; }
                    newRecipeCard.TitleBinding = recipe.Title;
                    newRecipeCard.FavoriteImageBinding = "favorites_filled.svg";
                    newRecipeCard.FavoriteCommandBinding = new Command(() => { onClick_Favorited(recipe, newRecipeCard); });
                    newRecipeCard.DescriptionBinding = recipe.Description;
                    ContentSection.Children.Add(newRecipeCard);
                }
            }
        }
    }

    private async void onClick_Favorited(Recipe targetRecipe, RecipeCard recipeCard)
    {
        dbRecipe dbTargetRecipe = await databaseService.GetRecipe(targetRecipe.Id);
        dbTargetRecipe.Favorited = false;
        ContentSection.Children.Remove(recipeCard);
        await databaseService.UpdateRecipe(dbTargetRecipe);
        await userSession.UpdateUserRecipes();
    }


    private async void onClick_Logout(object sender, EventArgs e)
    {
        await userSession.Logout();
        await Navigation.PopToRootAsync();
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