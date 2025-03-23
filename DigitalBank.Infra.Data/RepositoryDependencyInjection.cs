using DigitalBank.Util.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DigitalBank.Infra.Data;

public static class RepositoryDependencyInjection
{
    public static void ConfigureRepositoryLayer(this IServiceCollection services)
    {
        var assemblies = Assembly.GetExecutingAssembly();
        if (assemblies != null)
        {
            var implementations = assemblies.GetRepositoryImplementations();

            foreach (var implementation in implementations)
            {
                var interfaces = implementation.GetInterfaces().Where(x => !x.IsAssignableFrom(implementation.BaseType));
                if (!interfaces.Any())
                    continue;
                services.AddScoped(interfaces.First(), implementation);
            }
        }
    }
}
