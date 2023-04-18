namespace LinksStorage.ShareServer;

public class Cache
{
	private HashSet<string> Connections { get; } = new();

	public void Add(string id)
	{
		Connections.Add(id);
	}

	public void Remove(string id)
	{
		Connections.Remove(id);
	}

	public List<string> List(string asked)
	{
		var connections = Connections.Except(new[] { asked });
		return connections.ToList();
	}
}