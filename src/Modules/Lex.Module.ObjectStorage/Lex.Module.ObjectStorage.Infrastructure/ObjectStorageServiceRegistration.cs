using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lex.Module.ObjectStorage.Core;

namespace Lex.Module.ObjectStorage;

public static class ObjectStorageServiceRegistration
{
    public static IServiceCollection AddObjectStorageModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not configured.");
        
        services.AddDbContext<Lex.Module.ObjectStorage.Persistence.ObjectStorageDbContext>(o =>
            o.UseNpgsql(cs, b => b.MigrationsAssembly(typeof(ObjectStorageServiceRegistration).Assembly.FullName)));

        services.AddScoped<Infrastructure.Providers.IStorageProvider, Infrastructure.Providers.PostgresStorageProvider>();
        services.AddScoped<Infrastructure.Providers.IStorageProvider, Infrastructure.Providers.GarageStorageProvider>();
        
        services.AddScoped<SharedKernel.Abstractions.IObjectStorageService, Infrastructure.ObjectStorageService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ObjectStoragePermissions).Assembly));
        
        services.AddValidatorsFromAssembly(typeof(ObjectStoragePermissions).Assembly);
        
        return services;
    }
}
