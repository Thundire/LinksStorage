using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LinksStorage.ViewModels;

public class RootVM : ObservableObject
{
    public ObservableCollection<string> HotLinks { get; }
    public ObservableCollection<string> Groups { get; }

    public GroupEditFormVM GroupEditForm { get; }
}

public class GroupVM : ObservableObject
{
    public ObservableCollection<string> Links { get; }
    public ObservableCollection<string> Groups { get; }

    public GroupEditFormVM GroupEditForm { get; }
    public LinkEditFormVM LinkEditForm { get; }
}

public class GroupEditFormVM : ObservableObject
{

}

public class LinkEditFormVM : ObservableObject
{

}