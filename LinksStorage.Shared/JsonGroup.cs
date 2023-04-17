namespace LinksStorage.Shared;

public class JsonGroup
{
    public JsonGroup(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public List<JsonGroup> Groups { get; set; }
    public List<JsonLink> Links { get; set; }

    public override string ToString() => $"{Name}";
}