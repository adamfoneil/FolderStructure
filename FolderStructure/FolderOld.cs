namespace FolderStructure
{
    public class FolderOld<T>
    {
        /// <summary>
        /// if not null, then it's a leaf item
        /// </summary>
        public T? Data { get; init; }
        /// <summary>
        /// folder name between separators
        /// </summary>
        public string Name { get; init; } = default!;
        public IEnumerable<FolderOld<T>> Children { get; init; } = Enumerable.Empty<FolderOld<T>>();

        public static FolderOld<T> From(IEnumerable<T> items, Func<T, string> pathAccessor, char separator = '/', string rootName = "root")
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

            var result = new FolderOld<T>()
            {
                Name = rootName,
                Children = BuildChildrenR(0, allPaths)
            };

            return result;

            IEnumerable<FolderOld<T>> BuildChildrenR(int level, IEnumerable<FolderInfo> folderInfos)
            {
                List<FolderOld<T>> results = new();

                var folders = folderInfos
                    .Where(item => item.Count > level)
                    .GroupBy(item => item.Folders[level])
                    .Select(grp =>
                    {
                        var data = (level == (grp.First().Count - 1)) ? grp.First().Data : default;

                        return new FolderOld<T>()
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
