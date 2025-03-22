using maui_lab_1.Controls;
using Microsoft.Extensions.Logging;
#if ANDROID
using maui_lab_1.Platforms.Android;
#endif
using maui_lab_1.Services;
using WeatherMauiApp;
namespace maui_lab_1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(BorderedEntry), typeof(BorderedEntryHandler));
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton(new WeatherService("a08b57d9b7ce4af38c3121348252103"));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
