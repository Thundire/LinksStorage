namespace LinksStorage.Shared;

public interface IShareDataHub
{
	Task<List<JsonGroup>> Import(string clientId);
	Task Export(List<JsonGroup> groups, string clientId);
	List<string> ListClients();
	Task AskForShare(string clientId);
	Task ShareAnswer(string clientId, bool confirm);
}