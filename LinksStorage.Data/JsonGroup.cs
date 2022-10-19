using LinksStorage.Data.TablesProtoModels;

namespace LinksStorage.Data;

public class JsonGroup
{
    public JsonGroup()
    {

    }

    public JsonGroup(string name, int parentGroupId)
    {
        Name = name;
        ParentGroupId = parentGroupId;
    }

    public string Name { get; init; }
    public int ParentGroupId { get; init; }

    public Group Map() => new() { GroupId = ParentGroupId, Name = Name };

    public override string ToString() => $"{Name}, {ParentGroupId}";
}