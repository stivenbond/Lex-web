# Domain Specification: File Records

## 1. Context
- **Module**: ObjectStorage
- **Scope**: Metadata tracking and storage mapping for all binary assets in the system.

## 2. Core Ubiquitous Language
- **FileRecord**: The aggregate root tracking a file's existence and metadata.
- **Provider**: The physical storage engine (Postgres vs. Garage).
- **BlobKey**: The unique identifier for the bytes within the provider's storage.
- **Hash/Checksum**: SHA-256 hash of the content to ensure integrity and deduplication.

## 3. Domain Model Hierarchy

- **Aggregate Root: FileRecord**
    - **Purpose**: To provide a single source of truth for where a file is physically located and what it represents.
    - **Invariants**: 
        - Must have a non-empty `OriginalFileName` and `ContentType`.
        - `Size` must be non-negative.
        - `StorageLocation` must match a configured provider.

- **Value Object: StorageLocation**
    - **Purpose**: Decouples the record from the provider implementation.
    - **Composition**: `ProviderId`, `BucketName`, `Key`.

## 4. Value Objects
- **FileMetadata**: Extensible metadata (Width/Height for images, Duration for audio).
- **StorageProvider (Enum)**: `PostgreSQL`, `GarageS3`, `LocalStorage`.

## 5. Domain Events
- **FileRecordCreated**: Triggered when metadata is first persisted.
- **StorageCommitted**: Triggered when the physical bytes have been confirmed by the provider.

## 6. Business Operations (Conceptual)
- **Op: Register File**
    - **Invariants Checked**: Valid file type allowed by system policy.
    - **Resulting State**: `FileRecord` created with status `Pending`.
- **Op: Commit Storage**
    - **Resulting State**: Status changed to `Available`. `FileUploadedEvent` published to MassTransit.
