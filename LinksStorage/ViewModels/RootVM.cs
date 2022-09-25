using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LinksStorage.ViewModels;

public partial class RootVM : ObservableObject, IDisposable
{
    public RootVM()
    {
        HotLinks = new();
        Groups = new();

        MessagingCenter.Subscribe<object, LinkInfo>(this, Messages.EditLink, UpdateHotLink);
    }

    public ObservableCollection<LinkInfo> HotLinks { get; }
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
        await Shell.Current.GoToAsync(NavigationRoutes.Group, new Dictionary<string, object>()
        {
            ["group"] = group
        });
    }

    [RelayCommand]
    private async Task EditLink(LinkInfo info)
    {
        await Shell.Current.GoToAsync(NavigationRoutes.LinkEditForm, new Dictionary<string, object>()
        {
            ["info"] = new LinkCreateInfo(NavigationRoutes.Root)
        });
    }

    private void UpdateHotLink(object _, LinkInfo info)
    {
        HotLinks.FirstOrDefault(i => i.Id == info.Id)?.Update(info);
    }

    public void Dispose()
    {
        MessagingCenter.Unsubscribe<object, LinkInfo>(this, Messages.EditLink);
    }
}

[QueryProperty(nameof(Group), "group")]
public partial class GroupVM : ObservableObject, IDisposable
{
    [ObservableProperty] private string _group;

    public GroupVM()
    {
        Links = new();
        Groups = new();

        MessagingCenter.Subscribe<object, LinkInfo>(this, Messages.CreateLink, AddLink);
        MessagingCenter.Subscribe<object, LinkInfo>(this, Messages.EditLink, EditLink);
    }

    public ObservableCollection<LinkInfo> Links { get; }
    public ObservableCollection<string> Groups { get; }

    [RelayCommand]
    private async Task AddGroup()
    {
        var input = await Shell.Current.DisplayPromptAsync("Add Group", "Enter group name");
        if (input is not { Length: > 0 }) return;
        Groups.Add(input);
    }

    [RelayCommand]
    private async Task OpenGroup(string group)
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Group, new Dictionary<string, object>()
        {
            ["group"] = group
        });
    }

    [RelayCommand]
    private async Task AddLink()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.LinkEditForm, new Dictionary<string, object>()
        {
            ["info"] = new LinkCreateInfo(_group)
        });
    }

    [RelayCommand]
    private async Task EditLink(LinkInfo info)
    {
        await Shell.Current.GoToAsync(NavigationRoutes.LinkEditForm, new Dictionary<string, object>()
        {
            ["info"] = new LinkEditInfo(_group, info.Id, info.Alias, info.Url)
        });
    }

    private void AddLink(object _, LinkInfo info)
    {
        Links.Add(info);
    }
    private void EditLink(object _, LinkInfo info)
    {
        Links.FirstOrDefault(i => i.Id == info.Id)?.Update(info);
    }

    public void Dispose()
    {
        MessagingCenter.Unsubscribe<object, LinkInfo>(this, Messages.CreateLink);
        MessagingCenter.Unsubscribe<object, LinkInfo>(this, Messages.EditLink);
    }
}

public partial class LinkEditVM : ObservableObject, IQueryAttributable
{
    private string _parent;
    private bool _isFromRoot;
    private Guid _id;

    [ObservableProperty] 
    private bool _isNew;

    [ObservableProperty]
    private string _alias;
    [ObservableProperty]
    private string _url;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var info = query["info"];
        if (info is LinkCreateInfo createInfo)
        {
            _id = Guid.NewGuid();
            _isNew = true;
            _parent = createInfo.Parent;
            return;
        }

        if (info is not LinkEditInfo editInfo) throw new InvalidOperationException("Link Edit VM need for edit or create links");

        _id = editInfo.Id;
        _parent = editInfo.Parent;
        Alias = editInfo.Alias;
        Url = editInfo.Url;
        _isFromRoot = editInfo.IsFromRoot;
    }

    [RelayCommand]
    private async Task Save()
    {
        string message = _isNew ? Messages.CreateLink : Messages.EditLink;
        LinkInfo info = new()
        {
            Id = _isNew ? Guid.NewGuid() : _id,
            Alias = Alias,
            Url = Url,
            Parent = _parent
        };
        MessagingCenter.Send<object, LinkInfo>(new object(), message, info);
        await Shell.Current.GoToAsync("..", new Dictionary<string, object>()
        {
            ["group"] = _isFromRoot ? null : _parent
        });
    }
}

public static class Messages
{
    public const string EditLink = nameof(EditLink);
    public const string CreateLink = nameof(CreateLink);
}

public static class NavigationRoutes
{
    public const string Root = "root";
    public const string Group = "group";
    public const string LinkEditForm = "linkEditForm";
}

public record LinkEditInfo(string Parent, Guid Id, string Alias, string Url)
{
    public bool IsFromRoot => Parent == NavigationRoutes.Root;
}

public partial class LinkInfo : ObservableObject
{
    [ObservableProperty] private Guid _id;
    [ObservableProperty] private string _alias;
    [ObservableProperty] private string _url;
    [ObservableProperty] private string _parent;
    [ObservableProperty] private bool _isHotLink;

    public void Update(LinkInfo data)
    {
        Alias = data.Alias;
        Url = data.Url;
    }
}

public record LinkCreateInfo(string Parent);