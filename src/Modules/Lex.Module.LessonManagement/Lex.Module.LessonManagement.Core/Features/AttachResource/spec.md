# Feature Specification: Attach Resource

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Link a digital asset (uploaded to ObjectStorage) to a lesson plan.
- **Type**: Command
- **User Story**: As a Teacher, I want to attach a PDF or video to my lesson plan so that I can access it during teaching.

## 2. Request Representation
- **Request Class**: `AttachResourceCommand`
- **Payload Structure**:
    - `LessonPlanId`: GUID
    - `FileId`: GUID
    - `DisplayName`: String
- **Validation Rules**:
    - `LessonPlanId` must not be empty.
    - `FileId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/lessons/{id}/resources`
- **Domain Logic**:
    - Load `LessonPlan`.
    - Ensure status is `Draft` or `Rejected`.
    - Create a new `LessonResource` entity and add it to the plan's collection.
- **Side Effects**:
    - Publish `LessonResourceAttachedEvent`.

## 4. Persistence
- **Affected Tables**: `lesson.lesson_resources`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **LessonResource**: Entity containing `FileId` link and metadata.

## 6. API / UI Contracts
- **Route**: `POST /api/lessons/{id}/resources`
- **Response**: `Result<Guid>` (The Resource ID)
- **UI Interaction**: Triggered when a teacher finishes uploading a file or selects one from the library.

## 7. Security
- **Required Permission**: `LessonPermissions.Edit`
- **Auth Policy**: `TeacherOnly`
