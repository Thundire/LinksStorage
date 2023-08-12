using System.Linq.Expressions;
using LinksStorage.BlazorApp.Application.Database;
using LinksStorage.BlazorApp.Application.Models;

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;

namespace LinksStorage.BlazorApp.Application.Services;

public class LinksRepository
{
	private readonly IDbContextFactory<DatabaseContext> _factory;

	public LinksRepository(IDbContextFactory<DatabaseContext> factory)
	{
		_factory = factory;
	}

	public async Task<List<Link>> Query(Expression<Func<Link, bool>>? query = null)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var set = context.Links.AsQueryable().Include(x=>x.Tags).AsNoTracking();
		if (query is not null) set = set.Where(query);
		return await set.ToListAsync();
	}

	public async Task<Link?> Query(int id, Expression<Func<Link, bool>>? query = null)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var set = context.Links.AsQueryable().AsNoTracking();
		if (query is not null) set = set.Where(query);
		return await set.FirstOrDefaultAsync(x=>x.Id == id);
	}

	public async Task Store(Link newLink)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Links.Update(newLink);
		await context.SaveChangesAsync();
	}

	public async Task Update(Link data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Update(data);
		await context.SaveChangesAsync();
	}
	public async Task Destroy(Link data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Attach(data);
		context.Remove(data);
		await context.SaveChangesAsync();
	}
}