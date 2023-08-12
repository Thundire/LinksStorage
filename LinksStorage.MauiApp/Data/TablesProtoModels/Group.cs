using SQLite;

namespace LinksStorage.Data.TablesProtoModels;
[Table("groups")]
public class Group
{
	[PrimaryKey, AutoIncrement, Column("id")]
	public int Id { get; set; }

	[Indexed, Column("name")]
	public string Name { get; set; }

	[Column("group_id")]
	public int GroupId { get; set; }
}
