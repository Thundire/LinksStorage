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
    }
}