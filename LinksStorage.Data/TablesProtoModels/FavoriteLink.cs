using SQLite;

namespace LinksStorage.Data.TablesProtoModels;

[Table("favorites")]
public class FavoriteLink
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int LinkId { get; set; }

    [Indexed]
    public int GroupId { get; set; }
}