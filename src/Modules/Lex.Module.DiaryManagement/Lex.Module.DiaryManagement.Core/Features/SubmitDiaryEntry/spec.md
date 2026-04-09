# Feature Specification: Submit Diary Entry

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Mark a diary entry as submitted for review by an administrator/supervisor.
- **Type**: Command
- **User Story**: As a Teacher, I want to submit my diary entry so that my supervisor can review and approve my lesson progress.

## 2. Request Representation
- **Request Class**: `SubmitDiaryEntryCommand`
- **Payload Structure**:
    - `Id`: GUID
- **Validation Rules**:
    - `Id` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/diary/{id}/submit`
- **Domain Logic**:
    - Load `DiaryEntry`.
    - Ensure current state is `Draft` or `Rejected`.
    - Change `Status` to `Submitted`.
- **Side Effects**:
    - Publish `DiaryEntrySubmittedEvent` to the message bus for the `Notifications` module.

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **DiaryEntrySubmittedEvent**: Contains `EntryId`, `TeacherId`, and `Subject`.

## 6. API / UI Contracts
- **Route**: `POST /api/diary/{id}/submit`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Submit" button in the entry editor footer.

## 7. Security
- **Required Permission**: `DiaryPermissions.Edit`
- **Auth Policy**: `TeacherOnly`
