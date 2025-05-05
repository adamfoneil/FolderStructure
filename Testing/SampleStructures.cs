using FolderStructure;
using System.Text.Json;

namespace Test;

[TestClass]
public class SampleStructures
{
    private static NavEntry[] Routes =>
    [
        new() { Route = "Welcome", Href = "/help/kb/welcome.html" },
        new() { Route = "Setup/This", Href = "/help/kb/setup/this.html" },
        new() { Route = "Setup/That", Href = "/help/kb/setup/that.html" },
        new() { Route = "Setup/Other", Href = "/help/kb/setup/other.html" },
        new() { Route = "Ops/Thing1", Href = "/help/kb/ops/welcome.html" },
		new() { Route = "Ops/Thing1", Href = "/help/kb/ops/detail.html" },
		new() { Route = "Ops/Thing2", Href = "/help/kb/ops/index.html" },
		new() { Route = "Ops/Thing2", Href = "/help/kb/ops/detail.html" }
	];

    [TestMethod]
    public void BuildOldNavStructure()
    {
        var folder = FolderOld<NavEntry>.From(Routes, r => r.Route);

        var json = JsonSerializer.Serialize(folder, new JsonSerializerOptions()
        {
            WriteIndented = true
        });

        Assert.IsTrue(json.Equals(Static.GetResource("Test.Resources.folders.json")));
    }

    [TestMethod]
    public void BuildNavStructure()
    {
        var folder = Folder<NavEntry>.From(Routes, r => r.Route);
    }

    public class NavEntry
    {
        /// <summary>
        /// builds tree view in reader UI,
        /// last part is the title
        /// </summary>
        public string Route { get; set; } = default!;
        /// <summary>
        /// link to the physical file
        /// </summary>
        public string Href { get; set; } = default!;
    }
}