# Feature Specification: Approve Diary Entry

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Mark a submitted diary entry as approved.
- **Type**: Command
- **User Story**: As an Admin/Supervisor, I want to approve a diary entry so that it is officially recorded as completed.

## 2. Request Representation
- **Request Class**: `ApproveDiaryEntryCommand`
- **Payload Structure**:
    - `Id`: GUID
    - `Comments`: String (Optional)
- **Validation Rules**:
    - `Id` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/diary/{id}/approve`
- **Domain Logic**:
    - Load `DiaryEntry`.
    - Ensure current state is `Submitted`.
    - Change `Status` to `Approved`.
    - Record `ApprovedBy` and `ApprovalDate`.
- **Side Effects**:
    - Publish `DiaryEntryApprovedEvent` for the `Notifications` and `Reporting` modules.

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **DiaryEntryApprovedEvent**: Contains `EntryId`, `ApprovedBy`, and `TeacherId`.

## 6. API / UI Contracts
- **Route**: `POST /api/diary/{id}/approve`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Approve" button in the Supervisor's Review dashboard.

## 7. Security
- **Required Permission**: `DiaryPermissions.Approve`
- **Auth Policy**: `AdminOnly`
