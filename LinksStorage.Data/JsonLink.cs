using LinksStorage.Data.TablesProtoModels;

namespace LinksStorage.Data;

public class JsonLink
{
    public JsonLink()
    {

    }

    public JsonLink(string name, string url, bool isFavorite, int groupId)
    {
        Name = name;
        Url = url;
        IsFavorite = isFavorite;
        GroupId = groupId;
    }

    public string Name { get; init; }
    public string Url { get; init; }
    public bool IsFavorite { get; init; }
    public int GroupId { get; init; }

    public Link Map() => new() { Name = Name, Url = Url, GroupId = GroupId, IsFavorite = IsFavorite };
}