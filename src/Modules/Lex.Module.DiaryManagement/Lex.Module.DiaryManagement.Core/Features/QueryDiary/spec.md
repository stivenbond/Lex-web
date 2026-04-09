# Feature Specification: Diary Queries

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Read-only access to diary entries for classes, dates, or specific records.
- **Type**: Query
- **User Story**: As a User, I want to review past session notes so that I can track progress over time.

## 2. Request Representation
- **Request Classes**:
    - `GetDiaryEntriesForClassQuery` { ClassId, StartDate, EndDate }
    - `GetDiaryEntriesForDateQuery` { Date }
    - `GetDiaryEntryByIdQuery` { EntryId }
- **Validation Rules**: IDs must be valid. Date range validation (max 1 year).

## 3. Business Logic (The Slice)
- **Trigger**: `REST GET /api/diary/entries?classId=...` etc.
- **Logic**: Optimized SQL queries on the `diary.entries` table. Optionally includes attachments metadata.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `diary.entries`, `diary.attachments`.
- **Repository Methods**: `GetForClassAsync`, `GetByDateAsync`, `GetByIdAsync`.

## 5. Domain Objects (High-Level)
- **DiaryEntryDto**: Data transfer object for the entry, including `BlockContent`.

## 6. API / UI Contracts
- **Route**: `GET /api/diary/entries`
- **Response**: `Result<List<DiaryEntryDto>>` or `Result<DiaryEntryDto>`.
- **UI Interaction**: Feed view of sessions or detailed record view.

## 7. Security
- **Required Permission**: `DiaryPermissions.ViewDiary`.
- **Auth Policy**: Context-based (must be assigned teacher, student in class, or head of department).
