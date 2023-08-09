using Spark.Library.Database;

namespace LinksStorage.BlazorApp.Application.Models;

public class Tag : BaseModel
{
	public string Name { get; set; } = string.Empty;
	public List<TagCategory> Categories { get; set; } = new();
	public List<Link> Links { get; set; } = new();
	public int UserId { get; set; }
	public User? User { get; set; }
}