# Feature Specification: Get Lesson Plan by ID

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Retrieve the full details of a lesson plan, including its content and resources.
- **Type**: Query
- **User Story**: As a Teacher, I want to view my lesson plan so that I can prepare for my class.

## 2. Request Representation
- **Request Class**: `GetLessonPlanByIdQuery`
- **Payload Structure**:
    - `Id`: GUID
- **Validation Rules**:
    - `Id` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/lessons/{id}`
- **Domain Logic**:
    - Load `LessonPlan` aggregate.
    - If status is `Published`, return the immutable snapshot.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.lesson_plans`, `lesson.lesson_resources`
- **Repository Methods**: `GetByIdAsync`

## 5. Domain Objects (High-Level)
- **LessonPlanDto**: Full representation of the plan and resources.

## 6. API / UI Contracts
- **Route**: `GET /api/lessons/{id}`
- **Response**: `Result<LessonPlanDto>`
- **UI Interaction**: Displayed in the Lesson Editor or Viewer.

## 7. Security
- **Required Permission**: `LessonPermissions.View`
- **Auth Policy**: `GeneralAccess`
