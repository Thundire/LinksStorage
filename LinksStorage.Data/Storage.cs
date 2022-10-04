using LinksStorage.Data.TablesProtoModels;
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
        await _connection.CreateTablesAsync<FavoriteLink, Group, Link>();
        await _connection.ExecuteAsync("insert into groups(Name) select 'root' where not exists (select 1 from groups where Id = 1 and Name = 'root')");
        return this;
    }

    public async Task<int> AddGroup(string name, int parentGroupId)
    {
        Group group = new() { Name = name, GroupId = parentGroupId };
        await _connection.InsertAsync(group);
        return group.Id;
    }

    public async Task<int> AddLink(string alias, string url, int parentGroupId)
    {
        Link link = new() { Name = alias, Url = url, GroupId = parentGroupId };
        await _connection.InsertAsync(link);
        return link.Id;
    }

    public async Task UpdateLink(int id, string name, string url)
    {
        await _connection.ExecuteAsync($"update links set Name = '{name}', Url = '{url}' where Id = {id}");
    }

    public async Task<LinkInfoData> RegisterFavoriteLink(int linkId)
    {
        var link = await _connection.FindAsync<Link>(linkId);
        FavoriteLink favorite = new() { LinkId = linkId, GroupId = link.GroupId };
        await _connection.InsertAsync(favorite);
        return new () { Id = linkId, Name = link.Name, Url = link.Url };
    }

    public async Task<GroupData> GetRootPage()
    {
        var hotLinks = await _connection.QueryAsync<LinkInfoData>("select l.Id, l.Name, l.Url from links as l inner join favorites as f on l.Id = f.LinkId");
        var groups = await _connection.QueryAsync<GroupInfoData>("select Id, Name from groups where GroupId = 1");
        return new(hotLinks, groups);
    }

    public async Task<GroupData> GetGroup(int groupId)
    {
        var links = await _connection.QueryAsync<LinkInfoData>("select Id, Name, Url from links where GroupId = ?", groupId);
        var groups = await _connection.QueryAsync<GroupInfoData>("select Id, Name from groups where GroupId = ?", groupId);
        return new(links, groups);
    }

    public async Task ChangeGroupName(int groupId, string name)
    {
        await _connection.ExecuteAsync($"update groups set name = '{name}' where id = {groupId}");
    }

    public async Task RemoveLink(int id)
    {
        await _connection.RunInTransactionAsync(c =>
        {
            c.Execute("delete from links where Id = ?", id);
            c.Execute("delete from favorites where LinkId = ?", id);
        });
    }

    public async Task RemoveGroup(int id)
    {
        await _connection.RunInTransactionAsync(c =>
        {
            // delete group
            c.Delete<Group>(id);
            // delete groups that inside group
            c.Execute("delete from groups where GroupId = ?", id);
            // delete favorite links in group
            c.Execute("delete from favorites where GroupId = ?", id);
            // delete links in group
            c.Execute("delete from links where GroupId = ?", id);
        });
    }

    public async Task RemoveLinkFromFavorites(int id)
    {
        await _connection.ExecuteAsync("delete from favorites where LinkId = ?", id);
    }
}