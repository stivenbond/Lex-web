# Feature Specification: Get Lesson Plans for Period

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Retrieve the lesson plan associated with a specific teaching period or slot.
- **Type**: Query
- **User Story**: As a Teacher, I want to see which lesson plan is assigned to my next period so that I can start delivery.

## 2. Request Representation
- **Request Class**: `GetLessonPlansForPeriodQuery`
- **Payload Structure**:
    - `PeriodId`: GUID
    - `Date`: DateTimeOffset
- **Validation Rules**:
    - `PeriodId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/lessons/period/{periodId}`
- **Domain Logic**:
    - The mapping between a `Slot` (from Scheduling) and a `LessonPlan` is stored in the `LessonManagement` module (e.g., in a `lesson.lesson_slot_assignments` table).
    - If no specific assignment exists, the query might return the latest `Published` lesson plan for the `Subject` associated with that slot.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.lesson_plans`, `lesson.lesson_slot_assignments`
- **Repository Methods**: `GetByPeriodIdAsync`

## 5. Domain Objects (High-Level)
- **LessonPlanDto**: Full details of the assigned plan.

## 6. API / UI Contracts
- **Route**: `GET /api/lessons/period/{periodId}`
- **Response**: `Result<LessonPlanDto>`
- **UI Interaction**: Triggered by the "Open Lesson" action in the timetable grid.

## 7. Security
- **Required Permission**: `LessonPermissions.View`
- **Auth Policy**: `TeacherOnly`
