using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace Lex.Module.Scheduling.Tests.TestInfrastructure;

/// <summary>
/// Shared testcontainers fixture that can be reused by module test projects.
/// It centralizes the infra stack shape (Postgres + RabbitMQ) so app-wide
/// integration tests can adopt the same setup with minimal duplication.
/// </summary>
public sealed class AppContainerFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _postgres;
    private RabbitMqContainer? _rabbitMq;

    public bool IsAvailable { get; private set; }
    public string? UnavailableReason { get; private set; }

    public string PostgresConnectionString =>
        _postgres?.GetConnectionString()
        ?? throw new InvalidOperationException("Postgres container is not available.");

    public string RabbitMqConnectionString =>
        _rabbitMq?.GetConnectionString()
        ?? throw new InvalidOperationException("RabbitMQ container is not available.");

    public async Task InitializeAsync()
    {
        try
        {
            _postgres = new PostgreSqlBuilder()
                .WithImage("postgres:16-alpine")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build();

            _rabbitMq = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management-alpine")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                .Build();

            await _postgres.StartAsync();
            await _rabbitMq.StartAsync();
            IsAvailable = true;
        }
        catch (Exception ex)
        {
            IsAvailable = false;
            UnavailableReason = ex.Message;
        }
    }

    public async Task DisposeAsync()
    {
        if (_rabbitMq is not null)
        {
            await _rabbitMq.DisposeAsync();
        }

        if (_postgres is not null)
        {
            await _postgres.DisposeAsync();
        }
    }

    public Lex.Module.Scheduling.Persistence.SchedulingDbContext CreateSchedulingDbContext()
    {
        var options = new DbContextOptionsBuilder<Lex.Module.Scheduling.Persistence.SchedulingDbContext>()
            .UseNpgsql(PostgresConnectionString)
            .Options;

        return new Lex.Module.Scheduling.Persistence.SchedulingDbContext(options);
    }
}
