﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LinksStorage.Data;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

public partial class RootGroupVM : ObservableObject, IDisposable
{
    protected readonly IMessagingCenter _messenger;
    private readonly IServiceScopeFactory _scopeFactory;
    protected static readonly object SenderMark = new();
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
        messenger.Subscribe<DataPersistenceOutbox, EditLink>(this, nameof(Services.EditLink), UpdateLink);
    }

    [RelayCommand]
    public async Task Refresh()
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
    private void OpenLinkInBrowser(LinkInfo payload)
    {

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
        _messenger.Unsubscribe<DataPersistenceOutbox, EditLink>(this, nameof(Services.EditLink));
    }
}