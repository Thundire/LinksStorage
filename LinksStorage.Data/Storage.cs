using LinksStorage.Data.TablesProtoModels;
using LinksStorage.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace LinksStorage.Data;

public class Storage
{
    private readonly DataContext _context;

    public Storage(string connectionString)
    {
        _context = new(connectionString);
    }

    public async Task<Storage> Initialize()
    {
        await _context.Database.MigrateAsync();
        return this;
    }

    public async Task<Guid> AddGroup(string name, Guid parentGroupId)
    {
        Group? parentGroup = await _context.Groups.FindAsync(parentGroupId);
        if(parentGroup is null) return Guid.Empty;
        
        Group group = new() { Name = name };

		parentGroup.Groups.Add(group);

        await _context.SaveChangesAsync();

        return group.Id;
    }

    public async Task<Guid> AddLink(string alias, string url, Guid parentGroupId)
    {
		Group? parentGroup = await _context.Groups.FindAsync(parentGroupId);
		if (parentGroup is null) return Guid.Empty;

		Link link = new() { Name = alias, Url = url };

        parentGroup.Links.Add(link);

		await _context.SaveChangesAsync();

		return link.Id;
    }

    public async Task UpdateLink(Guid id, string name, string url)
    {
        Link? link = await _context.Links.FindAsync(id);
        if (link is null) return;

		link.Name = name;
        link.Url = url;

		await _context.SaveChangesAsync();
	}

    public async Task<LinkInfoData> RegisterFavoriteLink(Guid linkId)
    {
        Link? link = await _context.FindAsync<Link>(linkId);
        if (link is null) return LinkInfoData.Empty;

		link.IsFavorite = true;

		await _context.SaveChangesAsync();

		return new() { Id = linkId, Name = link.Name, Url = link.Url, IsFavorite = true };
    }

    public async Task<GroupData> GetRootPage()
    {
        var hotLinks = await _context.Links.AsNoTracking().Where(x => x.IsFavorite).Select(x => new LinkInfoData() { Id = x.Id, Name = x.Name, Url = x.Url, IsFavorite = true }).ToListAsync();
        var groups = await _context.Groups.AsNoTracking().Where(x => x.Parent == null).Select(x => new GroupInfoData() { Id = x.Id, Name = x.Name }).ToListAsync();
        return new(hotLinks, groups);
    }

    public async Task<GroupData> GetGroup(Guid groupId)
    {
		GroupData? result = await _context.Groups
			.AsNoTracking()
	        .Where(x => x.Id == groupId)
	        .Select(x => new GroupData(
		        x.Links.Select(l =>  new LinkInfoData  { Id = l.Id, Name = l.Name, Url = l.Url, IsFavorite = l.IsFavorite }),
		        x.Groups.Select(g => new GroupInfoData { Id = g.Id, Name = g.Name })))
	        .FirstOrDefaultAsync();
		return result ?? GroupData.Empty;
    }

    public async Task ChangeGroupName(Guid groupId, string name)
    {
        Group? group = await _context.Groups.FindAsync(groupId);
        if (group is null) return;

		group.Name = name;

		await _context.SaveChangesAsync();
	}

    public async Task RemoveLink(Guid id)
    {
        Link? link = await _context.Links.FindAsync(id);
        if (link is null) return;

		_context.Remove(link);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveGroup(Guid id)
    {
		Group? group = await _context.Groups.FindAsync(id);
		if (group is null) return;
        
		_context.Remove(group);
		
        await _context.SaveChangesAsync();
    }

    public async Task RemoveLinkFromFavorites(Guid id)
    {
        Link? link = await _context.Links.FindAsync(id);
        if(link is null) return;
        link.IsFavorite = false;
        await _context.SaveChangesAsync();
    }

    public async Task<JsonData> Export()
    {
        var links = await _context.Links.AsNoTracking().Select(x=>new JsonLink(x.Name, x.Url, x.IsFavorite, x.Parent.Id)).ToListAsync();
        var groups = await _context.Groups.AsNoTracking().Select(x => new JsonGroup(x.Id, x.Name, x.Parent != null ? x.Parent.Id : Guid.Empty)).ToListAsync();
        
        return new(){Id = Guid.NewGuid(), Links = links, Groups = groups};
    }

    public async Task Import(JsonData data)
    {
	    
    }
}