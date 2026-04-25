# Feature Specification: Manage Diary Entry Content

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Allows teachers to record or edit the content of a class diary.
- **Type**: Command
- **User Story**: As a Teacher, I want to record what I taught in a lesson so that my pedagogical records are complete.

## 2. Request Representation
- **Request Class**: `UpdateDiaryEntryCommand` (used for both initial record and updates)
- **Payload Structure**:
    - `DiaryEntryId` (Guid)
    - `Content` (BlockContent object - text, images, etc.)
    - `SubjectCoverage` (String)
    - `Attachments` (List of FileIds)
- **Validation Rules**:
    - `DiaryEntryId` must exist and be in `Draft` state.
    - `Content` must follow the `BlockContent` schema.

## 3. Business Logic (The Slice)
- **Trigger**: REST PATCH `/api/diary/entries/{id}`
- **Domain Logic**: 
    - Loads the `DiaryEntry` aggregate.
    - Verifies ownership (Must be the teacher assigned to the Slot).
    - Updates the body and attachment references.
- **Side Effects**: None (until submission).

## 4. Persistence
- **Affected Tables**: `diary.entries`, `diary.attachments`
- **Repository Methods**: `IDiaryRepository.UpdateAsync()`

## 5. Domain Objects (High-Level)
- **DiaryEntry**: The aggregate root being modified.
- **BlockContent**: The complex value object storing the body.

## 6. API / UI Contracts
- **Route**: `PATCH /api/diary/entries/{id}`
- **Response**: `Result<Success>`

## 7. Security
- **Required Permission**: `DiaryPermissions.EditOwn`
- **Auth Policy**: `RequireDiaryOwner`
