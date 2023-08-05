namespace LinksStorage.Shared;

public interface IShareDataHub
{
	Task<JsonData> Import(string clientId);
	Task Export(JsonData data, string clientId);
	Task<List<string>> ListClients();
	Task AskForShare(string clientId);
	Task ShareAnswer(string clientId, bool confirm);
}