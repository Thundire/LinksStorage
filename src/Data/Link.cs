namespace LinksStorage.Data;

public class Link
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public bool Favorite { get; set; }
}