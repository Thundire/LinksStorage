using Spark.Library.Database;

namespace LinksStorage.BlazorApp.Application.Models;

public class Link : BaseModel
{
	public string Name { get; set; } = string.Empty;
	public string Uri { get; set; } = string.Empty;
	public bool Favorite { get; set; }
	public List<Tag> Tags { get; set; } = new();
	public int UserId { get; set; }
	public User? User { get; set; }
}