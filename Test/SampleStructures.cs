using FolderStructure;
using System.Text.Json;

namespace Test
{
    [TestClass]
    public class SampleStructures
    {
        [TestMethod]
        public void BuildNavStructure()
        {
            var routes = new NavEntry[]
            {
                new NavEntry() { Route = "Welcome", Href = "/help/kb/welcome.html" },
                new NavEntry() { Route = "Setup/This", Href = "/help/kb/setup/this.html" },
                new NavEntry() { Route = "Setup/That", Href = "/help/kb/setup/that.html" },
                new NavEntry() { Route = "Setup/Other", Href = "/help/kb/setup/other.html" },
                new NavEntry() { Route = "Ops/Thing1", Href = "/help/kb/ops/thing1.html" },
                new NavEntry() { Route = "Ops/Thing2", Href = "/help/kb/ops/thing2.html" },
            };

            var folder = Folder<NavEntry>.From(routes, r => r.Route);

            var json = JsonSerializer.Serialize(folder, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            Assert.IsTrue(json.Equals(Static.GetResource("Test.Resources.folders.json")));
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
}
