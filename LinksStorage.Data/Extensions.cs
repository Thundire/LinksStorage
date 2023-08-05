using LinksStorage.Data.TablesProtoModels;
using LinksStorage.Shared;

namespace LinksStorage.Data;

public static class Extensions
{
	public static Link Map(this JsonLink self, int parentId) => new() { Name = self.Name, Url = self.Url, IsFavorite = self.IsFavorite };
}