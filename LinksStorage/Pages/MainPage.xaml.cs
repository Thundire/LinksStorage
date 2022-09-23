using CommunityToolkit.Maui.Views;
using LinksStorage.Forms;
using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(RootVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void PopupAddGroupForm(object sender, EventArgs e)
    {
        GroupEditForm editForm = new()
        {
            CanBeDismissedByTappingOutsideOfPopup = true
        };

        this.ShowPopup(editForm);
    }
}