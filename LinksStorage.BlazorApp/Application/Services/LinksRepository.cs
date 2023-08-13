using System.Linq.Expressions;
using LinksStorage.BlazorApp.Application.Database;
using LinksStorage.BlazorApp.Application.Models;

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;

using static System.Runtime.InteropServices.JavaScript.JSType;

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
		IQueryable<Link> set = context.Links.AsNoTracking();
		if (query is not null) set = set.Where(query);
		return await set.ToListAsync();
	}

	public async Task<Link?> Query(int id, Expression<Func<Link, bool>>? query = null)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var set = context.Links.AsNoTracking();
		if (query is not null) set = set.Where(query);
		return await set.FirstOrDefaultAsync(x=>x.Id == id);
	}

	public async Task<Link> StoreOrUpdate(Link data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var existedLink = await context.Links.FindAsync(data.Id);
		if (existedLink is null)
		{
			existedLink = new();
			data.Copy(existedLink, true);
			context.AttachRange(data.Tags);
			existedLink.Tags.AddRange(data.Tags);
			context.Update(existedLink);
		}
		else
		{
			data.Copy(existedLink, true);
			var leftOuterJoin =
				from outer in existedLink.Tags
				join inner in data.Tags on outer.Id equals inner.Id into temp
				from inner in temp.DefaultIfEmpty()
				select new
				{
					Outer = outer,
					Inner = inner,
					ToDelete = inner is null,
					ToIgnore = inner is not null,
					ToAdd = false,
				};
			var rightOuterJoin =
				from inner in data.Tags
				join outer in existedLink.Tags on inner.Id equals outer.Id into temp
				from outer in temp.DefaultIfEmpty()
				select new
				{
					Outer = outer,
					Inner = inner,
					ToDelete = false,
					ToIgnore = outer is not null,
					ToAdd = outer is null,
				};
			var join = leftOuterJoin.Union(rightOuterJoin).ToArray();
			foreach (var result in join)
			{
				switch (result.ToAdd, result.ToDelete)
				{
					case (true, false):
						context.Tags.Attach(result.Inner);
						existedLink.Tags.Add(result.Inner);
						break;
					case (false, true):
						existedLink.Tags.Remove(result.Outer);
						break;
				}
			}
				
		}

		await context.SaveChangesAsync();
		return data;
	}

	public async Task Destroy(Link data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Attach(data);
		context.Remove(data);
		await context.SaveChangesAsync();
	}
}