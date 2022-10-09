using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class GroupPage : ContentPage
{
    public GroupPage(GroupVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private GroupVM VM => (GroupVM)BindingContext;

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await VM.Refresh();
        base.OnNavigatedTo(args);
    }

    private async void NavigateToHome(object sender, EventArgs e)
    {
       await AppShell.ToRoot();
    }

    private async void NavigateBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}