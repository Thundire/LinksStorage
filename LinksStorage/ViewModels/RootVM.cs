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
    
    private void AddGroup(object _, GroupName args)
    {
        Groups.Add(args.Name);
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
    [ObservableProperty] private string _group;

    public GroupVM()
    {
        Links = new();
        Groups = new();
        MessagingCenter.Subscribe<GroupEditForm, GroupName>(this, "changeGroupName", AddGroup);
    }

    public ObservableCollection<string> Links { get; }
    public ObservableCollection<string> Groups { get; }

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