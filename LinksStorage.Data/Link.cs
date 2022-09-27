using SQLite;

namespace LinksStorage.Data;

public class Link
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string Alias { get; set; }

    [MaxLength(2050)]
    public string Url { get; set; }

    [Indexed]
    public int GroupId { get; set; }
}