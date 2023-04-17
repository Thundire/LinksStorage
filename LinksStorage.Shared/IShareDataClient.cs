namespace LinksStorage.Shared;

public interface IShareDataClient
{
	Task Importing(List<JsonGroup> groups);
	Task<List<JsonGroup>> Exporting();
}