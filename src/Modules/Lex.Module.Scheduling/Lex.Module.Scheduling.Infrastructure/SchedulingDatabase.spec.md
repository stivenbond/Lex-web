# Infrastructure Specification: Scheduling Database

## 1. Overview
- **Category**: DB Repository implementation
- **Parent Module**: Scheduling
- **Component Name**: SchedulingDbContext

## 2. Technical Strategy
- **Base Technology**: EF Core 10, Npgsql.
- **Storage/Transport**: PostgreSQL.

## 3. Implementation Details
The module uses a dedicated slice of the database under the `scheduling` schema.

- **Interface Realized**: `ISchedulingDbContext`
- **Tables**:
    - `scheduling.academic_years`: Metadata for years.
    - `scheduling.terms`: Child table for academic year subdivisions.
    - `scheduling.periods`: Master list of daily time blocks.
    - `scheduling.slots`: Intersection of time, classroom, and date.
    - `scheduling.class_allocations`: Assignments of Class/Teacher to Slots.

## 4. Configuration & Settings
- **Settings Class**: `SchedulingDbSettings`
- **Key in `appsettings.json`**: `ConnectionStrings:DefaultConnection` (shared).
- **Default Schema**: `scheduling`.

## 5. Resilience & Performance
- **Indexes**:
    - Composite index on `slots(Date, PeriodId, ClassroomId)` for conflict detection.
    - Index on `class_allocations(TeacherId, Date)` for schedule lookups.
- **Transactions**: Commands must use `IDbContextTransaction` to ensure atomicity when modifying slots and allocations together.

## 6. Integration Points
- **Health Checks**: Standard EF Core DbContext health check.

## 7. Migration & Deployment
- **Database Migrations**: Managed via `Lex.Module.Scheduling.Infrastructure` project.
- **Auto-Migration**: Part of the `MigrationService` run on startup.
