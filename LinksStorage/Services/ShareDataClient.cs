using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using LinksStorage.Shared;

using Microsoft.AspNetCore.SignalR.Client;

namespace LinksStorage.Services;

public class ShareDataClient : IAsyncDisposable, IShareDataClient, IShareDataHub
{
	private readonly IMessenger _messenger;

	private bool _started;
	private bool _starting;
	private HubConnection _hub;

	public ShareDataClient(IMessenger messenger)
	{
		_messenger = messenger;

		messenger.Register<AnswerShare>(this, async (recipient, message) => await ShareAnswer(message.ClientId, message.Confirm));
		messenger.Register<ShareDataClient, ListClients>(this, (recipient, message) => message.Reply(recipient.ListClients()));
	}

	public bool IsConnected => _hub?.State == HubConnectionState.Connected;

	public async ValueTask Start(string hubPath)
	{
		if(_started || _starting) return;
		_starting = true;

		_hub = new HubConnectionBuilder()
			.WithUrl(hubPath)
			.WithAutomaticReconnect()
			.Build();

		await _hub.StartAsync();

		_started = true;
	}

	public async ValueTask DisposeAsync()
	{
		if(_hub is not null)
			await _hub.DisposeAsync();
	}

	public async Task<JsonData> Import(string clientId)
	{
		return await _hub.InvokeAsync<JsonData>(nameof(Import), clientId);
	}

	public async Task Export(JsonData data, string clientId)
	{
		await _hub.SendAsync(nameof(Export), data, clientId);
	}

	public async Task<List<string>> ListClients()
	{
		return await _hub.InvokeAsync<List<string>>(nameof(ListClients));
	}

	public async Task AskForShare(string clientId)
	{
		await _hub.SendAsync(nameof(AskForShare), clientId);
	}

	public async Task ShareAnswer(string clientId, bool confirm)
	{
		await _hub.SendAsync(nameof(ShareAnswer), clientId, confirm);
	}

	public Task Importing(JsonData data)
	{
		_messenger.Send(new Import(data));
		return Task.CompletedTask;
	}

	public async Task<JsonData> Exporting()
	{
		var data = await _messenger.Send<PrepareExportingData>();
		return data;
	}

	public async Task ShareAnswering(string clientId, bool confirm)
	{
		if (!confirm) return;

		await _messenger.Send(new ExportToShareServer { ClientId = clientId });
	}
}

public class ListClients : AsyncRequestMessage<List<string>>{}