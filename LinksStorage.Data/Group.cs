using SQLite;

namespace LinksStorage.Data;

public class Group
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string Name { get; set; }
}