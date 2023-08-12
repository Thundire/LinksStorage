using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class LinkEditPage : ContentPage
{
    public LinkEditPage(LinkEditVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void Back(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}