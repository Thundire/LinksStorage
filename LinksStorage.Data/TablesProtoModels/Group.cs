namespace LinksStorage.Data.TablesProtoModels;

public class Group : Entity
{
    public string Name { get; set; } = string.Empty;

    public Group? Parent { get; set; }
    public ICollection<Link> Links { get; set; } = new HashSet<Link>();
    public ICollection<Group> Groups { get; set; } = new HashSet<Group>();
}