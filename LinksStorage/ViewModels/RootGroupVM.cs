using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LinksStorage.Data;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

public partial class RootGroupVM : ObservableObject, IDisposable
{
    protected readonly IMessagingCenter _messenger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly BrowserLauncherService _browserLauncherService;
    protected static readonly object SenderMark = new();
    public int GroupId { get; set; }

    [ObservableProperty] private string _groupName;
    [ObservableProperty] private bool _isNotFromRoot;
    [ObservableProperty] private ObservableCollection<LinkInfo> _links;
    [ObservableProperty] private ObservableCollection<GroupInfo> _groups;

    public RootGroupVM(
        IMessagingCenter messenger,
        IServiceScopeFactory scopeFactory,
        BrowserLauncherService browserLauncherService)
    {
        GroupId = 1;
        GroupName = "Home";

        _messenger = messenger;
        _scopeFactory = scopeFactory;
        _browserLauncherService = browserLauncherService;

        Links = new();
        Groups = new();

        messenger.Subscribe<DataPersistenceOutbox, MarkedLinkAsFavorite>(this, nameof(MarkedLinkAsFavorite), MarkLinkAsFavorite);
        messenger.Subscribe<DataPersistenceOutbox, RemovedMarkLinkAsFavorite>(this, nameof(RemovedMarkLinkAsFavorite), RemoveMarkLinkAsFavorite);
        messenger.Subscribe<DataPersistenceOutbox, RemovedLink>(this, nameof(RemovedLink), RemoveLink);
        messenger.Subscribe<DataPersistenceOutbox, RemovedGroup>(this, nameof(RemovedGroup), RemoveGroup);
        messenger.Subscribe<DataPersistenceOutbox, CreatedGroup>(this, nameof(CreatedGroup), AddGroup);
        messenger.Subscribe<DataPersistenceOutbox, EditLink>(this, nameof(Services.EditLink), UpdateLink);
    }

    [RelayCommand]
    public async Task Refresh()
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        var rootData = await GetGroupData(GroupId, storage);

        Links = new(rootData.Links.Select(x => new LinkInfo(x) { IsFavorite = x.IsFavorite }));
        Groups = new(rootData.Groups.Select(x => new GroupInfo(x)));
    }

    [RelayCommand]
    private async Task AddGroup()
    {
        var input = await Shell.Current.DisplayPromptAsync("Add Group", "Enter group name");
        if (input is not { Length: > 0 }) return;
        _messenger.Send(SenderMark, nameof(CreateGroup), new CreateGroup(input, GroupId));
    }

    [RelayCommand]
    private async Task OpenGroup(GroupInfo group)
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Group, new Dictionary<string, object>
        {
            ["groupId"] = group.Id,
            ["groupName"] = group.Name,
            ["notFromRoot"] = GroupId != 1
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
        _messenger.Send(SenderMark, nameof(Services.RemoveGroup), new RemoveGroup(payload.Id));
    }

    [RelayCommand]
    private void RemoveLink(LinkInfo payload)
    {
        _messenger.Send(SenderMark, nameof(Services.RemoveLink), new RemoveLink(payload.Id));
    }

    [RelayCommand]
    private void RemoveMarkLinkAsFavorite(LinkInfo payload)
    {
        _messenger.Send(SenderMark, nameof(Services.RemoveMarkLinkAsFavorite), new RemoveMarkLinkAsFavorite(payload.Id));
    }

    [RelayCommand]
    private async Task OpenLinkInBrowser(LinkInfo payload)
    {
        await _browserLauncherService.Open(payload.Url);
    }

    [RelayCommand]
    private async Task ShowMoreActions()
    {
        string action = await Shell.Current.DisplayActionSheet(
            "Options",
            "Cancel",
            null,
            nameof(Export),
            nameof(Import));

        switch (action)
        {
            case nameof(Export):
                await Export();
                break;
            case nameof(Import):
                await Import();
                break;
        }
    }
    
    private async Task Export()
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        var data = await storage.Export();
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions(){WriteIndented = true});
        await Clipboard.Default.SetTextAsync(json);
    }
    
    private async Task Import()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Import);
    }

    private void AddGroup(DataPersistenceOutbox _, CreatedGroup args)
    {
        if (args.ParentGroupId != GroupId) return;
        Groups.Add(new() { Id = args.Id, Name = args.Name });
    }

    private void RemoveGroup(DataPersistenceOutbox _, RemovedGroup args)
    {
        Refresh();
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
        Links.Add(new() { Id = args.Id, Name = args.Name, Url = args.Url, IsFavorite = true });
    }

    private void UpdateLink(DataPersistenceOutbox _, EditLink args)
    {
        Links.FirstOrDefault(i => i.Id == args.Id)?.Update(args);
    }

    protected virtual Task<GroupData> GetGroupData(int groupId, Storage storage) => storage.GetRootPage();

    public virtual void Dispose()
    {
        _messenger.Unsubscribe<DataPersistenceOutbox, MarkedLinkAsFavorite>(this, nameof(MarkedLinkAsFavorite));
        _messenger.Unsubscribe<DataPersistenceOutbox, RemovedMarkLinkAsFavorite>(this, nameof(RemovedMarkLinkAsFavorite));
        _messenger.Unsubscribe<DataPersistenceOutbox, RemovedLink>(this, nameof(RemovedLink));
        _messenger.Unsubscribe<DataPersistenceOutbox, RemovedGroup>(this, nameof(RemovedGroup));
        _messenger.Unsubscribe<DataPersistenceOutbox, CreatedGroup>(this, nameof(CreatedGroup));
        _messenger.Unsubscribe<DataPersistenceOutbox, EditLink>(this, nameof(Services.EditLink));
    }
}