# Feature Specification: Get Lesson Plans for Subject

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Retrieve a list of all lesson plans (draft and published) for a specific subject.
- **Type**: Query
- **User Story**: As a Teacher, I want to see all lesson plans for "Math 10A" so that I can reuse or manage them.

## 2. Request Representation
- **Request Class**: `GetLessonPlansForSubjectQuery`
- **Payload Structure**:
    - `SubjectId`: GUID
- **Validation Rules**:
    - `SubjectId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/lessons/subject/{subjectId}`
- **Domain Logic**:
    - Filter `lesson.lesson_plans` by `SubjectId`.
    - Return a paged list of summary DTOs.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.lesson_plans`
- **Repository Methods**: `GetBySubjectIdAsync`

## 5. Domain Objects (High-Level)
- **LessonPlanSummaryDto**: Basic info (ID, Title, CreatedDate, Status).

## 6. API / UI Contracts
- **Route**: `GET /api/lessons/subject/{subjectId}`
- **Response**: `Result<PagedList<LessonPlanSummaryDto>>`
- **UI Interaction**: Displayed in the Subject-specific lesson library.

## 7. Security
- **Required Permission**: `LessonPermissions.View`
- **Auth Policy**: `TeacherOnly`
