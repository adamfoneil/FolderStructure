namespace FolderStructure
{
    public class Folder<T>
    {
        /// <summary>
        /// if not null, then it's a leaf item
        /// </summary>
        public T? Data { get; init; }
        /// <summary>
        /// folder name between separators
        /// </summary>
        public string Name { get; init; } = default!;
        public IEnumerable<Folder<T>> Children { get; init; } = Enumerable.Empty<Folder<T>>();

        public static Folder<T> From(IEnumerable<T> items, Func<T, string> pathAccessor, char separator = '/', string rootName = "root")
        {
            var allPaths = items.Select(i =>
            {
                var folders = pathAccessor.Invoke(i).Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                return new FolderInfo()
                {
                    Folders = folders,
                    Count = folders.Length,
                    Data = i
                };
            });

            var result = new Folder<T>()
            {
                Name = rootName,
                Children = BuildChildrenR(0, allPaths)
            };

            return result;

            IEnumerable<Folder<T>> BuildChildrenR(int level, IEnumerable<FolderInfo> folderInfos)
            {
                List<Folder<T>> results = new();

                var folders = folderInfos
                    .Where(item => item.Count > level)
                    .GroupBy(item => item.Folders[level])
                    .Select(grp =>
                    {
                        var data = (level == (grp.First().Count - 1)) ? grp.First().Data : default;

                        return new Folder<T>()
                        {
                            Name = grp.Key,
                            Data = data,
                            Children = BuildChildrenR(level + 1, grp)
                        };
                    });

                results.AddRange(folders);

                return results;
            }
        }

        private class FolderInfo
        {
            public string[] Folders { get; init; } = Enumerable.Empty<string>().ToArray();
            public int Count { get; init; }
            public T? Data { get; init; }
        }
    }
}
