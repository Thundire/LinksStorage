using LinksStorage.Pages;
using LinksStorage.ViewModels;

namespace LinksStorage;

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
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FABrand");
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FAFreeRegular");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FAFreeSolid");
            });

        builder.Services
            .AddTransient<RootVM>()
            .AddTransient<MainPage>()
            .AddTransient<GroupVM>()
            .AddTransient<GroupPage>();

        return builder.Build();
    }
}