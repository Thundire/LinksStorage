using Spark.Library.Database;

namespace LinksStorage.BlazorApp.Application.Models;

public class Tag : BaseModel
{
	public string Name { get; set; } = string.Empty;
	public List<Link> Links { get; set; } = new();
}