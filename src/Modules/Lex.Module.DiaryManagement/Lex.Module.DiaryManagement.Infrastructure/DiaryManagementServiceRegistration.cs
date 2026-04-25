using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lex.Module.DiaryManagement;
public static class DiaryManagementServiceRegistration
{
    public static IServiceCollection AddDiaryManagementModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not configured.");
        services.AddDbContext<Lex.Module.DiaryManagement.Persistence.DiaryDbContext>(o =>
            o.UseNpgsql(cs, b => b.MigrationsAssembly(typeof(DiaryManagementServiceRegistration).Assembly.FullName)));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DiaryManagementPermissions).Assembly));
        services.AddValidatorsFromAssembly(typeof(DiaryManagementPermissions).Assembly);
        services.AddScoped<Lex.Module.DiaryManagement.Core.Domain.IDiaryRepository>(sp => 
            sp.GetRequiredService<Lex.Module.DiaryManagement.Persistence.DiaryDbContext>());

        return services;
    }
}
