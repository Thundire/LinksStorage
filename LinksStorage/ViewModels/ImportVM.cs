using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using LinksStorage.Services;
using LinksStorage.Shared;

namespace LinksStorage.ViewModels;

public partial class ImportVM : ObservableObject
{
	private readonly IMessenger _messenger;

	[ObservableProperty] 
    private string _value;

    public ImportVM(IMessenger messenger) => _messenger = messenger;

    [RelayCommand]
    private async Task Import()
    {
        if (Value is not { Length: > 2 }) return;

        var data = JsonSerializer.Deserialize<JsonData>(Value);
        _messenger.Send(new Import(data));

        await Shell.Current.GoToAsync("..");
    }
}