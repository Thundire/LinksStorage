using LinksStorage.BlazorApp.Application.Models;

namespace LinksStorage.BlazorApp.Application.Services;

public static class ModelsExtensions
{
	public static void Copy(this Link source, Link destination, bool ignoreCollections = false)
	{
		destination.Id        = source.Id;
		destination.Name      = source.Name;
		destination.Favorite  = source.Favorite;
		destination.Uri       = source.Uri;
		destination.User      = source.User;
		destination.UserId    = source.UserId;
		destination.CreatedAt = source.CreatedAt;
		destination.UpdatedAt = source.UpdatedAt;
		if (!ignoreCollections)
		{
			destination.Tags.Clear();
			destination.Tags.AddRange(source.Tags);
		}
	}
}
