namespace LinksStorage.Shared;

public class JsonGroup
{
	public JsonGroup(Guid id, string name, Guid parentId)
	{
		Id = id;
		Name = name;
		ParentId = parentId;
	}

	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public Guid ParentId { get; init; }
}