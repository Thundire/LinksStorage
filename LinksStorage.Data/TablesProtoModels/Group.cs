using SQLite;

namespace LinksStorage.Data.TablesProtoModels;

[Table("groups")]
public class Group
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string Name { get; set; }

    public int GroupId { get; set; }
}