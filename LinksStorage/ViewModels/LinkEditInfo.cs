namespace LinksStorage.ViewModels;

public record LinkEditInfo(Guid Group, Guid Id, string Name, string Url)
{
    public bool IsFromRoot => Group == Guid.Empty;
}