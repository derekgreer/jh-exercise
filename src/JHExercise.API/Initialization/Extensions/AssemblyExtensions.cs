using System.Reflection;

namespace JHExercise.API.Initialization.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Assembly> GetRelatedAssemblies(this Assembly assembly)
    {
        var assemblies = new List<Assembly> { assembly };

        var prefix = GetAssemblyPrefix(assembly);

        foreach (var a in assembly.GetReferencedAssemblies()
                     .Where(name => name.Name.StartsWith(prefix))
                     .Where(referencedAssembly =>
                         !assemblies.Select(a => a.FullName).Contains(referencedAssembly.FullName)))
            assemblies.AddRange(GetRelatedAssemblies(Assembly.Load(a)));

        return assemblies.DistinctBy(a => a.FullName);
    }

    static string GetAssemblyPrefix(Assembly assembly)
    {
        var name = assembly.GetName().Name;
        name = name.Substring(0, name.LastIndexOf(".", StringComparison.Ordinal));
        return name;
    }
}