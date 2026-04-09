# Feature Specification: Manage Diary Entry

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Creation, update, submission, and approval of daily session records.
- **Type**: Command
- **User Story**: As a Teacher, I want to record my session notes so that I can document curriculum delivery.

## 2. Request Representation
- **Request Classes**:
    - `CreateDiaryEntryCommand` { SlotId, SubjectId }
    - `UpdateDiaryEntryCommand` { EntryId, Body (BlockContent) }
    - `SubmitDiaryEntryCommand` { EntryId }
    - `ApproveDiaryEntryCommand` { EntryId }
- **Validation Rules**: 
    - Body must conform to `BlockContent` schema.
    - Cannot update if `Approved`.

## 3. Business Logic (The Slice)
- **Trigger**: `REST POST /api/diary/entries` etc.
- **Logic**:
    - Verify user identity vs `Slot` assignment.
    - Transition state based on command.
- **Side Effects**: Publishes `DiaryEntrySubmittedEvent` or `DiaryEntryApprovedEvent`.

## 4. Persistence
- **Affected Tables**: `diary.entries`
- **Repository Methods**: `GetByIdAsync`, `SaveAsync`.

## 5. Domain Objects (High-Level)
- **DiaryEntry**: Aggregate Root.
- **DiaryStatus**: Enum (Draft, Submitted, Approved).

## 6. API / UI Contracts
- **Route**: `POST /api/diary/entries`
- **Response**: `Result<Guid>`
- **UI Interaction**: Editor UI (Tiptap) for the `Body`.

## 7. Security
- **Required Permission**: `DiaryPermissions.ManageDiary`, `DiaryPermissions.ApproveDiary`.
- **Auth Policy**: Ownership check (Teacher must be assigned to Slot).
