# Infrastructure Specification: Diary Database

## 1. Overview
- **Category**: DB Repository implementation
- **Parent Module**: DiaryManagement
- **Component Name**: DiaryDbContext

## 2. Technical Strategy
- **Base Technology**: EF Core 10, Npgsql.
- **Storage/Transport**: PostgreSQL.

## 3. Implementation Details
The module uses a dedicated slice of the database under the `diary` schema.

- **Interface Realized**: `IDiaryDbContext`
- **Tables**:
    - `diary.entries`: Main table for session records. Includes `Body` (jsonb/text).
    - `diary.attachments`: Join table storing references to `FileRecord` IDs.

## 4. Configuration & Settings
- **Settings Class**: `DiaryDbSettings`
- **Key in `appsettings.json`**: `ConnectionStrings:DefaultConnection` (shared).
- **Default Schema**: `diary`.

## 5. Resilience & Performance
- **Indexes**:
    - Index on `entries(SlotId)` for lookup by period.
    - Index on `entries(ClassId, Date)` for feed lookups.
- **Soft Deletion**: Consider implementing soft delete (`IsDeleted`) for entries to maintain an audit trail.

## 6. Integration Points
- **Health Checks**: Standard EF Core DbContext health check.

## 7. Migration & Deployment
- **Database Migrations**: Managed via `Lex.Module.DiaryManagement.Infrastructure` project.
- **Initial Migration**: Must create the `diary` schema before table creation.
