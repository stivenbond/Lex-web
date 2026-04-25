---
name: scaffolding-workflow
description: "Guide for using make commands to scaffold new components in the Lex platform. Use when: needing to create new modules, features, integrations, ADRs, forms, or client modules via make targets."
---

# Scaffolding Workflow Agent

This agent provides guidance on using the Makefile commands for scaffolding new components in the Lex platform. Each command generates boilerplate code following the modular monolith architecture.

## Available Make Commands

### Backend Module Creation
**Command:** `make new-module MOD=ModuleName`  
**What it does:** Creates a complete backend module with Core and Infrastructure projects, including:
- `src/Modules/Lex.Module.ModuleName/Lex.Module.ModuleName.Core/` (domain logic, features, abstractions)
- `src/Modules/Lex.Module.ModuleName/Lex.Module.ModuleName.Infrastructure/` (persistence, consumers, external APIs)
- Project files (.csproj) with proper dependencies
- Permissions class, DbContext, service registration

**Generated files to fill out:**
- Domain entities in `Domain/`
- Feature handlers in `Features/{FeatureName}/` (command/query handlers, validators)
- Repository interfaces in `Abstractions/`
- Repository implementations in `Infrastructure/Persistence/`
- MassTransit consumers in `Infrastructure/Consumers/`
- External API clients in `Infrastructure/ExternalApis/`

**What the agent should do next:**
1. Implement domain entities and aggregates
2. Add feature commands/queries with handlers and validators
3. Configure EF Core mappings and migrations
4. Register the module in the host application
5. Add integration tests

### Vertical Slice Feature
**Command:** `make new-feature MOD=ModuleName FEAT=FeatureName TYPE=command|query|event`  
**What it does:** Creates a feature folder with MediatR command/query/event classes and handlers.

**Generated files to fill out:**
- `{FeatureName}Command.cs` / `{FeatureName}Query.cs` / `{FeatureName}Event.cs` (request classes)
- `{FeatureName}CommandHandler.cs` / `{FeatureName}QueryHandler.cs` (MediatR handlers)
- `{FeatureName}Validator.cs` (FluentValidation rules for commands)

**What the agent should do next:**
1. Implement business logic in the handler
2. Add domain events if needed
3. Update repository interfaces if new data access is required
4. Add unit tests for the handler

### External API Integration
**Command:** `make new-integration MOD=ModuleName SVC=ServiceName`  
**What it does:** Creates Refit-based external API integration interfaces.

**Generated files to fill out:**
- `I{ServiceName}Client.cs` (abstraction interface)
- `I{ServiceName}Api.cs` (Refit HTTP client interface)

**What the agent should do next:**
1. Define API endpoints and DTOs in the Refit interface
2. Implement the client interface in Infrastructure
3. Register the client in DI container
4. Add Polly policies for resilience
5. Create integration tests with test doubles

### Architecture Decision Records
**Command:** `make new-adr TITLE="Decision Title"` or `make new-adr-client TITLE="Decision Title"`  
**What it does:** Creates numbered ADR files in `docs/adr/` or `docs/adr/client/`.

**Generated files to fill out:**
- `ADR-XXX.md` with template sections: Status, Date, Context, Decision, Consequences

**What the agent should do next:**
1. Fill out the ADR template with the decision details
2. Update status to "Accepted" or "Rejected" as appropriate
3. Reference the ADR in relevant code comments

### Frontend Module
**Command:** `make new-client-module MOD=modulename`  
**What it does:** Creates a Next.js app router page in `client/app/(app)/modulename/`.

**Generated files to fill out:**
- `page.tsx` (basic React component with metadata)

**What the agent should do next:**
1. Implement the page UI components
2. Add API calls using the client-side API layer
3. Style with Tailwind CSS
4. Add form validation if needed
5. Create unit tests for components

### React Form Component
**Command:** `make new-form FORM=FormName MOD=ModuleName`  
**What it does:** Creates a form component using react-hook-form and zod.

**Generated files to fill out:**
- `{FormName}Form.tsx` (form component with schema and submit handler)

**What the agent should do next:**
1. Define the zod schema for form validation
2. Add form fields with proper input types
3. Implement form submission logic
4. Add error handling and loading states
5. Integrate with parent component

### Full Module Pair
**Command:** `make new-module-full MOD=ModuleName`  
**What it does:** Combines `new-module` and `new-client-module` for complete backend-frontend pair.

**Generated files:** All from both backend and frontend commands above.

**What the agent should do next:**
1. Implement backend features first
2. Create corresponding frontend components
3. Set up API endpoints in the host
4. Add end-to-end tests
5. Update documentation

## Development Workflow Integration

After scaffolding:
1. Run `make build` to ensure compilation
2. Add EF migrations with `make migrate MOD=ModuleName` if database changes
3. Run `make test` to validate
4. Update any relevant specs in `docs/specs/`
5. Consider creating integration tests in `tests/Lex.Module.{Name}.Tests/`

## Architecture Compliance

All generated code follows the vertical slice architecture:
- Features own their commands, handlers, and validation
- Infrastructure is separate from domain logic
- Modules are isolated with explicit boundaries
- Naming conventions match MassTransit and EF Core expectations