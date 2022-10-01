namespace LinksStorage.Data;

public class LinkInfoData
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Url { get; init; }

    public override string ToString() => $"{Name}: {Url}";
}