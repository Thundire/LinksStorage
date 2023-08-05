namespace LinksStorage.Data;

public class GroupInfoData
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public override string ToString() => Name;
}