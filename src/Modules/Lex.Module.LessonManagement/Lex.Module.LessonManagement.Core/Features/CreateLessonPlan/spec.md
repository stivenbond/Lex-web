# Feature Specification: Create Lesson Plan

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Create a new draft lesson plan for a specific subject.
- **Type**: Command
- **User Story**: As a Teacher, I want to create a new lesson plan draft so that I can start preparing my teaching material.

## 2. Request Representation
- **Request Class**: `CreateLessonPlanCommand`
- **Payload Structure**:
    - `Title`: String
    - `SubjectId`: GUID
- **Validation Rules**:
    - `Title` must not be empty.
    - `SubjectId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/lessons`
- **Domain Logic**:
    - A new `LessonPlan` aggregate is created with `Status = Draft`.
    - Association with the `SubjectId` is established.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.lesson_plans`
- **Repository Methods**: `AddAsync(LessonPlan plan)`

## 5. Domain Objects (High-Level)
- **LessonPlan**: Aggregate root initialized with title and subject.

## 6. API / UI Contracts
- **Route**: `POST /api/lessons`
- **Response**: `Result<Guid>` (The LessonPlan ID)
- **UI Interaction**: Triggered by the "New Lesson Plan" button in the Lesson Library.

## 7. Security
- **Required Permission**: `LessonPermissions.Create`
- **Auth Policy**: `TeacherOnly`
