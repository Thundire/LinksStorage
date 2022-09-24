using System.Collections.ObjectModel;
using System.Security.Cryptography;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LinksStorage.Forms;

namespace LinksStorage.ViewModels;

public partial class RootVM : ObservableObject
{
    public RootVM()
    {
        HotLinks = new();
        Groups = new();
        MessagingCenter.Subscribe<GroupEditForm, GroupName>(this, "changeGroupName", AddGroup);
    }

    public ObservableCollection<string> HotLinks { get; }
    public ObservableCollection<string> Groups { get; }
    
    [RelayCommand]
    private async Task AddGroup()
    {
        var input = await Shell.Current.DisplayPromptAsync("Add Group", "Enter group name");
        if(input is not {Length: > 0}) return;
        Groups.Add(input);
    }

    [RelayCommand]
    private async Task OpenGroup(string group)
    {
        await Shell.Current.GoToAsync("group", new Dictionary<string, object>()
        {
            ["group"] = group
        });
    } 
}

public partial class GroupVM : ObservableObject, IQueryAttributable
{

[QueryProperty(nameof(Group), "group")]
public partial class GroupVM : ObservableObject, IDisposable
{
    [ObservableProperty] private string _group;

    public GroupVM()
    {
        Links = new();
        Groups = new();
        MessagingCenter.Subscribe<GroupEditForm, GroupName>(this, "changeGroupName", AddGroup);
    }

    public ObservableCollection<string> Links { get; }
    public ObservableCollection<string> Groups { get; }

    [RelayCommand]
    private async Task AddGroup(string name)
    {
        var input = await Shell.Current.DisplayPromptAsync("Add Group", "Enter group name");
        if (input is not { Length: > 0 }) return;
        Groups.Add(name);
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _group = query["group"].ToString();
    }

    private void AddGroup(object _, GroupName args)
    {
        Groups.Add(args.Name);
    }
}

public class GroupName
{
    public string Name { get; set; }
}