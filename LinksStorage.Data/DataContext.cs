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

	public void Reload()
	{
		Database.CloseConnection();
		Database.OpenConnection();
	}

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

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Group>(entityBuilder =>
		{
			entityBuilder.ToTable("groups");

			entityBuilder.Property(e => e.Name).HasColumnName("name");
			entityBuilder.Property(e => e.Parent).HasColumnName("parent");
			entityBuilder.IndexerProperty<string>(nameof(Group.Name));

			entityBuilder.HasMany(e => e.Groups).WithOne(e => e.Parent);
			entityBuilder.HasMany(e => e.Links).WithOne(e => e.Parent);
		});

		modelBuilder.Entity<Link>(entityBuilder =>
		{
			entityBuilder.ToTable("links");

			entityBuilder.Property(e => e.Name).HasColumnName("name");
			entityBuilder.Property(e => e.Parent).HasColumnName("parent");
			entityBuilder.Property(e => e.Url).HasMaxLength(2050).HasColumnName("url");
			entityBuilder.Property(e => e.IsFavorite).HasColumnName("favorite");
			entityBuilder.IndexerProperty<string>(nameof(Link.Name));
			entityBuilder.IndexerProperty<bool>(nameof(Link.IsFavorite));
		});
	}
}
