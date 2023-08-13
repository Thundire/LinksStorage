using LinksStorage.BlazorApp.Application.Database;
using LinksStorage.BlazorApp.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinksStorage.BlazorApp.Application.Services;

public class TagsRepository
{

	private readonly IDbContextFactory<DatabaseContext> _factory;

	public TagsRepository(IDbContextFactory<DatabaseContext> factory)
	{
		_factory = factory;
	}

	public async Task<List<Tag>> Query(Expression<Func<Tag, bool>>? query = null)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var set = context.Tags.AsNoTracking();
		if (query is not null) set = set.Where(query);
		return await set.ToListAsync();
	}

	public async Task<Tag?> Query(int id, Expression<Func<Tag, bool>>? query = null)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var set = context.Tags.AsNoTracking();
		if (query is not null) set = set.Where(query);
		return await set.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<Tag> Store(Tag newTag)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Tags.Add(newTag);
		await context.SaveChangesAsync();
		return newTag;
	}
	public async Task Destroy(Tag data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Attach(data);
		context.Remove(data);
		await context.SaveChangesAsync();
	}

}