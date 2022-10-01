using SQLite;

namespace LinksStorage.Data.TablesProtoModels;

[Table("links")]
public class Link
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string Name { get; set; }

    [MaxLength(2050)]
    public string Url { get; set; }

    [Indexed]
    public int GroupId { get; set; }
}