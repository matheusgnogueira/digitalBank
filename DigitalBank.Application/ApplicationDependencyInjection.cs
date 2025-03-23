using DigitalBank.Util.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DigitalBank.Application;

public static class ApplicationDependencyInjection
{
    public static void ConfigureApplicationLayer(this IServiceCollection services)
    {
        var assemblies = Assembly.GetExecutingAssembly();
        if (assemblies != null)
        {
            var implementations = assemblies.GetServiceImplementations();

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
