using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(RootGroupVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private RootGroupVM VM => (RootGroupVM)BindingContext;

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await VM.Refresh();
        base.OnNavigatedTo(args);
    }
}