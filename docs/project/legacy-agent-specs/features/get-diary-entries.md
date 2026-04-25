# Feature Specification: Get Diary Entries

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Allows users to view historical diary records for a specific class or timeframe.
- **Type**: Query
- **User Story**: As a Teacher, I want to review previous lessons for my class to maintain continuity.

## 2. Request Representation
- **Request Class**: `GetDiaryEntriesQuery`
- **Payload Structure**:
    - `ClassId` (Guid)
    - `StartDate` (DateTime)
    - `EndDate` (DateTime)

## 3. Business Logic (The Slice)
- **Trigger**: REST GET `/api/diary/entries?classId=...&start=...&end=...`
- **Logic**: Reads from `diary.entries`.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `diary.entries`
- **Repository Methods**: `IDiaryRepository.SearchAsync()`

## 5. Domain Objects (High-Level)
- **DiaryEntryDto**: Data transfer object for the UI list.

## 6. API / UI Contracts
- **Route**: `GET /api/diary/entries`
- **Response**: `Result<List<DiaryEntryDto>>`

## 7. Security
- **Required Permission**: `DiaryPermissions.View`
- **Auth Policy**: `RequireAnyAuthenticated` (Subject to data scope checks)
