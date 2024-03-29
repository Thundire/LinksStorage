﻿using CommunityToolkit.Mvvm.Messaging;
using LinksStorage.Data;

namespace LinksStorage.Services;

public class DataPersistenceOutbox : IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessenger _messenger;

    public DataPersistenceOutbox(IServiceScopeFactory scopeFactory, IMessenger messenger)
    {
        _scopeFactory = scopeFactory;
        _messenger = messenger;

        messenger.Register<CreateLink>(this, CreateLink);
        messenger.Register<EditLink>(this, EditLink);
        messenger.Register<MarkLinkAsFavorite>(this, MarkLinkAsFavorite);
        messenger.Register<RemoveMarkLinkAsFavorite>(this, RemoveMarkLinkAsFavorite);
        messenger.Register<RemoveLink>(this, RemoveLink);

        messenger.Register<CreateGroup>(this, CreateGroup);
        messenger.Register<ChangeGroupName>(this, ChangeGroupName);
        messenger.Register<RemoveGroup>(this, RemoveGroup);
    }

    private async void CreateLink(object _, CreateLink command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        var id = await storage.AddLink(command.Name, command.Url, command.GroupId);
        _messenger.Send(new CreatedLink(id, command.Name, command.Url, command.GroupId));
    }

    private async void EditLink(object _, EditLink command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        await storage.UpdateLink(command.Id, command.Name, command.Url);
        _messenger.Send(new EditedLink(command.Id, command.Name, command.Url, command.GroupId));
    }

    private async void MarkLinkAsFavorite(object _, MarkLinkAsFavorite command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        var linkInfo = await storage.RegisterFavoriteLink(command.Id);
        _messenger.Send(new MarkedLinkAsFavorite(command.Id, linkInfo.Name, linkInfo.Url));
    }

    private async void RemoveMarkLinkAsFavorite(object _, RemoveMarkLinkAsFavorite command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        await storage.RemoveLinkFromFavorites(command.Id);
        _messenger.Send(new RemovedMarkLinkAsFavorite(command.Id));
    }

    private async void RemoveLink(object _, RemoveLink command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        await storage.RemoveLink(command.Id);
        _messenger.Send(new RemovedLink(command.Id));
    }

    private async void CreateGroup(object _, CreateGroup command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        var id = await storage.AddGroup(command.Name, command.ParentGroupId);
        _messenger.Send(new CreatedGroup(id, command.Name, command.ParentGroupId));
    }

    private async void ChangeGroupName(object _, ChangeGroupName command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        await storage.ChangeGroupName(command.Id, command.Name);
        _messenger.Send(new ChangedGroupName(command.Id, command.Name));
    }

    private async void RemoveGroup(object _, RemoveGroup command)
    {
        using var scope = _scopeFactory.CreateScope();
        var storage = await scope.ServiceProvider.GetRequiredService<Storage>().Initialize();
        await storage.RemoveGroup(command.Id);
        _messenger.Send(new RemovedGroup(command.Id));
    }

	public void Dispose()
	{
		_messenger.UnregisterAll(this);
	}
}

public record CreateLink(string Name, string Url, int GroupId);
public record EditLink(int Id, string Name, string Url, int GroupId);
public record MarkLinkAsFavorite(int Id);
public record RemoveMarkLinkAsFavorite(int Id);
public record RemoveLink(int Id);

public record CreateGroup(string Name, int ParentGroupId);
public record ChangeGroupName(int Id, string Name);
public record RemoveGroup(int Id);

public record CreatedLink(int Id, string Name, string Url, int GroupId);
public record EditedLink(int Id, string Name, string Url, int GroupId);
public record MarkedLinkAsFavorite(int Id, string Name, string Url);
public record RemovedMarkLinkAsFavorite(int Id);
public record RemovedLink(int Id);

public record CreatedGroup(int Id, string Name, int ParentGroupId);
public record ChangedGroupName(int Id, string Name);
public record RemovedGroup(int Id);