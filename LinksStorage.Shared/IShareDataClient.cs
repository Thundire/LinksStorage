namespace LinksStorage.Shared;

public interface IShareDataClient
{
	Task Importing(List<JsonGroup> groups);
	Task<List<JsonGroup>> Exporting();
	Task AskForShare(string clientId);
	Task ShareAnswering(string clientId, bool confirm);
}