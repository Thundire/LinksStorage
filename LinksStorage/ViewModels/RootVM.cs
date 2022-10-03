using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LinksStorage.Data;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

public partial class RootGroupVM : ObservableObject, IDisposable
{
    protected readonly IMessagingCenter _messenger;
    private readonly IServiceScopeFactory _scopeFactory;
    private static readonly object SenderMark = new();
    public int GroupId { get; set; }

    [ObservableProperty] private ObservableCollection<LinkInfo> _links;
    [ObservableProperty] private ObservableCollection<GroupInfo> _groups;

    public RootGroupVM(IMessagingCenter messenger, IServiceScopeFactory scopeFactory)
    {
        GroupId = 1;

        _messenger = messenger;
        _scopeFactory = scopeFactory;

        Links = new();
        Groups = new();

        messenger.Subscribe<DataPersistenceOutbox, MarkedLinkAsFavorite>(this, nameof(MarkedLinkAsFavorite), MarkLinkAsFavorite);
        messenger.Subscribe<DataPersistenceOutbox, RemovedMarkLinkAsFavorite>(this, nameof(RemovedMarkLinkAsFavorite), RemoveMarkLinkAsFavorite);
        messenger.Subscribe<DataPersistenceOutbox, RemovedLink>(this, nameof(RemovedLink), RemoveLink);
        messenger.Subscribe<DataPersistenceOutbox, RemovedGroup>(this, nameof(RemovedGroup), RemoveGroup);
        messenger.Subscribe<DataPersistenceOutbox, CreatedGroup>(this, nameof(CreatedGroup), AddGroup);
        messenger.Subscribe<DataPersistenceOutbox, EditLink>(this, nameof(EditLink), UpdateLink);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        var rootData = await storage.GetRootPage();

        Links = new(rootData.Links.Select(x=> new LinkInfo(x)));
        Groups = new(rootData.Groups.Select(x=> new GroupInfo(x)));
    }

    [RelayCommand]
    private async Task AddGroup()
    {
        var input = await Shell.Current.DisplayPromptAsync("Add Group", "Enter group name");
        if (input is not { Length: > 0 }) return;
        _messenger.Send(SenderMark, nameof(CreateGroup), new CreateGroup(input, GroupId));
    }

    [RelayCommand]
    private async Task OpenGroup(int groupId)
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Group, new Dictionary<string, object>()
        {
            ["group"] = groupId
        });
    }

    [RelayCommand]
    private async Task ChangeGroupName(GroupInfo payload)
    {
        var input = await Shell.Current.DisplayPromptAsync("Add Group", "Enter group name", initialValue: payload.Name);
        if (input is not { Length: > 0 }) return;
        _messenger.Send(SenderMark, nameof(ChangeGroupName), new ChangeGroupName(payload.Id, input));
    }

    [RelayCommand]
    private async Task EditLink(LinkInfo payload)
    {
        await Shell.Current.GoToAsync(NavigationRoutes.LinkEditForm, new Dictionary<string, object>()
        {
            ["payload"] = new LinkEditInfo(GroupId, payload.Id, payload.Name, payload.Url)
        });
    }

    [RelayCommand]
    private void RemoveGroup(GroupInfo payload)
    {
        _messenger.Send(SenderMark, nameof(RemoveGroup), new RemoveGroup(payload.Id));
    }

    [RelayCommand]
    private void RemoveLink(LinkInfo payload)
    {
        _messenger.Send(SenderMark, nameof(RemoveLink), new RemoveLink(payload.Id));
    }

    [RelayCommand]
    private void RemoveLinkFromHotList(LinkInfo payload)
    {
        _messenger.Send(SenderMark, nameof(Services.RemoveMarkLinkAsFavorite), new RemoveMarkLinkAsFavorite(payload.Id));
    }

    private void AddGroup(DataPersistenceOutbox _, CreatedGroup args)
    {
        if (args.ParentGroupId != GroupId) return;
        Groups.Add(new() { Id = args.Id, Name = args.Name });
    }

    private void RemoveGroup(DataPersistenceOutbox _, RemovedGroup args)
    {
        if (Groups.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        Groups.Remove(entry);
    }

    private void RemoveLink(DataPersistenceOutbox _, RemovedLink args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        Links.Remove(entry);
    }

    protected virtual void RemoveMarkLinkAsFavorite(DataPersistenceOutbox _, RemovedMarkLinkAsFavorite args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        Links.Remove(entry);
    }

    protected virtual void MarkLinkAsFavorite(DataPersistenceOutbox _, MarkedLinkAsFavorite args)
    {
        Links.Add(new() { Id = args.Id, Name = args.Name, Url = args.Url });
    }

    private void UpdateLink(DataPersistenceOutbox _, EditLink args)
    {
        Links.FirstOrDefault(i => i.Id == args.Id)?.Update(args);
    }

    public virtual void Dispose()
    {
        _messenger.Unsubscribe<DataPersistenceOutbox, MarkedLinkAsFavorite>(this, nameof(MarkedLinkAsFavorite));
        _messenger.Unsubscribe<DataPersistenceOutbox, RemovedMarkLinkAsFavorite>(this, nameof(RemovedMarkLinkAsFavorite));
        _messenger.Unsubscribe<DataPersistenceOutbox, RemovedLink>(this, nameof(RemovedLink));
        _messenger.Unsubscribe<DataPersistenceOutbox, RemovedGroup>(this, nameof(RemovedGroup));
        _messenger.Unsubscribe<DataPersistenceOutbox, CreatedGroup>(this, nameof(CreatedGroup));
        _messenger.Unsubscribe<DataPersistenceOutbox, EditLink>(this, nameof(EditLink));
    }
}

[QueryProperty(nameof(GroupId), "group")]
public sealed partial class GroupVM : RootGroupVM
{
    public GroupVM(IMessagingCenter messenger, IServiceScopeFactory scopeFactory) : base(messenger, scopeFactory)
    {
        messenger.Subscribe<DataPersistenceOutbox, CreatedLink>(this, nameof(CreatedLink), AddLink);

    }

    [RelayCommand]
    private async Task AddLink()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.LinkEditForm, new Dictionary<string, object>()
        {
            ["payload"] = new LinkCreateInfo(GroupId)
        });
    }

    private void AddLink(DataPersistenceOutbox _, CreatedLink args)
    {
        if(args.GroupId != GroupId) return;
        Links.Add(new(){Id = args.Id, Name = args.Name, Url = args.Url});
    }

    protected override void MarkLinkAsFavorite(DataPersistenceOutbox _, MarkedLinkAsFavorite args)
    {
        if(Links.FirstOrDefault(x=>x.Id == args.Id) is not {} entry) return;
        entry.IsFavorite = true;
    }

    protected override void RemoveMarkLinkAsFavorite(DataPersistenceOutbox _, RemovedMarkLinkAsFavorite args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        entry.IsFavorite = false;
    }

    public override void Dispose()
    {
        _messenger.Unsubscribe<DataPersistenceOutbox, CreatedLink>(this, nameof(CreatedLink));
        base.Dispose();
    }
}

public partial class LinkEditVM : ObservableObject, IQueryAttributable
{
    private readonly IMessagingCenter _messenger;
    private static readonly object SenderMark = new();
    private int _group;
    private bool _isFromRoot;
    private int _id;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _url;

    public LinkEditVM(IMessagingCenter messenger)
    {
        _messenger = messenger;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var payload = query["payload"];
        if (payload is LinkCreateInfo createInfo)
        {
            _id = 0;
            _isNew = true;
            _group = createInfo.Group;
            return;
        }

        if (payload is not LinkEditInfo editInfo) throw new InvalidOperationException("Link Edit VM need for edit or create links");

        _id = editInfo.Id;
        _group = editInfo.Group;
        Name = editInfo.Name;
        Url = editInfo.Url;
        _isFromRoot = editInfo.IsFromRoot;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_isNew)
        {
            _messenger.Send(SenderMark, nameof(CreateLink), new CreateLink(_name, _url, _group));
        }
        else
        {
            _messenger.Send(SenderMark, nameof(EditLink), new EditLink(_id, _name, _url, _group));
        }

        await Shell.Current.GoToAsync("..", new Dictionary<string, object>()
        {
            ["group"] = _isFromRoot ? null : _group
        });
    }
}

public static class NavigationRoutes
{
    public const string Root = "root";
    public const string Group = "group";
    public const string LinkEditForm = "linkEditForm";
}

public record LinkEditInfo(int Group, int Id, string Name, string Url)
{
    public bool IsFromRoot => Group == 1;
}

public partial class LinkInfo : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _url;
    [ObservableProperty] private bool _isFavorite;

    public LinkInfo() { }
    public LinkInfo(LinkInfoData data)
    {
        _id = data.Id;
        _name = data.Name;
        _url = data.Url;
    }
    
    public void Update(EditLink data) => Update(data.Name, data.Url);

    private void Update(string name, string url)
    {
        Name = name;
        Url = url;
    }
}

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

public record LinkCreateInfo(int Group);