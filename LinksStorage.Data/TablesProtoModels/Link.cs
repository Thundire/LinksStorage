namespace LinksStorage.Data.TablesProtoModels;

public class Link : Entity
{
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsFavorite { get; set; }

    public Group Parent { get; set; }
}