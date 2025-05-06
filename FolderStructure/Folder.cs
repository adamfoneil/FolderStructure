using System.Diagnostics;

namespace FolderStructure;

[DebuggerDisplay("{Name} - {Items.Length} items, {Subfolders.Length} subfolders")]
public class Folder<T>
{
	public required string Name { get; init; }
	public required string Path { get; init; }
	public required bool IsRoot { get; init; }
	public T[] Items { get; init; } = [];
	public Folder<T>[] Subfolders { get; init; } = [];

	/// <summary>
	/// scans an IEnumerable of items to build a tree structure from a given path accessor and separator character
	/// </summary>
	public static Folder<T> From(IEnumerable<T> items, Func<T, string> pathAccessor, char separator = '/')
	{
		var allItems = items.Select(item =>
		{
			var folders = pathAccessor.Invoke(item).Split([separator], StringSplitOptions.RemoveEmptyEntries).ToArray();
			return new ItemInfo()
			{
				Segments = folders,
				Depth = folders.Length,
				Item = item
			};
		}).ToArray();

		var result = new Folder<T>()
		{
			Name = separator.ToString(),
			Path = separator.ToString(),
			IsRoot = true,
			Items = BuildItems(0, allItems),
			Subfolders = BuildChildrenR(0, allItems, separator, string.Empty)
		};

		return result;

		static T[] BuildItems(int depth, IEnumerable<ItemInfo> itemInfos) => [.. itemInfos.Where(io => io.Depth == depth).Select(io => io.Item)];

		static Folder<T>[] BuildChildrenR(int depth, IEnumerable<ItemInfo> itemInfos, char separator, string parentPath)
		{
			List<Folder<T>> results = [];

			var folders = itemInfos
				.Where(item => item.Depth > depth)
				.GroupBy(item => item.Segments[depth])
				.Select(grp =>
				{
					var newParentPath = $"{parentPath}{separator}{grp.Key}";
					return new Folder<T>()
					{
						IsRoot = false,
						Name = grp.Key,
						Path = newParentPath,
						Subfolders = BuildChildrenR(depth + 1, grp, separator, newParentPath),
						Items = BuildItems(depth + 1, grp),
					};
				});
			results.AddRange(folders);
			return [.. results];
		}
	}

	private class ItemInfo
	{
		public string[] Segments { get; init; } = [];
		public int Depth { get; init; }
		public required T Item { get; init; }
	}
}
