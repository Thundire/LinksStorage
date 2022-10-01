namespace LinksStorage.Data;

public class GroupInfoData
{
    public int Id { get; init; }
    public string Name { get; init; }

    public override string ToString() => Name;
}