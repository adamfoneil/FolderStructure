using System.Reflection;

namespace Test
{
    internal static class Static
    {
        internal static string GetResource(string resourceName)
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource {resourceName} not found");
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
