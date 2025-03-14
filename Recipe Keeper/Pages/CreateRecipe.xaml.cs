using System;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Classes.Utilities;
using Microsoft.Maui.Media;
using Recipe_Keeper.Controls;
using System.Diagnostics;
using Recipe_Keeper.Database;


namespace Recipe_Keeper.Pages;

public partial class CreateRecipe : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private string imagePath = string.Empty;
    private DatabaseService databaseService;
    public List<string> unitsList { get; set; }
    public List<string> categoriesList { get; set; }
    public List<dbIngredient> RecipeIngredients { get; set; }
    public List<dbDirection> RecipeDirections { get; set; }
    public CreateRecipe(IServiceProvider serviceProvider, UserSession userSession, DatabaseService databaseService)
	{
		InitializeComponent();
        ServiceProvider = serviceProvider;
        this.userSession = userSession;
        imagePath = string.Empty;
        unitsList = Units.units;
        categoriesList = RecipeCategories.categories;
        RecipeIngredients = new List<dbIngredient>();
        RecipeDirections = new List<dbDirection>();
        this.databaseService = databaseService;
        BindingContext = this;
    }

    private async void OnClick_AddPhoto(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo == null)
                return;

            // Load the image into the UI
            using var stream = await photo.OpenReadAsync();
            imagePath = photo.FullPath;
            recipeImage.Source = imagePath;
            photoText1.Text = string.Empty;
            photoText2.Text = string.Empty;
            photoBorder.Padding = 0;
        }
        catch (Exception exception)
        { await DisplayAlert("Error", $"An error occurred: {exception.Message}", "Ok"); }
    }

    private void OnClick_AddIngredient(object sender, EventArgs e)
    {
        Grid UIContainer = new Grid
        {
            BackgroundColor = Colors.WhiteSmoke,
            Margin = new Thickness(0, 10, 0, 0),
            HeightRequest = 80
        };

    }

    private async void OnClick_TakePhoto(object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    imagePath = localFilePath;
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                    recipeImage.Source=localFilePath;
                    photoText1.Text = string.Empty;
                    photoText2.Text = string.Empty;
                    photoBorder.Padding = 0;

                }
            }
        }
        catch (Exception exception)
        { await DisplayAlert("Error", $"An error occurred: {exception.Message}", "Ok"); }
    }
    private void onClick_AddIngredient(string qty, string unit, string ingredient)
    {
        var addIngredientControl = ServiceProvider.GetService<AddIngredient>();
        addIngredientControl.qtyText = qty;
        addIngredientControl.unitText = unit;
        addIngredientControl.ingredientText = ingredient;
        addIngredientControl.deleteBinding = new Command(() =>
        {
            ingredientsSection.Children.Remove(addIngredientControl);
            var ingredientToRemove = RecipeIngredients.FirstOrDefault(Ingredient => Ingredient.Title == ingredient);
            RecipeIngredients.Remove(ingredientToRemove);
        });
        ingredientsSection.Children.Add(addIngredientControl);
    }



    private void onClick_NewIngredient(object sender, EventArgs e)
    {
        var newIngredientControl = ServiceProvider.GetService<NewIngredient>();
        newIngredientControl.unitsListBinding = unitsList;
        newIngredientControl.saveBinding = new Command(() =>
        {
        if (newIngredientControl.ingredientQuantity == string.Empty | newIngredientControl.ingredientUnit == string.Empty | newIngredientControl.ingredientTitle == string.Empty)
        { DisplayAlert("Incomplete Information", "Please make sure to complete each input field.", "Confirm"); }
        else
        {
            var ingredientChecked = RecipeIngredients.FirstOrDefault(Ingredient => Ingredient.Title == newIngredientControl.ingredientTitle);
            if (ingredientChecked != null)
            { DisplayAlert("Duplicate Ingredient", $"An ingredient with the title:{newIngredientControl.ingredientTitle} already exists.", "Confirm"); }
            else
            {
                onClick_AddIngredient(newIngredientControl.ingredientQuantity, newIngredientControl.ingredientUnit, newIngredientControl.ingredientTitle);
                ingredientsSection.Children.Remove(newIngredientControl);
                dbIngredient newIngredient = new dbIngredient { Title = newIngredientControl.ingredientTitle, Quantity = newIngredientControl.ingredientQuantity, Unit = newIngredientControl.ingredientUnit};
                RecipeIngredients.Add(newIngredient);
                }
            }
        });
        ingredientsSection.Children.Add(newIngredientControl);
    }

    private void onClick_NewDirection(object sender, EventArgs e)
    {
        var newDirectionControl = ServiceProvider.GetService<NewDirection>();
        newDirectionControl.saveBinding = new Command(() =>
        {
            if (newDirectionControl.DirectionTitle == string.Empty | newDirectionControl.DirectionDescription == string.Empty)
            { DisplayAlert("Incomplete Information", "Please make sure to complete each input field.", "Confirm"); }
            else
            {
                var DirectionChecked = RecipeDirections.FirstOrDefault(Direction => Direction.Title == newDirectionControl.DirectionDescription);
                if (DirectionChecked != null)
                { DisplayAlert("Duplicate Direction", $"A direction with the title:{newDirectionControl.DirectionTitle} already exists.", "Confirm"); }
                else
                {
                    onClick_AddDirection(newDirectionControl.DirectionTitle, newDirectionControl.DirectionDescription);
                    directionsSection.Children.Remove(newDirectionControl);
                    dbDirection newDirection = new dbDirection { Title = newDirectionControl.DirectionTitle, Description = newDirectionControl.DirectionDescription  };
                    RecipeDirections.Add(newDirection);
                }
            }
        });
        directionsSection.Children.Add(newDirectionControl);
    }

    private void onClick_AddDirection(string title, string description)
    {
        var addDirectionControl = ServiceProvider.GetService<AddDirection>();
        addDirectionControl.directionTitle = title;
        addDirectionControl.directionDescription = description;
        addDirectionControl.deleteBinding = new Command(() =>
        {
            directionsSection.Children.Remove(addDirectionControl);
            var DirectionToRemove = RecipeDirections.FirstOrDefault(Direction => Direction.Title == title);
            RecipeDirections.Remove(DirectionToRemove);
        });
        directionsSection.Children.Add(addDirectionControl);
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

    private async void onClick_SaveRecipe(object sender, EventArgs e)
    {
        Console.WriteLine("Image path: " + imagePath);
        if (input_Title.InputValue == string.Empty || input_Author.InputValue == string.Empty || input_Category.SelectedItem.ToString() == string.Empty || input_Description.Text == string.Empty)
        { await DisplayAlert("Incomplete Information", "Please make sure to complete each input field.", "Confirm"); }
        else 
        {
            if (RecipeIngredients.Count == 0)
            { await DisplayAlert("Incomplete Information", "No ingredients have been added to the recipe. Add ingredients before continuing.", "Confirm"); }
            else 
            {
                if (RecipeDirections.Count == 0)
                { await DisplayAlert("Incomplete Information", "No directions have been added to the recipe. Add directions before continuing.", "Confirm"); }
                else
                {
                    bool recipeExists = await databaseService.IsRecipe(input_Title.InputValue);
                    if (recipeExists)
                    { await DisplayAlert("Invalid Recipe Name", $"A recipe with the title \"{input_Title.InputValue}\" already exists.", "Confirm"); }
                    else
                    {
                        dbRecipe newRecipe = new dbRecipe { Title = input_Title.InputValue, Author = input_Author.InputValue, Description = input_Description.Text, Category = input_Category.SelectedItem.ToString(), Favorited = false, ImagePath = imagePath, UserId = userSession.id };
                        await databaseService.AddRecipe(newRecipe);
                        int recipeId = await databaseService.GetRecipeID(input_Title.InputValue);
                        foreach (dbIngredient ingredient in RecipeIngredients)
                        {
                            ingredient.Id = recipeId;
                            await databaseService.AddIngredient(ingredient);
                        }
                        int posCount = 0;
                        foreach (dbDirection Direction in RecipeDirections)
                        {
                            Direction.Id = recipeId;
                            Direction.OrderPosition = posCount++;
                            await databaseService.AddDirection(Direction);
                        }
                        Navigation.PopAsync();
                    }

                }
            }
        }
    }
}