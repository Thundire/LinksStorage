namespace LinksStorage.Shared;

public class JsonData
{
	public Guid Id { get; init; }
	public List<JsonGroup> Groups { get; init; } = null!;
    public List<JsonLink> Links { get; init; } = null!;
}
