using Microsoft.EntityFrameworkCore;
using Lex.Infrastructure.Persistence;
using Lex.Module.AssessmentDelivery.Core.Domain;

namespace Lex.Module.AssessmentDelivery.Persistence;

public sealed class AssessmentDeliveryDbContext(DbContextOptions<AssessmentDeliveryDbContext> options) 
    : BaseDbContext<AssessmentDeliveryDbContext>(options), IDeliveryRepository
{
    public DbSet<AssessmentSession> Sessions => Set<AssessmentSession>();

    public void AddSession(AssessmentSession session) => Sessions.Add(session);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("delivery");

        modelBuilder.Entity<AssessmentSession>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StudentId).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Status).HasConversion<string>();
            
            builder.OwnsMany(x => x.Answers, a =>
            {
                a.WithOwner().HasForeignKey("SessionId");
                a.Property<Guid>("Id");
                a.HasKey("Id");
                a.Property(x => x.QuestionId).IsRequired();
                a.Property(x => x.Content).IsRequired();
                a.ToTable("answers");
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}
