namespace LinksStorage.Data;

public class JsonData
{
    public JsonData(List<JsonLink> links, List<JsonGroup> groups)
    {
        Groups = groups;
        Links = links;
    }

    public List<JsonGroup> Groups { get; init; }
    public List<JsonLink> Links { get; init; }

    public override string ToString() => string.Join("\n", Groups) + "\n" + string.Join("\n", Links);
}