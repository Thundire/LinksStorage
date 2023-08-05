using System.Collections.Immutable;
using System.Text;

namespace LinksStorage.Data;

public class GroupData
{
    public GroupData(IEnumerable<LinkInfoData> links, IEnumerable<GroupInfoData> groups)
    {
        Links = links.ToImmutableList();
        Groups = groups.ToImmutableList();
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

	public static GroupData Empty { get; } = new (Array.Empty<LinkInfoData>(), Array.Empty<GroupInfoData>());
}