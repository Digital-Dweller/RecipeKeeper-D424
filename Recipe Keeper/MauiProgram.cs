using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Database;
using Recipe_Keeper.Pages;
using Recipe_Keeper.Controls;

namespace Recipe_Keeper
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Add database services to the DI container.
            builder.Services.AddSingleton(s =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "rkmaDB.db3");
                return new DatabaseService(dbPath);
            });

            //Add UserSession to the DI container.
            builder.Services.AddSingleton<UserSession>(s =>
            {
                var databaseService = s.GetRequiredService<DatabaseService>();
                return new UserSession(databaseService);
            });

            //Add pages to the DI container.
            builder.Services.AddTransient<Loading>();
            builder.Services.AddTransient<LandingPage>();
            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<CreateProfile>();
            builder.Services.AddTransient<Favorites>();
            builder.Services.AddTransient<CreateRecipe>();
            builder.Services.AddTransient<Profile>();
            builder.Services.AddTransient<Recipes>();
            builder.Services.AddTransient<Search>();
            builder.Services.AddTransient<EditRecipes>();

            //Add controls to the DI container.
            builder.Services.AddTransient<TextInput>();
            builder.Services.AddTransient<NavIcon>();
            builder.Services.AddTransient<UiNavButton>();
            builder.Services.AddTransient<AddIngredient>();
            builder.Services.AddTransient<NewIngredient>();
            builder.Services.AddTransient<NewDirection>();
            builder.Services.AddTransient<AddDirection>();
            builder.Services.AddTransient<RecipeCard>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
