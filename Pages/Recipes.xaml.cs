using System;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Controls;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class Recipes : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private DatabaseService databaseService;

    public Recipes(IServiceProvider serviceProvider, UserSession userSession, DatabaseService databaseService)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
        this.databaseService = databaseService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (userSession.UserRecipes.Count > 0)
        {
            foreach (Recipe recipe in userSession.UserRecipes)
            {
                var newRecipeCard = ServiceProvider.GetService<RecipeCard>();
                if (recipe.ImagePath == string.Empty)
                { newRecipeCard.ImageSourceBinding = "food.svg"; }
                else
                { newRecipeCard.ImageSourceBinding = recipe.ImagePath; }
                newRecipeCard.TitleBinding = recipe.Title;
                if (recipe.Favorited)
                { newRecipeCard.FavoriteImageBinding = "favorites_filled.svg"; }
                else
                { newRecipeCard.FavoriteImageBinding = "favorites_emptyblack.svg"; }
                newRecipeCard.FavoriteCommandBinding = new Command(() => { onClick_Favorited(recipe, newRecipeCard); });
                newRecipeCard.DescriptionBinding = recipe.Description;
                ContentSection.Children.Add(newRecipeCard);
            }
        }    
    }

    private async void onClick_Favorited(Recipe targetRecipe, RecipeCard recipeCard)
    {
        dbRecipe dbTargetRecipe = await databaseService.GetRecipe(targetRecipe.Id);
        if (!targetRecipe.Favorited)
        { 
            recipeCard.FavoriteImageBinding = "favorites_filled.svg";
            dbTargetRecipe.Favorited = true;
        }
        else
        { 
            recipeCard.FavoriteImageBinding = "favorites_emptyblack.svg";
            dbTargetRecipe.Favorited = false;
        }
        await databaseService.UpdateRecipe(dbTargetRecipe);
        await userSession.UpdateUserRecipes();
    }

    private async void onClick_EditRecipe(object sender, EventArgs e)
    {
        var editRecipe_page = ServiceProvider.GetService<EditRecipes>();
        Navigation.InsertPageBefore(editRecipe_page, Navigation.NavigationStack.Last());
        await Navigation.PopAsync();
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