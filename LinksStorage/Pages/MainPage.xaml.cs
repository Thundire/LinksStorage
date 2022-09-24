using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(RootVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}