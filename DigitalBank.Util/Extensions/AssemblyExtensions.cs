using System.Reflection;

namespace DigitalBank.Util.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetServiceImplementations(this Assembly assembly)
    {
        return GetServiceImplementations(assembly, new List<Type>());
    }

    public static IEnumerable<Type> GetServiceImplementations(this Assembly assembly, List<Type> ignoreTypes)
    {
        var implementations = assembly.GetExportedTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        !t.IsInterface &&
                        t.Name.EndsWith("Service") &&
                        !ignoreTypes.Contains(t));

        return implementations;
    }

    public static IEnumerable<Type> GetRepositoryImplementations(this Assembly assembly)
    {
        return GetRepositoryImplementations(assembly, new List<Type>());
    }

    public static IEnumerable<Type> GetRepositoryImplementations(this Assembly assembly, List<Type> ignoreTypes)
    {
        var implementations = assembly.GetExportedTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        !t.IsInterface &&
                        t.Name.EndsWith("Repository") &&
                        !ignoreTypes.Contains(t));

        return implementations;
    }
}
