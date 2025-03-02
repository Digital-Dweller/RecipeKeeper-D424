using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipe_Keeper.Classes;
using Recipe_Keeper.Database;
using Recipe_Keeper.Pages;

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
                bool IsLoggedIn = false;
                DatabaseService databaseService = s.GetService<DatabaseService>();
                return new UserSession(IsLoggedIn, databaseService);
            });

            //Add all the pages to the DI container.
            builder.Services.AddTransient<LandingPage>();
            builder.Services.AddTransient<Login>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
