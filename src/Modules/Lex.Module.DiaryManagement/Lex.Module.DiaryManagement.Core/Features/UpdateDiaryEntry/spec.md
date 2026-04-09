# Feature Specification: Update Diary Entry

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Update the content and metadata of an existing diary entry.
- **Type**: Command
- **User Story**: As a Teacher, I want to update my diary entry so that I can add details or correct information about the lesson.

## 2. Request Representation
- **Request Class**: `UpdateDiaryEntryCommand`
- **Payload Structure**:
    - `Id`: GUID
    - `Subject`: String
    - `Body`: `BlockContent` (The rich text/diagram content)
    - `FileIds`: List<Guid> (Optional attachments)
- **Validation Rules**:
    - `Id` must not be empty.
    - `Body` must be valid `BlockContent`.

## 3. Business Logic (The Slice)
- **Trigger**: REST `PUT /api/diary/{id}`
- **Domain Logic**:
    - Load `DiaryEntry` by `Id`.
    - Ensure `Status` is `Draft` or `Rejected` (submitted/approved entries cannot be edited).
    - Update `Subject`, `Body`, and `AttachedFileIds`.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`, `diary.diary_entry_attachments`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **DiaryEntry**: Updated with new `BlockContent` and attachment list.

## 6. API / UI Contracts
- **Route**: `PUT /api/diary/{id}`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Save" or auto-save event in the Diary Editor.

## 7. Security
- **Required Permission**: `DiaryPermissions.Edit`
- **Auth Policy**: `TeacherOnly` (plus ownership check in handler)
