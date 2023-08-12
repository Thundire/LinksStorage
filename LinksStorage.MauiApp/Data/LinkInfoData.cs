using SQLite;

namespace LinksStorage.Data;

public class LinkInfoData
{
	public int Id { get; init; }
	public string Name { get; init; }
	public string Url { get; init; }
	[Column("favorite")]
	public bool IsFavorite { get; init; }

	public override string ToString() => $"{Name}: {Url} {(IsFavorite ? "(*)" : string.Empty)}";
}