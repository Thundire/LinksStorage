using LinksStorage.Shared;
using Microsoft.AspNetCore.SignalR;

namespace LinksStorage.ShareServer;

public class ShareDataHub : Hub<IShareDataClient>, IShareDataHub
{
	private readonly Cache _cache;

	public ShareDataHub(Cache cache) => _cache = cache;

	public async Task<List<JsonGroup>> Import(string clientId)
	{
		IShareDataClient client = Clients.Client(clientId);
		var data = await client.Exporting();
		return data;
	}

	public async Task Export(List<JsonGroup> groups, string clientId)
	{
		IShareDataClient client = Clients.Client(clientId);
		await client.Importing(groups);
	}

	public async Task AskForShare(string clientId)
	{
		IShareDataClient client = Clients.Client(clientId);
		await client.AskForShare(Context.ConnectionId);
	}

	public async Task ShareAnswer(string clientId, bool confirm)
	{
		IShareDataClient client = Clients.Client(clientId);
		await client.ShareAnswering(Context.ConnectionId, confirm);
	}
	 
	public override Task OnConnectedAsync()
	{
		_cache.Add(Context.ConnectionId);
		return Task.CompletedTask;
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		_cache.Remove(Context.ConnectionId);
		return Task.CompletedTask;
	}

	public Task<List<string>> ListClients()
	{
		return Task.FromResult(_cache.List(Context.ConnectionId));
	}
}