namespace LinksStorage.Data;

public class Tag
{
	public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<TagCategory> Categories { get; set; } = new();
    public List<Link> Links { get; set; } = new();
}
