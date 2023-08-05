namespace LinksStorage.Data.TablesProtoModels;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
}
