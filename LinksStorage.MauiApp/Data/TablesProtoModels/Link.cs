using SQLite;

namespace LinksStorage.Data.TablesProtoModels;
[Table("links")]
public class Link
{
	[PrimaryKey, AutoIncrement, Column("id")]
	public int Id { get; set; }

	[Indexed, Column("name")]
	public string Name { get; set; }

	[MaxLength(2050), Column("url")]
	public string Url { get; set; }

	[Indexed, Column("group_id")]
	public int GroupId { get; set; }

	[Indexed, Column("favorite")]
	public bool IsFavorite { get; set; }
}
