using LinksStorage.Data.TablesProtoModels;

namespace LinksStorage.Data;

public class JsonLink
{
	public JsonLink()
	{

	}

	public JsonLink(string name, string url, bool isFavorite)
	{
		Name       = name;
		Url        = url;
		IsFavorite = isFavorite;
	}

	public string Name { get; init; }
	public string Url { get; init; }
	public bool IsFavorite { get; init; }

	public Link Map(int parentId) => new() { Name = Name, Url = Url, IsFavorite = IsFavorite, GroupId = parentId };

	public override string ToString() => $"{(IsFavorite ? "*" : "")}  {Name}: {Url}";
}