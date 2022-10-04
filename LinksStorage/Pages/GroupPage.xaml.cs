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
}