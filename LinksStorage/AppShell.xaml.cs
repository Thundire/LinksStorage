using LinksStorage.Pages;

namespace LinksStorage;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("group", typeof(GroupPage));
    }
}