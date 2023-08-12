using LinksStorage.ViewModels;

namespace LinksStorage.Pages;

public partial class ImportPage : ContentPage
{
    public ImportPage(ImportVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void Back(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}