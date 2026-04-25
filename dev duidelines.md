# Lex Web Platform Development Guidelines

## Overview
Lex is a modular monolith educational platform built with .NET 10 backend and Next.js 15 frontend. This document provides guidelines for implementing features with proper authentication, authorization, and domain-driven design patterns.

## Architecture Principles
- **Modular Monolith**: Vertical slices per module, no cross-module code references
- **Domain-Driven Design**: Rich domain models, commands/queries/events
- **Event-Driven**: MassTransit for cross-module communication
- **On-Prem First**: No mandatory cloud dependencies
- **Explicit Configuration**: All registrations explicit, no magic conventions

## Feature Implementation Workflow

### 1. Domain Specification
- Define aggregate roots, entities, value objects
- Specify commands, queries, domain events
- Create feature specs in `module-spec.md` and individual `spec.md` files
- Define permissions in `ModulePermissions.cs`

### 2. Backend Implementation

#### Core Layer
```csharp
// Domain model
public class Assessment : AggregateRoot
{
    // Invariants and business logic
}

// Command/Query
public record CreateAssessmentCommand(string Title, Guid SubjectId) : IRequest<Result<Guid>>;

// Handler
internal sealed class CreateAssessmentHandler : IRequestHandler<CreateAssessmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateAssessmentCommand request, CancellationToken ct)
    {
        // Domain logic here
        var assessment = Assessment.Create(request.Title, request.SubjectId);
        await repository.AddAsync(assessment, ct);
        await repository.SaveChangesAsync(ct);
        
        // Publish domain events
        await publisher.Publish(new AssessmentCreatedEvent(assessment.Id), ct);
        
        return assessment.Id;
    }
}
```

#### Infrastructure Layer
```csharp
// DbContext with schema isolation
public class ModuleDbContext : DbContext
{
    public DbSet<Entity> Entities => Set<Entity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("module_schema");
        // Entity configurations
    }
}

// Service registration
public static class ModuleServiceRegistration
{
    public static IServiceCollection AddModule(this IServiceCollection services, IConfiguration config)
    {
        // EF Core setup
        services.AddDbContext<ModuleDbContext>(o => 
            o.UseNpgsql(config.GetConnectionString("Default")));
        
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Permissions).Assembly));
        
        // Validators
        services.AddValidatorsFromAssembly(typeof(Permissions).Assembly);
        
        return services;
    }
}
```

#### API Layer
```csharp
// Controller with authorization
[ApiController]
[Route("api/module")]
[Authorize]
public class ModuleController : ControllerBase
{
    [HttpPost]
    [RequirePermission("module.create")]
    public async Task<IActionResult> Create(CreateCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess 
            ? CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value)
            : BadRequest(result.Error);
    }
}
```

### 3. Authentication & Authorization

#### Permission System
- Permissions defined as constants: `ModulePermissions.Create = "module.create"`
- ASP.NET Core policies map roles to permissions
- JWT claims contain user roles and tenant info

#### Current User Service
```csharp
public interface ICurrentUser
{
    Guid UserId { get; }
    string[] Roles { get; }
    Guid? TenantId { get; }
}

public class CurrentUser : ICurrentUser
{
    public Guid UserId => Guid.Parse(httpContext.User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException());
    public string[] Roles => httpContext.User.FindAll("role").Select(c => c.Value).ToArray();
    public Guid? TenantId => Guid.TryParse(httpContext.User.FindFirst("tenant")?.Value, out var id) ? id : null;
}
```

### 4. Frontend Implementation

#### API Client
```typescript
// lib/api/client.ts
export interface ApiFetchOptions extends Omit<RequestInit, 'body'> {
  params?: Record<string, string | number | boolean | undefined>;
  body?: unknown;
}

export async function apiFetch<T>(url: string, options: ApiFetchOptions = {}): Promise<T> {
  const { params, body, ...rest } = options;
  
  const token = tokenManager.accessToken;
  const headers = {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json',
    ...rest.headers
  };
  
  const queryString = params ? '?' + new URLSearchParams(params as any) : '';
  const response = await fetch(`${url}${queryString}`, {
    ...rest,
    headers,
    body: body ? JSON.stringify(body) : undefined
  });
  
  if (!response.ok) {
    throw new Error(`API Error: ${response.status}`);
  }
  
  return response.json();
}
```

#### State Management (Zustand)
```typescript
// lib/store/module.ts
interface ModuleState {
  items: Item[];
  loading: boolean;
  error: string | null;
}

export const useModuleStore = create<ModuleState & {
  fetchItems: () => Promise<void>;
  createItem: (data: CreateItemData) => Promise<void>;
}>((set, get) => ({
  items: [],
  loading: false,
  error: null,
  
  fetchItems: async () => {
    set({ loading: true, error: null });
    try {
      const items = await apiFetch<Item[]>('/api/module');
      set({ items, loading: false });
    } catch (error) {
      set({ error: error.message, loading: false });
    }
  },
  
  createItem: async (data) => {
    try {
      await apiFetch('/api/module', { method: 'POST', body: data });
      await get().fetchItems(); // Refresh
    } catch (error) {
      set({ error: error.message });
    }
  }
}));
```

#### Component Structure
```tsx
// components/module/ItemForm.tsx
'use client';

import { useModuleStore } from '@/lib/store/module';
import { Button } from '@/components/ui/button';

export function ItemForm() {
  const { createItem, loading, error } = useModuleStore();
  
  const handleSubmit = async (formData: FormData) => {
    await createItem(Object.fromEntries(formData));
  };
  
  return (
    <form action={handleSubmit}>
      {/* Form fields */}
      <Button type="submit" disabled={loading}>
        {loading ? 'Creating...' : 'Create'}
      </Button>
      {error && <p className="text-red-500">{error}</p>}
    </form>
  );
}
```

### 5. Testing Strategy

#### Unit Tests
- Domain logic in Core projects
- Handler logic with mocked dependencies
- Validator tests

#### Integration Tests
- Full request/response cycles
- Database operations
- Cross-module event handling

#### Architecture Tests
- Enforce project reference rules
- Prevent circular dependencies

### 6. Database Migrations
```bash
# Add migration
dotnet ef migrations add InitialCreate -p Lex.Module.Name.Infrastructure -s Lex.API -o Persistence/Migrations

# Update database
dotnet ef database update
```

### 7. Event-Driven Communication
```csharp
// Publishing events
await publisher.Publish(new DomainEvent(), ct);

// Consuming events (in another module)
public class EventConsumer : IConsumer<DomainEvent>
{
    public async Task Consume(ConsumeContext<DomainEvent> context)
    {
        // Handle cross-module side effects
    }
}
```

### 8. File Handling
- All file access through ObjectStorage module
- Permission checks before serving files
- Streaming endpoints for large files

### 9. Error Handling
- Result<T> pattern for domain operations
- Global exception handling middleware
- User-friendly error messages

### 10. Documentation
- Update specs for any changes
- ADR for architectural decisions
- API documentation with OpenAPI/Swagger