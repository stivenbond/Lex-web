# Feature Specification: Lesson Library Queries

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Search and retrieval of lesson plans within the pedagogical library.
- **Type**: Query
- **User Story**: As a Teacher, I want to find lesson plans for a specific subject so I can reuse existing material.

## 2. Request Representation
- **Request Class**: `GetLessonsQuery`
- **Payload Structure**:
    - `SubjectId` (Guid, optional)
    - `SearchTerm` (String, optional)
    - `Status` (Enum: Draft, Published)

## 3. Business Logic (The Slice)
- **Trigger**: REST GET `/api/lessons?subjectId=...&search=...`
- **Logic**: Performs a filtered synchronous read from the `lesson.plans` table.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.plans`
- **Repository Methods**: `ILessonRepository.SearchAsync()`

## 5. Domain Objects (High-Level)
- **LessonSummaryDto**: Light projection of the plan for library listing.

## 6. API / UI Contracts
- **Route**: `GET /api/lessons`
- **Response**: `Result<PagedList<LessonSummaryDto>>`

## 7. Security
- **Required Permission**: `LessonPermissions.View`
- **Auth Policy**: `RequireAnyAuthenticated`
