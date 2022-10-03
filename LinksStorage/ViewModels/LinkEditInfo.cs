namespace LinksStorage.ViewModels;

public record LinkEditInfo(int Group, int Id, string Name, string Url)
{
    public bool IsFromRoot => Group == 1;
}