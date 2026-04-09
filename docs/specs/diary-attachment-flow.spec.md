# Infrastructure Specification: Diary File Attachment Flow

## 1. Overview
- **Category**: Cross-Module Orchestration
- **Parent Module**: DiaryManagement
- **Component Name**: Diary File Attachment Flow

## 2. Technical Strategy
This spec defines the sequence of operations for attaching files to a Diary Entry, ensuring loose coupling between the `DiaryManagement` and `ObjectStorage` modules.

## 3. Implementation Details

### Sequential Flow
The flow follows the user's design for client-side orchestration:

1.  **Upload Phase**:
    *   Web Client calls `POST /api/files/upload` (ObjectStorage module).
    *   `ObjectStorage` processes the stream, stores bytes in Garage (S3), and records metadata in `storage.file_records`.
    *   API returns a `FileId` (GUID).

2.  **Attachment Phase**:
    *   Web Client includes the `FileId` in a `UpdateDiaryEntryCommand` or `SubmitDiaryEntryCommand`.
    *   `DiaryManagement` stores the `FileId` in a link table `diary.diary_entry_attachments`.
    *   **Inbound Event Handling**: `DiaryManagement` subscribes to `FileUploadedEvent` from `ObjectStorage` (optional, for verification) but primarily relies on the client providing the ID.

3.  **Handoff/Verification**:
    *   When a user views a Diary Entry, `DiaryManagement` returns the `FileId`s.
    *   The Web Client uses these IDs to generate download links via `GET /api/files/{id}` (ObjectStorage).

- **Implementation Note**: `DiaryManagement` NEVER touches the `storage.*` schema. It only stores the GUID reference.

## 4. Configuration & Settings
- **Interface Realized**: No direct interface; purely event-driven and ID-based.

## 5. Resilience & Performance
- **Garbage Collection**: If a `FileId` is uploaded but never linked to a Diary Entry (or any other module) within 24 hours, the `ObjectStorage` module may run a cleanup job (to be specced in ObjectStorage).
- **Concurrency**: Multiple files can be attached in a single `UpdateDiaryEntryCommand`.

## 6. Integration Points
- **Outbound Events**: `DiaryManagement` might publish `FileAttachedToDiaryEvent` to notify `ObjectStorage` to pin the file (prevent cleanup).

## 7. Migration & Deployment
- **Database Migrations**: `diary.diary_entry_attachments` table migration.
