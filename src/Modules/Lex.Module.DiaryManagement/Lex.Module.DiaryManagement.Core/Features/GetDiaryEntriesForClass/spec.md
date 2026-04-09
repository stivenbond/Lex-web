# Feature Specification: Get Diary Entries for Class

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Retrieve all diary entries for a specific class within a date range.
- **Type**: Query
- **User Story**: As a Student or Parent, I want to see the diary entries for my class so that I can keep track of what was learned.

## 2. Request Representation
- **Request Class**: `GetDiaryEntriesForClassQuery`
- **Payload Structure**:
    - `ClassId`: GUID
    - `FromDate`: DateTimeOffset
    - `ToDate`: DateTimeOffset
- **Validation Rules**:
    - `ClassId` must not be empty.
    - `FromDate` must be before `ToDate`.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/diary/class/{classId}`
- **Domain Logic**:
    - Query the `diary.diary_entries` table.
    - Since `DiaryEntry` only store `SlotId`, the query may need to join with `scheduling.slots` (denormalized read model in Reporting if extracted, but in-process for now).
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`, `scheduling.slots` (read-only)
- **Repository Methods**: `GetByClassIdAsync`

## 5. Domain Objects (High-Level)
- **DiaryEntrySummaryDto**: Simplified view of an entry.

## 6. API / UI Contracts
- **Route**: `GET /api/diary/class/{classId}?from=...&to=...`
- **Response**: `Result<List<DiaryEntrySummaryDto>>`
- **UI Interaction**: Displayed in the class "Diary Feed" or "Subject Timeline".

## 7. Security
- **Required Permission**: `DiaryPermissions.View`
- **Auth Policy**: `GeneralAccess`
