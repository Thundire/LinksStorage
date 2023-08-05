namespace LinksStorage.Shared;

public interface IShareDataClient
{
	Task Importing(JsonData data);
	Task<JsonData> Exporting();
	Task AskForShare(string clientId);
	Task ShareAnswering(string clientId, bool confirm);
}