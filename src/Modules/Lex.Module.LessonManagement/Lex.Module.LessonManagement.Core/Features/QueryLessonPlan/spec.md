# Feature Specification: Query Lesson Plan

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Read-only access to lesson plans by various criteria.
- **Type**: Query
- **User Story**: As a Teacher, I want to find my lesson plans by subject or date so that I can prepare for my classes.

## 2. Request Representation
- **Request Classes**:
    - `GetLessonPlanByIdQuery` { PlanId }
    - `GetLessonPlansForSubjectQuery` { SubjectId }
    - `GetLessonPlansForPeriodQuery` { PeriodId }
- **Validation Rules**: IDs must be valid.

## 3. Business Logic (The Slice)
- **Trigger**: `REST GET /api/lessons/plans?subjectId=...` etc.
- **Logic**: Sequential SQL queries on the `lesson.plans` and `lesson.resources` tables.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `lesson.plans`, `lesson.resources`.
- **Repository Methods**: `GetByIdAsync`, `GetBySubjectAsync`.

## 5. Domain Objects (High-Level)
- **LessonPlanDto**: Data transfer object including `BlockContent` and resource list.

## 6. API / UI Contracts
- **Route**: `GET /api/lessons/plans`
- **Response**: `Result<List<LessonPlanDto>>` or `Result<LessonPlanDto>`.
- **UI Interaction**: Lesson library view with filtering by subject.

## 7. Security
- **Required Permission**: `LessonPermissions.ManageLessons` (View).
- **Auth Policy**: Context-based (must be the owner or authorized staff).
