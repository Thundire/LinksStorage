namespace LinksStorage.Shared;

public class JsonLink
{
    public JsonLink()
    {

    }

	public JsonLink(string name, string url, bool isFavorite, Guid groupId)
	{
		Name = name;
		Url = url;
		IsFavorite = isFavorite;
		GroupId = groupId;
	}

	public string Name { get; init; }
    public string Url { get; init; }
    public bool IsFavorite { get; init; }
    public Guid GroupId { get; init; }

    public override string ToString() => $"{(IsFavorite ? "*" : "")}  {Name}: {Url}";
}