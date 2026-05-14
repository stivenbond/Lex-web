using Microsoft.EntityFrameworkCore;
using Lex.Infrastructure.Persistence;
using Lex.Module.Notifications.Core.Domain;

namespace Lex.Module.Notifications.Persistence;

public sealed class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : BaseDbContext<NotificationsDbContext>(options), INotificationRepository
{
    public DbSet<Notification> Notifications => Set<Notification>();

    public void AddNotification(Notification notification) => Notifications.Add(notification);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("notifications");

        modelBuilder.Entity<Notification>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.IsRead).HasDefaultValue(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}
