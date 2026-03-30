#!/usr/bin/env bash
# =============================================================================
# new-module.sh
# Scaffolds a new backend module (Core and Infrastructure projects).
# =============================================================================
set -euo pipefail

MOD=$1
APP=${2:-Lex}

if [[ -z "$MOD" ]]; then
  echo "Usage: $0 <ModuleName> [AppName]"
  exit 1
fi

MOD_LOWER="${MOD,,}"
BASE="src/Modules/$APP.Module.$MOD"
CORE="$BASE/$APP.Module.$MOD.Core"
INFRA="$BASE/$APP.Module.$MOD.Infrastructure"

echo "Scaffolding backend module: $MOD"

mkdir -p "$CORE/Features" "$CORE/Domain" "$CORE/Abstractions"
mkdir -p "$INFRA/Persistence" "$INFRA/Consumers" "$INFRA/ExternalApis"

# Create Core .csproj
cat > "$CORE/$APP.Module.$MOD.Core.csproj" << CSPROJ
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\..\Core\Lex.SharedKernel\Lex.SharedKernel.csproj" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="MediatR"           Version="12.*" />
    <PackageReference Include="FluentValidation"  Version="11.*" />
  </ItemGroup>
</Project>
CSPROJ

# Create Infrastructure .csproj
cat > "$INFRA/$APP.Module.$MOD.Infrastructure.csproj" << CSPROJ
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\$APP.Module.$MOD.Core\$APP.Module.$MOD.Core.csproj" />
    <ProjectReference Include="..\..\..\Core\Lex.Infrastructure\Lex.Infrastructure.csproj" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore"          Version="8.*" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL"  Version="8.*" />
    <PackageReference Include="MassTransit.RabbitMQ"                   Version="8.*" />
    <PackageReference Include="Refit.HttpClientFactory"                Version="7.*" />
    <PackageReference Include="Microsoft.Extensions.Http.Resilience"   Version="8.*" />
  </ItemGroup>
</Project>
CSPROJ

# Permissions.cs
cat > "$CORE/${MOD}Permissions.cs" << CS
namespace Lex.Module.$MOD;
public static class ${MOD}Permissions
{
    private const string Prefix = "$MOD_LOWER";
    public const string View   = "${Prefix}.view";
    public const string Create = "${Prefix}.create";
    public const string Edit   = "${Prefix}.edit";
    public const string Delete = "${Prefix}.delete";
}
CS

# DbContext.cs
cat > "$INFRA/Persistence/${MOD}DbContext.cs" << CS
using Microsoft.EntityFrameworkCore;
namespace Lex.Module.$MOD.Persistence;
public sealed class ${MOD}DbContext : DbContext
{
    public ${MOD}DbContext(DbContextOptions<${MOD}DbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("$MOD_LOWER");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(${MOD}DbContext).Assembly);
    }
}
CS

# ServiceRegistration.cs
cat > "$INFRA/${MOD}ServiceRegistration.cs" << CS
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Lex.Module.$MOD;
public static class ${MOD}ServiceRegistration
{
    public static IServiceCollection Add${MOD}Module(
        this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not configured.");
        services.AddDbContext<Lex.Module.$MOD.Persistence.${MOD}DbContext>(o =>
            o.UseNpgsql(cs, b => b.MigrationsAssembly(typeof(${MOD}ServiceRegistration).Assembly.FullName)));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(${MOD}Permissions).Assembly));
        services.AddValidatorsFromAssembly(typeof(${MOD}Permissions).Assembly);
        return services;
    }
}
CS

# Add to solution
dotnet sln "$APP.sln" add "$CORE/$APP.Module.$MOD.Core.csproj"
dotnet sln "$APP.sln" add "$INFRA/$APP.Module.$MOD.Infrastructure.csproj"

echo "✓ Backend module $MOD scaffolded and added to $APP.sln"
