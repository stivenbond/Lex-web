using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Lex.Module.Scheduling.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lex.Module.Scheduling;
public static class SchedulingServiceRegistration
{
    public static IServiceCollection AddSchedulingModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not configured.");
        services.AddDbContext<Lex.Module.Scheduling.Persistence.SchedulingDbContext>(o =>
            o.UseNpgsql(cs, b => b.MigrationsAssembly(typeof(SchedulingServiceRegistration).Assembly.FullName)));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(SchedulingPermissions).Assembly));
        services.AddValidatorsFromAssembly(typeof(SchedulingPermissions).Assembly);
        services.AddScoped<ISchedulingRepository, SchedulingRepository>();
        services.AddScoped<ISchedulingEventPublisher, SchedulingEventPublisher>();

        services.AddAuthorizationBuilder()
            .AddPolicy(SchedulingPermissions.ViewSchedule, p => p.RequireAuthenticatedUser())
            .AddPolicy(SchedulingPermissions.ManageCalendar, p => p.RequireAuthenticatedUser())
            .AddPolicy(SchedulingPermissions.AssignSlots, p => p.RequireAuthenticatedUser());

        services.AddControllers()
            .AddApplicationPart(typeof(Lex.Module.Scheduling.Infrastructure.Controllers.SchedulingController).Assembly);

        return services;
    }
}
