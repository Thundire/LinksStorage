using Spark.Library.Database;

namespace LinksStorage.BlazorApp.Application.Models;

public class TagCategory : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public List<Tag> Tags { get; set; } = new();
}