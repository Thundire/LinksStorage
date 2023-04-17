namespace LinksStorage.Shared;

public interface IShareDataHub
{
	Task<List<JsonGroup>> Export();
	Task Import(List<JsonGroup> groups);
}