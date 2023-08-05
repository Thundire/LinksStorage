namespace LinksStorage.Data;

public class LinkInfoData
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    
    public bool IsFavorite { get; init; }

    public override string ToString() => $"{Name}: {Url} {(IsFavorite ? "(*)" : string.Empty)}";

	public static LinkInfoData Empty { get; } = new();
}