# Feature Specification: Manage Lesson Plan

## 1. Feature Overview
- **Parent Module**: LessonManagement
- **Description**: Creation, update, publication, and resource management for lesson plans.
- **Type**: Command
- **User Story**: As a Teacher, I want to create and organize my lesson materials so that I can deliver them in class.

## 2. Request Representation
- **Request Classes**:
    - `CreateLessonPlanCommand` { Title, SubjectId }
    - `UpdateLessonPlanCommand` { PlanId, Body (BlockContent) }
    - `PublishLessonPlanCommand` { PlanId }
    - `AttachResourceCommand` { PlanId, FileId }
- **Validation Rules**: 
    - Title must not be empty.
    - Cannot update if `Published`.

## 3. Business Logic (The Slice)
- **Trigger**: `REST POST /api/lessons/plans` etc.
- **Logic**:
    - Verifies ownership (teacher who created the plan).
    - Manages versioning when publishing.
- **Side Effects**: Publishes `LessonPlanPublishedEvent` or `LessonResourceAttachedEvent`.

## 4. Persistence
- **Affected Tables**: `lesson.plans`, `lesson.resources`.
- **Repository Methods**: `GetByIdAsync`, `SaveAsync`.

## 5. Domain Objects (High-Level)
- **LessonPlan**: Aggregate Root.
- **LessonStatus**: Enum (Draft, Published, Archived).

## 6. API / UI Contracts
- **Route**: `POST /api/lessons/plans`
- **Response**: `Result<Guid>`
- **UI Interaction**: Rich text editor for the body; file uploader for resources.

## 7. Security
- **Required Permission**: `LessonPermissions.ManageLessons`.
- **Auth Policy**: Ownership check (user must be the creator).
