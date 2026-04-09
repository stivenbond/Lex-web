---
description: Guide for implementing a new module from scratch in the .NET 10 Modular Monolith platform.
---

# Implementing a Module from Scratch

This workflow guides you through creating a new domain module in the system based on the architectural guidelines defined in `docs/architecture.md`.

## General Architectural Rules
- **Modular Isolation**: Modules may never reference each other directly via code. Cross-module communication goes through MassTransit or domain events.
- **Vertical Slices**: Code within the module is organised by feature (Commands, Queries, Handlers, Validators), not technical layers.
- **Infrastructural Independence**: Core domain and application logic must never depend on infrastructure concerns.
- **No Shared Database Schemas**: Each module receives its own segregated schema in the common PostgreSQL database.
- **Explicit Registration**: The module registers its services explicitly via an `Add[ModuleName]Module` extension method.

## Step-by-Step Implementation

### Step 1: Create the Project Structure
Create two new projects for the module and structure them as follows:
- `Modules/YourApp.Module.[Name]/YourApp.Module.[Name].Core`
- `Modules/YourApp.Module.[Name]/YourApp.Module.[Name].Infrastructure`

### Step 2: Configure Project References
Set up references according to the strict architecture rules:
- `[Name].Core` references **only** `YourApp.SharedKernel`. (NO Infrastructure, NO other Modules, NO EF Core).
- `[Name].Infrastructure` references `[Name].Core`, `YourApp.SharedKernel`, and `YourApp.Infrastructure`.
- Update `YourApp.API` (Host) to reference `[Name].Infrastructure`.

### Step 3: Scaffold the Internal Layout
Inside the `Core` project, create the following folders:
- `Features/`: For Vertical Slice feature folders.
- `Domain/`: For module-specific Aggregate Roots, Entities, and Value Objects.
- `Abstractions/`: For interfaces like Repository contracts.

Inside the `Infrastructure` project:
- Create `[Name]DbContext` extending `DbContext`, configuring a default schema named after the module (e.g., `b.HasDefaultSchema("module_name")`).
- Create implementations for Repository contracts defined in `Core`.
- Create a configuration file containing the `Add[Name]Module(this IServiceCollection services)` extension method.

### Step 4: Register the Module in Host
In `YourApp.API` (Host), add a call to `builder.Services.Add[Name]Module()` in `Program.cs` to explicitly wire up the new module.

### Step 5: Define Module-Specific Domain and Events
- Create your Aggregate Roots in the `Domain` folder.
- If the module emits events for other modules, define the `IDomainEvent` contracts in `[Name].Core`. (Ensure events follow naming convention: `[EventName]Event` and are additive only).

### Step 6: Initial Migrations
- Generate the initial EF Core migration for the `[Name].Infrastructure` project. Ensure it creates tables exclusively in the module's dedicated schema.
