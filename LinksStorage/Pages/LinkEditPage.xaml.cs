using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class LinkEditPage : ContentPage
{
    public LinkEditPage(LinkEditVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}