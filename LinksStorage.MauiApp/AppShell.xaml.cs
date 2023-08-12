using LinksStorage.Pages;
using LinksStorage.ViewModels;

namespace LinksStorage;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(NavigationRoutes.Group, typeof(GroupPage));
        Routing.RegisterRoute(NavigationRoutes.LinkEditForm, typeof(LinkEditPage));
        Routing.RegisterRoute(NavigationRoutes.Import, typeof(ImportPage));
    }

    public static async Task ToRoot()
    {
        var navigation = Shell.Current.Navigation;
        while (navigation.ModalStack.Count > 0)
        {
            await navigation.PopModalAsync(false);
        }

        while (navigation.NavigationStack.Count > 1)
        {
            await navigation.PopAsync(false);
        }
    }
}