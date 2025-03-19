using System;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Keeper.Classes.Utilities;
using Recipe_Keeper.Database;
using Recipe_Keeper.Controls;
using Recipe_Keeper.Classes;

namespace Recipe_Keeper.Pages;

public partial class EditRecipe : ContentPage
{
    private IServiceProvider ServiceProvider;
    private readonly UserSession userSession;
    private string imagePath = string.Empty;
    private DatabaseService databaseService;
    public List<string> unitsList { get; set; }
    public List<string> categoriesList { get; set; }
    public List<dbIngredient> RecipeIngredients { get; set; }
    public List<dbDirection> RecipeDirections { get; set; }
    public Recipe targetRecipe { get; set; }
    public EditRecipe(IServiceProvider serviceProvider, UserSession userSession, DatabaseService databaseService)
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (targetRecipe.ImagePath != null)
        {
            imagePath = targetRecipe.ImagePath;
            if (imagePath != string.Empty)
            {
                recipeImage.Source = imagePath;
                photoText1.Text = string.Empty;
                photoText2.Text = string.Empty;
                photoBorder.Padding = 0;
                photoBorder.WidthRequest = 150;

            }
        }

        input_Title.InputValue = targetRecipe.Title;
        input_Author.InputValue = targetRecipe.Author;
        input_Category.SelectedItem = targetRecipe.Category;
        input_Description.Text = targetRecipe.Description;
        RecipeIngredients.Clear();
        RecipeIngredients.AddRange(targetRecipe.Ingredients);
        Console.WriteLine($"Ingredient count: {RecipeIngredients.Count}");
        if (RecipeIngredients.Count > 0)
        {
            foreach (dbIngredient ingredient in RecipeIngredients)
            {
                onClick_AddIngredient(ingredient.Quantity, ingredient.Unit, ingredient.Title);
            }
        }
        RecipeDirections.Clear();
        RecipeDirections.AddRange(targetRecipe.Directions);
        Console.WriteLine($"Direction count: {RecipeDirections.Count}");
        if (RecipeDirections.Count > 0)
        {
            foreach (dbDirection direction in RecipeDirections)
            {
                onClick_AddDirection(direction.Title, direction.Description);
            }
        }
    }

    private async void OnClick_AddPhoto(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo == null)
            { return; }

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
                    recipeImage.Source = localFilePath;
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
                    dbIngredient newIngredient = new dbIngredient { Title = newIngredientControl.ingredientTitle, Quantity = newIngredientControl.ingredientQuantity, Unit = newIngredientControl.ingredientUnit };
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
                    dbDirection newDirection = new dbDirection { Title = newDirectionControl.DirectionTitle, Description = newDirectionControl.DirectionDescription };
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
    private async void onClick_Cancel(object sender, EventArgs e)
    {
        var editRecipe_page = ServiceProvider.GetService<EditRecipes>();
        Navigation.InsertPageBefore(editRecipe_page, Navigation.NavigationStack.Last());
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
                    bool recipeExists = await databaseService.IsRecipeTitle(input_Title.InputValue, targetRecipe.Id, userSession.id);
                    if (recipeExists)
                    { await DisplayAlert("Invalid Recipe Name", $"A recipe with the title \"{input_Title.InputValue}\" already exists.", "Confirm"); }
                    else
                    {
                        dbRecipe target_dbRecipe = await databaseService.GetRecipe(targetRecipe.Id);
                        target_dbRecipe.Title = input_Title.InputValue;
                        target_dbRecipe.Author = input_Author.InputValue;
                        target_dbRecipe.Description = input_Description.Text;
                        target_dbRecipe.Category = input_Category.SelectedItem.ToString();
                        target_dbRecipe.ImagePath = imagePath;
                        await databaseService.UpdateRecipe(target_dbRecipe);

                        //Delete ingredients if removed during editing.
                        List<dbIngredient> currentIngredients = await databaseService.GetIngredients(target_dbRecipe.Id);
                        foreach (dbIngredient ingredient in currentIngredients)
                        {
                            bool matched = false;
                            foreach (dbIngredient stagedIngredient in RecipeIngredients)
                            {
                                if (ingredient.Id == stagedIngredient.Id)
                                { matched = true; }
                            }
                            if (!matched)
                            { await databaseService.DeleteIngredient(ingredient); }
                        }
                        //Add new ingredients to the database.
                        foreach (dbIngredient stagedIngredient in RecipeIngredients)
                        {
                            bool matched = false;
                            foreach (dbIngredient ingredient in currentIngredients)
                            {
                                if (ingredient.Id == stagedIngredient.Id)
                                { matched = true; }
                            }
                            if (!matched)
                            { 
                                stagedIngredient.RecipeId = targetRecipe.Id;
                                await databaseService.AddIngredient(stagedIngredient);                                
                            }
                        }

                        int lastPos = 0;
                        //Delete directions if removed during editing.
                        List<dbDirection> currentDirections = await databaseService.GetDirections(target_dbRecipe.Id);
                        foreach (dbDirection Direction in currentDirections)
                        {
                            bool matched = false;
                            foreach (dbDirection stagedDirection in RecipeDirections)
                            {
                                if (Direction.Id == stagedDirection.Id)
                                { matched = true; }
                                if (stagedDirection.OrderPosition > lastPos)
                                {
                                    lastPos = stagedDirection.OrderPosition;
                                }
                            }
                            if (!matched)
                            { await databaseService.DeleteDirection(Direction); }
                        }
                        //Add new directions to the database.
                        foreach (dbDirection stagedDirection in RecipeDirections)
                        {
                            if (stagedDirection.OrderPosition <= 0)
                            { stagedDirection.OrderPosition = ++lastPos; }
                            bool matched = false;
                            foreach (dbDirection Direction in currentDirections)
                            {
                                if (Direction.Id == stagedDirection.Id)
                                { matched = true; }
                            }
                            if (!matched)
                            {
                                stagedDirection.RecipeId = targetRecipe.Id;
                                await databaseService.AddDirection(stagedDirection);
                            }
                        }
                        await userSession.UpdateUserRecipes();
                        await Navigation.PopAsync();
                    }
                }
            }
        }
    }
}