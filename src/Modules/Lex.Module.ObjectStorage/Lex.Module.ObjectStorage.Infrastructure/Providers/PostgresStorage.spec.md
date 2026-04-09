# Infrastructure Specification: PostgreSQL Bytea Storage

## 1. Overview
- **Category**: File Provider
- **Parent Module**: ObjectStorage
- **Component Name**: PostgresByteaProvider

## 2. Technical Strategy
- **Base Technology**: EF Core, PostgreSQL `bytea` column type.
- **Storage/Transport**: PostgreSQL.

## 3. Implementation Details
Stores file bytes directly in a table within the `object_storage` schema. Ideal for small files and low-infrastructure on-premises setups.

- **Interface Realized**: `IObjectStorageProvider`
- **Classes/Interfaces**:
    - **PostgresByteaProvider**: Implementation using EF Core `DbContext`.
    - **FileContent**: Private entity representing the `file_contents` table.

## 4. Configuration & Settings
- **Settings Class**: `PostgresStorageSettings`
- **Key in `appsettings.json`**: `ObjectStorage:Postgres`
- **Required Secrets**: DB connection string (handled by global settings).

## 5. Resilience & Performance
- **Polly Policies**: Retry on transient DB deadlocks or connection failures.
- **Streaming**: Uses PostgreSQL `LO_OPEN` or `bytea` streaming if possible, otherwise buffered for small files.
- **Timeout Model**: Standard (dependent on DB command timeout).

## 6. Integration Points
- **Health Checks**: Verified via the `object_storage` schema readiness check.
- **Metrics/Logging**: `module.objectstorage.postgres.bytes_written` (counter).

## 7. Migration & Deployment
- **Database Migrations**: `object_storage.file_contents` table creation.
- **Post-Deploy Verification**: Small file upload/download smoke test.
