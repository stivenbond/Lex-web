# Feature Specification: File Attachment Flow

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Allows teachers to attach files (images, documents) to a specific diary entry.
- **Type**: Command
- **User Story**: As a Teacher, I want to attach resources to my diary entry so that they are archived alongside the session notes.

## 2. Request Representation
- **Request Class**: `AttachFileToDiaryEntryCommand`
- **Payload Structure**: 
    - `EntryId` (Guid)
    - `FileId` (Guid) - The ID returned from the standalone `ObjectStorage` upload.
- **Validation Rules**: `EntryId` and `FileId` must exist.

## 3. Business Logic (The Slice)
The Diary module does not store files; it stores *references* to files managed by the `ObjectStorage` module.

- **Trigger**: `REST POST /api/diary/entries/{id}/attachments`
- **Logic**:
    1. Verify `DiaryEntry` existence and user permissions.
    2. Verify `FileRecord` existence in `ObjectStorage` (via internal service or DB join if allowed by architecture, otherwise event-based or direct call).
    3. Create `DiaryAttachment` linking the two IDs.
- **Side Effects**: Publishes `AttachmentAddedEvent`.

## 4. Persistence
- **Affected Tables**: `diary.attachments`.
- **Repository Methods**: `AddAttachmentAsync`.

## 5. Domain Objects (High-Level)
- **DiaryAttachment**: Join entity between `DiaryEntry` and `FileRecord` (referenced by ID only).

## 6. API / UI Contracts
- **Route**: `POST /api/diary/entries/{id}/attachments`
- **Response**: `Result`
- **UI Interaction**: File picker triggers `ObjectStorage` upload, then sends the resulting `FileId` to this endpoint.

## 7. Security
- **Required Permission**: `DiaryPermissions.ManageDiary`.
- **Auth Policy**: Ownership check on the `DiaryEntry`.
