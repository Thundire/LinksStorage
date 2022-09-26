using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class GroupPage : ContentPage
{
    public GroupPage(GroupVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}