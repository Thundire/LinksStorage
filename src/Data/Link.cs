namespace LinksStorage.Data;

public class Link
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public bool Favorite { get; set; }
    public List<Tag> Tags { get; set; } = new();
}
