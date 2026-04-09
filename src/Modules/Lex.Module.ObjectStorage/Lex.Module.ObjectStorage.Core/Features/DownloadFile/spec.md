# Feature Specification: Download File (Streaming)

## 1. Feature Overview
- **Parent Module**: ObjectStorage
- **Description**: Secure, mediated access to binary objects stored in any provider.
- **Type**: Query
- **User Story**: As a User, I want to download or view a file attached to a resource so that I can access its content securely.

## 2. Request Representation
- **Request Class**: `DownloadFileQuery`
- **Payload Structure**: 
    - `FileId` (GUID): The unique identifier of the `FileRecord`.
- **Validation Rules**: `FileId` must not be empty.

## 3. Business Logic (The Slice)
The handler orchestrates metadata retrieval and cross-module permission checking.

- **Trigger**: `REST GET /api/files/{id}`
- **Logic**:
    1. Load `FileRecord` from DB.
    2. Perform module-specific permission check (e.g., if the file is a Diary attachment, does the user have access to that Diary entry?).
    3. Determine active Provider (Postgres vs. Garage).
    4. Request stream from `IObjectStorageProvider`.
- **Side Effects**: Increment "Download Count" in metadata (optional).

## 4. Persistence
- **Affected Tables**: `object_storage.file_records`
- **Repository Methods**: `GetByIdAsync`, `GetFileStreamAsync`.

## 5. Domain Objects (High-Level)
- **IObjectStorageProvider**: Infrastructure abstraction for providers.

## 6. API / UI Contracts
- **Route**: `GET /api/files/{id}`
- **Response**: `FileStreamResult` (raw bytes with correct MIME type).
- **UI Interaction**: Standard `<img src="...">` or `<a href="...">` link.

## 7. Security
- **Required Permission**: `ObjectStoragePermissions.ViewFiles` (Plus dynamic ownership/context checks).
- **Auth Policy**: `SameOrigin` or authenticated identity required.
