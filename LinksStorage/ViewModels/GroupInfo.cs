using CommunityToolkit.Mvvm.ComponentModel;
using LinksStorage.Data;

namespace LinksStorage.ViewModels;

public partial class GroupInfo : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name;

    public GroupInfo() { }
    public GroupInfo(GroupInfoData data)
    {
        _id = data.Id;
        _name = data.Name;
    }
}