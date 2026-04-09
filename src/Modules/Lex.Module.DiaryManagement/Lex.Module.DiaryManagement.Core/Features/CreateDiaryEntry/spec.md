# Feature Specification: Create Diary Entry

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Create a new draft diary entry associated with a specific schedule slot.
- **Type**: Command
- **User Story**: As a Teacher, I want to create a diary entry for my lesson so that I can document the progress of the curriculum.

## 2. Request Representation
- **Request Class**: `CreateDiaryEntryCommand`
- **Payload Structure**:
    - `SlotId`: GUID (Reference to Scheduling slot)
    - `Subject`: String (Title/Topic of the lesson)
    - `Date`: DateTimeOffset
- **Validation Rules**:
    - `SlotId` must not be empty.
    - `Subject` must not be empty (max 200 chars).

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/diary`
- **Domain Logic**:
    - The module verifies that a diary entry for the given `SlotId` does not already exist (idempotency/limit one entry per slot).
    - A new `DiaryEntry` aggregate is created with `Status = Draft`.
- **Side Effects**: None (it is a draft).

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`
- **Repository Methods**: `AddAsync(DiaryEntry entry)`

## 5. Domain Objects (High-Level)
- **DiaryEntry**: Aggregate Root. Contains `SlotId`, `Subject`, `Body` (BlockContent), and `Status`.

## 6. API / UI Contracts
- **Route**: `POST /api/diary`
- **Response**: `Result<Guid>` (The Entry ID)
- **UI Interaction**: Triggered when a teacher clicks an empty cell or an "Add Entry" button on a specific slot in the timetable.

## 7. Security
- **Required Permission**: `DiaryPermissions.Create`
- **Auth Policy**: `TeacherOnly`
