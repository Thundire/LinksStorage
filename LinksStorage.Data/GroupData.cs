using System.Text;

namespace LinksStorage.Data;

public class GroupData
{
    public GroupData(IReadOnlyCollection<LinkInfoData> links, IReadOnlyCollection<GroupInfoData> groups)
    {
        Links = links;
        Groups = groups;
    }

    public IReadOnlyCollection<LinkInfoData> Links { get; }
    public IReadOnlyCollection<GroupInfoData> Groups { get; }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendJoin("\n", Groups);
        sb.AppendLine();
        sb.AppendJoin("\n", Links);
        return sb.ToString();
    }
}