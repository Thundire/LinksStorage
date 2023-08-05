namespace LinksStorage.Data;

public class TagCategory
{
	public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Tag> Tags { get; set; } = new();
}