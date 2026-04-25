using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Xunit;
using Respawn;
using Lex.Infrastructure.Persistence;

namespace Lex.IntegrationTests;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    private readonly RabbitMqContainer _mqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management-alpine")
        .Build();

    protected HttpClient Client { get; private set; } = null!;
    protected IServiceProvider ServiceProvider { get; private set; } = null!;
    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _mqContainer.StartAsync();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Override connection string for all DbContexts
                    // In a real modular monolith, we use the same connection string for simplicity in testing
                    services.RemoveAll(typeof(DbContextOptions));
                    
                    var connectionString = _dbContainer.GetConnectionString();
                    
                    // Here we would ideally re-register all module DbContexts. 
                    // For this MVP, we'll assume they all use the same 'Default' connection string from IConfiguration.
                    // We can also use a custom IConfiguration here.
                });
            });

        Client = factory.CreateClient();
        ServiceProvider = factory.Services;
        
        // Setup Respawn
        _respawner = await Respawner.CreateAsync(_dbContainer.GetConnectionString(), new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["object_storage", "file_processing", "google", "assessment", "diary", "lessons", "scheduling", "delivery", "notifications", "reporting"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbContainer.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _mqContainer.StopAsync();
    }
}
