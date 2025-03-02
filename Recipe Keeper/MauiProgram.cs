using Microsoft.Extensions.Logging;
using Recipe_Keeper.Database;

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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
