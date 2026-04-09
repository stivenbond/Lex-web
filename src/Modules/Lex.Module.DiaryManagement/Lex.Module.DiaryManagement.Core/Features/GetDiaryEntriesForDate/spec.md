# Feature Specification: Get Diary Entries for Date

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Retrieve all diary entries for all classes for a specific date (usually for a teacher's view).
- **Type**: Query
- **User Story**: As a Teacher, I want to see all my diary entries for today so that I can quickly review my teaching day.

## 2. Request Representation
- **Request Class**: `GetDiaryEntriesForDateQuery`
- **Payload Structure**:
    - `Date`: DateTimeOffset
    - `TeacherId`: GUID (Optional, filters for specific teacher)
- **Validation Rules**:
    - `Date` must be valid.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/diary/date/{date}`
- **Domain Logic**:
    - Fetch entries from `diary.diary_entries`.
    - Join with `scheduling.slots` to filter by `TeacherId` if provided and to get the period/subject context.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`, `scheduling.slots`
- **Repository Methods**: `GetByDateAsync`

## 5. Domain Objects (High-Level)
- **DiaryEntrySummaryDto**: Includes subject, period, and status.

## 6. API / UI Contracts
- **Route**: `GET /api/diary/date/{date}`
- **Response**: `Result<List<DiaryEntrySummaryDto>>`
- **UI Interaction**: Displayed in the "My Day" dashboard or "Timetable" quick-view.

## 7. Security
- **Required Permission**: `DiaryPermissions.View`
- **Auth Policy**: `TeacherOnly` (plus filter for current teacher in handler)
