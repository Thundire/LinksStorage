﻿using LinksStorage.Data.TablesProtoModels;

using SQLite;

namespace LinksStorage.Data;

public class Storage
{
	private readonly SQLiteAsyncConnection _connection;

	public Storage(string connectionString)
	{
		_connection = new(connectionString);
	}

	public async Task<Storage> Initialize()
	{
		await _connection.CreateTablesAsync<Group, Link>().ConfigureAwait(false);
		await _connection.ExecuteAsync("insert into groups(name) select 'root' where not exists (select 1 from groups where id = 1 and name = 'root')");
		return this;
	}

	public async Task<int> AddGroup(string name, int parentGroupId)
	{
		Group group = new() { Name = name, GroupId = parentGroupId };
		await _connection.InsertAsync(group).ConfigureAwait(false);
		return group.Id;
	}

	public async Task<int> AddLink(string alias, string url, int parentGroupId)
	{
		Link link = new() { Name = alias, Url = url, GroupId = parentGroupId };
		await _connection.InsertAsync(link).ConfigureAwait(false);
		return link.Id;
	}

	public async Task UpdateLink(int id, string name, string url)
	{
		await _connection.ExecuteAsync($"update links set name = '{name}', url = '{url}' where id = {id}").ConfigureAwait(false);
	}

	public async Task<LinkInfoData> RegisterFavoriteLink(int linkId)
	{
		var link = await _connection.FindAsync<Link>(linkId);
		await _connection.ExecuteAsync("update links set favorite = 1 where id = ?", linkId).ConfigureAwait(false);
		return new() { Id = linkId, Name = link.Name, Url = link.Url, IsFavorite = true };
	}

	public async Task<GroupData> GetRootPage()
	{
		var hotLinks = await _connection.QueryAsync<LinkInfoData>("select id, name, url, favorite from links where favorite = 1").ConfigureAwait(false);
		var groups = await _connection.QueryAsync<GroupInfoData>("select id, name from groups where group_id = 1");
		return new(hotLinks, groups);
	}

	public async Task<GroupData> GetGroup(int groupId)
	{
		var links = await _connection.QueryAsync<LinkInfoData>("select id, name, url, favorite from links where group_id = ?", groupId).ConfigureAwait(false);
		var groups = await _connection.QueryAsync<GroupInfoData>("select id, name from groups where group_id = ?", groupId);
		return new(links, groups);
	}

	public async Task ChangeGroupName(int groupId, string name)
	{
		await _connection.ExecuteAsync($"update groups set name = '{name}' where id = {groupId}").ConfigureAwait(false);
	}

	public async Task RemoveLink(int id)
	{
		await _connection.DeleteAsync<Link>(id).ConfigureAwait(false);
	}

	public async Task RemoveGroup(int id)
	{
		await _connection.RunInTransactionAsync(c =>
		{
			// delete group
			c.Delete<Group>(id);
			// delete groups that inside group
			c.Execute("delete from groups where group_id = ?", id);
			// delete links in group
			c.Execute("delete from links where group_id = ?", id);
		}).ConfigureAwait(false);
	}

	public async Task RemoveLinkFromFavorites(int id)
	{
		var link = await _connection.FindAsync<Link>(id).ConfigureAwait(false);
		await _connection.ExecuteAsync("update links set favorite = 0 where id = ?", id);
	}

	public async Task<List<JsonGroup>> Export()
	{
		var links = await _connection.QueryAsync<Link>("select * from links").ConfigureAwait(false);
		var groups = await _connection.QueryAsync<Group>("select * from groups where group_id >= 1");
		JsonGroup groupImport = ToExport(groups, links, string.Empty, 0);
		return groupImport.Groups;
	}

	public async Task Import(List<JsonGroup> data)
	{
		foreach (var group in data)
		{
			await Import(group, 1);
		}
	}

	private async Task Import(JsonGroup data, int parentGroupId)
	{
		var group = new Group { Name = data.Name, GroupId = parentGroupId };
		await _connection.InsertAsync(group).ConfigureAwait(false);

		await _connection.InsertAllAsync(data.Links.Select(x => x.Map(parentGroupId)));
		foreach (var dataGroup in data.Groups)
		{
			await Import(dataGroup, group.Id);
		}
	}

	private JsonGroup ToExport(List<Group> groups, List<Link> links, string name, int id)
	{
		List<JsonGroup> innerGroups = new();
		while (groups.Count > 0 && groups.MinBy(g => g.GroupId) is { } group)
		{
			groups.Remove(group);
			if (ToExport(groups, links, group.Name, group.Id) is { } innerGroup)
			{
				innerGroups.Add(innerGroup);
			}
		}

		List<JsonLink> linksToImport = new();
		if (id > 0)
		{
			var innerLinks = links.Where(x => x.GroupId == id).ToList();
			if (innerLinks.Count > 0)
			{
				foreach (var toRemove in innerLinks)
					links.Remove(toRemove);

				linksToImport.AddRange(innerLinks.Select(x => new JsonLink(x.Name, x.Url, x.IsFavorite)));
			}
		}

		JsonGroup result = new(name) { Groups = innerGroups, Links = linksToImport };
		return result;
	}
}