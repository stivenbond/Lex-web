# Feature Specification: Update Lesson Plan

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Update the content, metadata, and resources of a draft lesson plan.
- **Type**: Command
- **User Story**: As a Teacher, I want to update my lesson plan so that I can refine the teaching strategy and content.

## 2. Request Representation
- **Request Class**: `UpdateLessonPlanCommand`
- **Payload Structure**:
    - `Id`: GUID
    - `Title`: String
    - `Body`: `BlockContent`
- **Validation Rules**:
    - `Id` must not be empty.
    - `Title` must not be empty.
    - `Body` must be valid `BlockContent`.

## 3. Business Logic (The Slice)
- **Trigger**: REST `PUT /api/lessons/{id}`
- **Domain Logic**:
    - Load `LessonPlan` by `Id`.
    - Ensure `Status` is `Draft` or `Rejected`.
    - Update `Title` and `Body`.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.lesson_plans`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **LessonPlan**: Aggregate root updated with new content.

## 6. API / UI Contracts
- **Route**: `PUT /api/lessons/{id}`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by auto-save or "Save" button in the Tiptap editor.

## 7. Security
- **Required Permission**: `LessonPermissions.Edit`
- **Auth Policy**: `TeacherOnly`
