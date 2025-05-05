using System.Diagnostics;

namespace FolderStructure;

[DebuggerDisplay("{Name} - {Items.Length} items, {Folders.Length} subfolders")]
public class Folder<T>
{
	public required string Name { get; init; }
	public T[] Items { get; init; } = [];
	public Folder<T>[] Folders { get; init; } = [];

	public static Folder<T> From(IEnumerable<T> items, Func<T, string> pathAccessor, char separator = '/', string rootName = "root")
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
			Name = rootName,
			Items = BuildItems(0, allItems),
			Folders = BuildChildrenR(0, allItems)
		};

		return result;

		static T[] BuildItems(int depth, IEnumerable<ItemInfo> itemInfos) => [.. itemInfos.Where(io => io.Depth == depth).Select(io => io.Item)];

		static Folder<T>[] BuildChildrenR(int depth, IEnumerable<ItemInfo> itemInfos)
		{
			List<Folder<T>> results = [];

			var folders = itemInfos
				.Where(item => item.Depth > depth)
				.GroupBy(item => item.Segments[depth])
				.Select(grp =>
				{					
					return new Folder<T>()
					{
						Name = grp.Key,
						Folders = BuildChildrenR(depth + 1, grp),
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
