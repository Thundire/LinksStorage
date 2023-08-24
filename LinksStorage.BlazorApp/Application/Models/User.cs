using Spark.Library.Database;

namespace LinksStorage.BlazorApp.Application.Models;

public class User : BaseModel
{
	public string Name { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public string? RememberToken { get; set; }

	public DateTime? EmailVerifiedAt { get; set; }

	public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}