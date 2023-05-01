using LinksStorage.Data.TablesProtoModels;

using Microsoft.EntityFrameworkCore;

namespace LinksStorage.Data;

public class DataContext : DbContext
{
    public static bool Initialized { get; protected set; }
    public static string File { get; protected set; }

	/// <summary>
	/// Constructor for creating migrations
	/// </summary>
	public DataContext()
    {
        File = Path.Combine("../", "UsedByMigratorOnly.db3");
		Initialize();
	}

	/// <summary>
	/// Constructor for mobile app
	/// </summary>
	/// <param name="filenameWithPath"></param>
	public DataContext(string filenameWithPath)
	{
		File = filenameWithPath;
		Initialize();
	}

	public DbSet<Group> Groups { get; init; }
    public DbSet<Link> Links { get; init; }

	void Initialize()
	{
		if (!Initialized)
		{
			Initialized = true;

			SQLitePCL.Batteries_V2.Init();

			Database.Migrate();
		}
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.UseSqlite($"Filename={File}");
	}

	public void Reload()
	{
		Database.CloseConnection();
		Database.OpenConnection();
	}
}
