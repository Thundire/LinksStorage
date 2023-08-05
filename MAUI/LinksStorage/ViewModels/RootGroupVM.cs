using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LinksStorage.Data;
using LinksStorage.Services;
using Microsoft.Extensions.Configuration;

namespace LinksStorage.ViewModels;

public partial class RootGroupVM : ObservableObject, IDisposable
{
    protected readonly IMessenger _messenger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly BrowserLauncherService _browserLauncherService;
    protected static readonly object SenderMark = new();
    public int GroupId { get; set; }

    [ObservableProperty] private string _groupName;
    [ObservableProperty] private bool _isNotFromRoot;
    [ObservableProperty] private ObservableCollection<LinkInfo> _links;
    [ObservableProperty] private ObservableCollection<GroupInfo> _groups;

    public RootGroupVM(
        IMessenger messenger,
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

        messenger.Register<MarkedLinkAsFavorite>(this, MarkLinkAsFavorite);
        messenger.Register<RemovedMarkLinkAsFavorite>(this, RemoveMarkLinkAsFavorite);
        messenger.Register<RemovedLink>(this, RemoveLink);
        messenger.Register<RemovedGroup>(this, RemoveGroup);
        messenger.Register<CreatedGroup>(this, AddGroup);
        messenger.Register<EditLink>(this, UpdateLink);
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
        _messenger.Send(new CreateGroup(input, GroupId));
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
        _messenger.Send(new ChangeGroupName(payload.Id, input));
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
        _messenger.Send(new RemoveGroup(payload.Id));
    }

    [RelayCommand]
    private void RemoveLink(LinkInfo payload)
    {
        _messenger.Send(new RemoveLink(payload.Id));
    }

    [RelayCommand]
    private void RemoveMarkLinkAsFavorite(LinkInfo payload)
    {
        _messenger.Send(new RemoveMarkLinkAsFavorite(payload.Id));
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
            nameof(Import),
            "DataBase path");

        switch (action)
        {
            case nameof(Export):
                await Export();
                break;
            case nameof(Import):
                await Import();
                break;
            case "DataBase path":
            {
                using var scope = _scopeFactory.CreateScope();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                await Shell.Current.DisplayPromptAsync("Database", "Path", initialValue:configuration["database"]);
                break;
            }
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

    private void AddGroup(object _, CreatedGroup args)
    {
        if (args.ParentGroupId != GroupId) return;
        Groups.Add(new() { Id = args.Id, Name = args.Name });
    }

    private void RemoveGroup(object _, RemovedGroup args)
    {
        _ = Refresh();
    }

    private void RemoveLink(object _, RemovedLink args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        Links.Remove(entry);
    }

    protected virtual void RemoveMarkLinkAsFavorite(object _, RemovedMarkLinkAsFavorite args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        Links.Remove(entry);
    }

    protected virtual void MarkLinkAsFavorite(object _, MarkedLinkAsFavorite args)
    {
        Links.Add(new() { Id = args.Id, Name = args.Name, Url = args.Url, IsFavorite = true });
    }

    private void UpdateLink(object _, EditLink args)
    {
        Links.FirstOrDefault(i => i.Id == args.Id)?.Update(args);
    }

    protected virtual Task<GroupData> GetGroupData(int groupId, Storage storage) => storage.GetRootPage();

    public virtual void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}