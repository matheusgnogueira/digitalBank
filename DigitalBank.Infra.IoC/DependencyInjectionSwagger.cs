using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DigitalBank.Infra.Ioc;

public static class DependencyInjectionSwagger
{
    public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DigitalBank API",
                Version = "v1",
                Description = "Sistema de Caixa de Banco"
            });
        });

        return services;
    }
}
