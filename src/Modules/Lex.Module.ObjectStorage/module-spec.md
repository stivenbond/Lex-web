# Module Specification: ObjectStorage

## 1. Overview
- **Name**: ObjectStorage
- **Purpose**: Manage physical binary data storage using a dual-provider strategy (PostgreSQL bytea and Garage S3).
- **Schema**: `object_storage.*`
- **Primary Stakeholders**: All modules requiring file attachments (Diary, Lessons, Assessments).

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module.

- **Aggregate Root: FileRecord**: Tracks the metadata and location of a stored object.
- **Value Object: StorageLocation**: Encapsulates the provider identity (Postgres vs. Garage) and the specific key/bucket.

## 3. Module Responsibilities
- Provide a consistent `IObjectStorageService` abstraction to the rest of the system.
- Mediate access to files via permission checks in `FileController`.
- Persist binary data into PostgreSQL `bytea` (initial) or Garage S3 (scaled).
- Track file metadata (Size, ContentType, Hash) for integrity.

## 4. Integration & Dependencies
- **Inbound Events**: 
    - None directly (usually invoked via `IObjectStorageService` in other modules' handlers).
- **Outbound Events**: 
    - `FileUploadedEvent`: Triggered when a file is successfully persisted.
- **External Dependencies**: 
    - PostgreSQL (EF Core)
    - Garage (S3-compatible API via AWS SDK)

## 5. Security & Authorization
- **Permission Constants**: `ObjectStoragePermissions.cs`
- **Policies**: 
    - `CanUploadFiles`: Required to invoke the upload endpoint.
    - `CanDownloadFiles`: Checked dynamically based on ownership or module-specific associations.

## 6. Cross-Module Interactions
- The `ObjectStorage` module acts as a leaf node in the dependency graph. Other modules depend on its abstractions (SharedKernel) but not its implementation.
- `FileProcessing` module reacts to `FileUploadedEvent` to generate thumbnails or extract text.
