using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LinksStorage.Data;
using System.Text.Json;
using LinksStorage.Shared;

namespace LinksStorage.ViewModels;

public partial class ImportVM : ObservableObject
{
    private readonly IServiceScopeFactory _scopeFactory;

    [ObservableProperty] 
    private string _value;

    public ImportVM(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    [RelayCommand]
    private async Task Import()
    {
        if (Value is not { Length: > 2 }) return;

        var data = JsonSerializer.Deserialize<List<JsonGroup>>(Value);
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        await storage.Import(data);

        await Shell.Current.GoToAsync("..");
    }
}