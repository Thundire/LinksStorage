using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(RootGroupVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}