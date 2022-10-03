using CommunityToolkit.Maui;
using LinksStorage.Data;
using LinksStorage.Pages;
using LinksStorage.Services;
using LinksStorage.ViewModels;
using Microsoft.Extensions.Configuration;

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
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FABrand");
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FAFreeRegular");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FAFreeSolid");
            });

        builder.Services
            .AddTransient<RootGroupVM>()
            .AddTransient<MainPage>()
            .AddTransient<LinkEditVM>()
            .AddTransient<LinkEditPage>()
            .AddTransient<GroupVM>()
            .AddTransient<GroupPage>();

        builder.Configuration["database"] = Path.Combine(FileSystem.AppDataDirectory, "LinksStorage.sqlite3");
        builder.Services.AddScoped<Storage>(provider => 
            ActivatorUtilities.CreateInstance<Storage>(
                provider, 
                provider.GetRequiredService<IConfiguration>()["database"]));
        builder.Services.AddSingleton<DataPersistenceOutbox>();

        return builder.Build();
    }
}