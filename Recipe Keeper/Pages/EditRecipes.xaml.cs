using System;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Controls;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Pages;

public partial class EditRecipes : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private DatabaseService databaseService;
    private List<Tuple<Grid, CheckBox, string>> RecipeCards;
    public EditRecipes(IServiceProvider serviceProvider, UserSession userSession, DatabaseService databaseService)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
        this.databaseService = databaseService;
        RecipeCards = new List<Tuple<Grid, CheckBox, string>>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (userSession.UserRecipes.Count > 0)
        {
            foreach (Recipe recipe in userSession.UserRecipes)
            {
                Grid containerGrid = new Grid 
                {
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 40 }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };
                

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

                CheckBox deleteCheckBox = new CheckBox 
                {
                    IsChecked = false
                };

                ImageButton editRecipeButton = new ImageButton 
                {
                    Source = "edit.svg",
                    Margin = new Thickness(5,0,0,0),
                    VerticalOptions = LayoutOptions.Center
                };
                editRecipeButton.Clicked += (s, e) => onClick_EditRecipe(s, e, recipe);
                containerGrid.Children.Add(newRecipeCard);
                containerGrid.SetRowSpan(newRecipeCard, 2);
                containerGrid.Children.Add(deleteCheckBox);
                containerGrid.SetColumn(deleteCheckBox, 1);
                containerGrid.Children.Add(editRecipeButton);
                containerGrid.SetColumn(editRecipeButton, 1);
                containerGrid.SetRow(editRecipeButton, 1);

                ContentSection.Children.Add(containerGrid);

                //Add container and checkbox references to the RecipeCards list.
                RecipeCards.Add(new Tuple<Grid, CheckBox, string>(containerGrid, deleteCheckBox, recipe.Title));
            }
        }
    }

    private async void onClick_DeleteSelected(object sender, EventArgs e)
    {
        var pendingRemoval = RecipeCards.Where(t => t.Item2.IsChecked).ToList();
        foreach (var recipeCardPair in pendingRemoval)
        {
            ContentSection.Children.Remove(recipeCardPair.Item1);
            RecipeCards.Remove(recipeCardPair);
            int recipeId = await databaseService.GetRecipeID(recipeCardPair.Item3, userSession.id);
            await databaseService.DeleteRecipe(recipeId);
            await userSession.UpdateUserRecipes();
        }
    }


    private async void onClick_EditRecipe(object sender, EventArgs e, Recipe targetRecipe)
    {
        var editRecipe_page = ServiceProvider.GetService<EditRecipe>();
        editRecipe_page.targetRecipe = targetRecipe;

        Navigation.InsertPageBefore(editRecipe_page, Navigation.NavigationStack.Last());
        await Navigation.PopAsync();
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