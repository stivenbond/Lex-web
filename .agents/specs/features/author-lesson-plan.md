# Feature Specification: Lesson Plan Authoring

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Creation and management of reusable lesson plans.
- **Type**: Command
- **User Story**: As a Teacher, I want to create and organize my lesson plans so I can use them across multiple classes.

## 2. Request Representation
- **Command**: `UpdateLessonPlanCommand`
- **Payload Structure**:
    - `LessonPlanId` (Guid)
    - `Title` (String)
    - `SubjectId` (Guid)
    - `Body` (BlockContent)
    - `Resources` (List of ResourceId)
- **Validation Rules**:
    - Title is mandatory.
    - Subject must exist.

## 3. Business Logic (The Slice)
- **Trigger**: REST PATCH `/api/lessons/{id}`
- **Domain Logic**: Updates the `LessonPlan` aggregate. Business rules for content validation.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.plans`, `lesson.resources`
- **Repository Methods**: `ILessonRepository.UpdateAsync()`

## 5. Domain Objects (High-Level)
- **LessonPlan**: The aggregate.

## 6. API / UI Contracts
- **Route**: `PATCH /api/lessons/{id}`
- **Response**: `Result<Success>`

## 7. Security
- **Required Permission**: `LessonPermissions.Manage`
- **Auth Policy**: `RequireLessonAuthor`
