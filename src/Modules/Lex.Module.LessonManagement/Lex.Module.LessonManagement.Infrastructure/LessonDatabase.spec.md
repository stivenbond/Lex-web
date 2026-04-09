# Infrastructure Specification: Lesson Database

## 1. Overview
- **Category**: DB Repository implementation
- **Parent Module**: LessonManagement
- **Component Name**: LessonDbContext

## 2. Technical Strategy
- **Base Technology**: EF Core 10, Npgsql.
- **Storage/Transport**: PostgreSQL.

## 3. Implementation Details
The module uses a dedicated slice of the database under the `lesson` schema.

- **Interface Realized**: `ILessonDbContext`
- **Tables**:
    - `lesson.plans`: Main table for lesson plans.
    - `lesson.resources`: Supporting materials linked to plans.

## 4. Configuration & Settings
- **Settings Class**: `LessonDbSettings`
- **Key in `appsettings.json`**: `ConnectionStrings:DefaultConnection` (shared).
- **Default Schema**: `lesson`.

## 5. Resilience & Performance
- **Indexes**:
    - Index on `plans(SubjectId)`.
    - Index on `resources(PlanId)`.
- **Eager Loading**: Query handlers should consider using `.Include(p => p.Resources)` to avoid N+1 issues when retrieving full plans.

## 6. Integration Points
- **Health Checks**: Standard EF Core DbContext health check.

## 7. Migration & Deployment
- **Database Migrations**: Managed via `Lex.Module.LessonManagement.Infrastructure` project.
- **Initial Migration**: Must create the `lesson` schema.
