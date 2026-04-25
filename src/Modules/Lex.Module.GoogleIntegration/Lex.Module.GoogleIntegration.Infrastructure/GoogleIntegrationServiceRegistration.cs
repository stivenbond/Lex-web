using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lex.Module.GoogleIntegration;
public static class GoogleIntegrationServiceRegistration
{
    public static IServiceCollection AddGoogleIntegrationModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not configured.");
        services.AddDbContext<Lex.Module.GoogleIntegration.Persistence.GoogleIntegrationDbContext>(o =>
            o.UseNpgsql(cs, b => b.MigrationsAssembly(typeof(GoogleIntegrationServiceRegistration).Assembly.FullName)));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GoogleIntegrationPermissions).Assembly));
        services.AddValidatorsFromAssembly(typeof(GoogleIntegrationPermissions).Assembly);
        
        services.AddScoped<Lex.Module.GoogleIntegration.Core.Abstractions.IGoogleDriveService, Lex.Module.GoogleIntegration.Infrastructure.Services.GoogleDriveService>();

        return services;
    }
}
