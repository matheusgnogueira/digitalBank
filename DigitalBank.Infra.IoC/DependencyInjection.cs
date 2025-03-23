using DigitalBank.Application;
using DigitalBank.Infra.Data;
using DigitalBank.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalBank.Infra.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.ConfigureRepositoryLayer();
        services.ConfigureApplicationLayer();

        return services;
    }
}
