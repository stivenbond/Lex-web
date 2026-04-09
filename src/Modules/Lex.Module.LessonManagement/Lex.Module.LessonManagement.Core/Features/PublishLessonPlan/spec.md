# Feature Specification: Publish Lesson Plan

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Finalize a lesson plan draft and create an immutable version for use in scheduling.
- **Type**: Command
- **User Story**: As a Teacher, I want to publish my lesson plan so that I can link it to a specific time slot on my timetable.

## 2. Request Representation
- **Request Class**: `PublishLessonPlanCommand`
- **Payload Structure**:
    - `Id`: GUID
- **Validation Rules**:
    - `Id` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/lessons/{id}/publish`
- **Domain Logic**:
    - Load `LessonPlan`.
    - Ensure status is `Draft` or `Rejected`.
    - Set status to `Published`.
    - Store the completion timestamp.
- **Side Effects**:
    - Publish `LessonPlanPublishedEvent` for other modules (e.g., Scheduling, Diary).

## 4. Persistence
- **Affected Tables**: `lesson.lesson_plans`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **LessonPlanPublishedEvent**: Contains `LessonPlanId`, `SubjectId`, and `VersionMetadata`.

## 6. API / UI Contracts
- **Route**: `POST /api/lessons/{id}/publish`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Publish" button in the editor. Shows a confirmation modal explaining that the lesson will become read-only.

## 7. Security
- **Required Permission**: `LessonPermissions.Publish`
- **Auth Policy**: `TeacherOnly`
