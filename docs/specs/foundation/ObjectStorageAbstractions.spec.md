# Infrastructure Specification: Object Storage Abstractions

## 1. Overview
- **Category**: Storage Abstraction
- **Parent Module**: SharedKernel (Core Interface)
- **Component Name**: IObjectStorageService

## 2. Technical Strategy
- **Base Technology**: Provider-agnostic stream handling.
- **Role**: Decouples domain modules from specific storage implementations (PostgreSQL vs. Garage/S3).

## 3. Implementation Details
The abstraction ensures that modules like `DiaryManagement` or `LessonManagement` never know *where* a file is stored, only that it can be retrieved by an ID.

- **Interface Realized**: `IObjectStorageService`
- **Classes/Interfaces**:
    - **IObjectStorageService**:
        - `Task<Result<Guid>> UploadAsync(Stream data, string fileName, string contentType, CancellationToken ct)`
        - `Task<Result<Stream>> DownloadAsync(Guid fileId, CancellationToken ct)`
        - `Task<Result> DeleteAsync(Guid fileId, CancellationToken ct)`
    - **IFileStorageService**: (Specific to file-system-like operations if needed)

## 4. Configuration & Settings
- **Implementation Mapping**: Managed by the `ObjectStorage` module via Dependency Injection.

## 5. Resilience & Performance
- **Streaming**: Must use `Stream` and `CancellationToken` to handle large files without exhausting RAM and to allow graceful cancellation.
- **Result Pattern**: Returns `Result<T>` to handle storage-full or file-not-found scenarios gracefully.

## 6. Integration Points
- **Consuming Modules**: Any module requiring attachments.
- **Implementing Module**: `ObjectStorage` module (via its Infrastructure project).

## 7. Migration & Deployment
- **Provider Switching**: The interface allows switching from local Postgres storage to Garage S3 without changing any consumer code in other modules.
