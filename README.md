This is reboot of my [FolderBuilder](https://github.com/adamfoneil/FolderBuilder) project to modernize and simplify some things.

The point of this is to take any `IEnumerable<T>` and convert it into a recursive folder structure based on some kind of path expression and separator character. I'm using it as part of [KnowledgeBase.RCL](https://github.com/adamfoneil/KnowledgeBase.RCL) to present a list of routes as a tree view, grouped by folder.

Install via NuGet package from my [personal NuGet feed](https://aosoftware.blob.core.windows.net/packages/index.json): `AO.FolderStructure`

There's a single public object: [Folder](https://github.com/adamfoneil/FolderStructure/blob/master/FolderStructure/Folder.cs). You initialize it from any `IEnumerable<T>` like this, taken from the [test](https://github.com/adamfoneil/FolderStructure/blob/master/Test/SampleStructures.cs#L10). For example, here I have an array of [NavEntry](https://github.com/adamfoneil/FolderStructure/blob/master/Test/SampleStructures.cs#L32) objects, and I'm converting it to a folder structure by `Route`:

```csharp
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
```
When serialized as json, it looks like [this](https://github.com/adamfoneil/FolderStructure/blob/master/Test/Resources/folders.json). The json is a bit verbose, so I decided not to show it here.
