using SQLite;

namespace LinksStorage.Data;

public class FavoriteLink
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string LinkId { get; set; }
}