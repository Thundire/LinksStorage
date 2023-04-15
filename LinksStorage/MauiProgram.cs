using CommunityToolkit.Maui;
using LinksStorage.Data;
using LinksStorage.Pages;
using LinksStorage.Resources;
using LinksStorage.Services;
using LinksStorage.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LinksStorage;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", FontFamilies.Brand);
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", FontFamilies.Regular);
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", FontFamilies.Solid);
            });

        builder.Services
            .AddTransient<RootGroupVM>()
            .AddTransient<MainPage>()
            .AddTransient<LinkEditVM>()
            .AddTransient<LinkEditPage>()
            .AddTransient<GroupVM>()
            .AddTransient<GroupPage>()
            .AddTransient<ImportVM>()
            .AddTransient<ImportPage>();

        builder.Configuration["database"] = Path.Combine(FileSystem.AppDataDirectory, "LinksStorage.sqlite3");
        builder.Services.AddScoped<Storage>(provider => 
            ActivatorUtilities.CreateInstance<Storage>(
                provider, 
                provider.GetRequiredService<IConfiguration>()["database"]));
        builder.Services.AddSingleton<DataPersistenceOutbox>();
        builder.Services.AddSingleton<IMessagingCenter, MessagingCenter>();
        builder.Services.AddScoped<BrowserLauncherService>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
    }
}