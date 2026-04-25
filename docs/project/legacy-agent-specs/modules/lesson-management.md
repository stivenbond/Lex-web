# Module Specification: LessonManagement

## 1. Overview
- **Name**: LessonManagement
- **Purpose**: Managing the pedagogical library, lesson planning, and resource allocation.
- **Schema**: `lesson.*`
- **Primary Stakeholders**: Teachers (authoring plans), Students (viewing resources).

## 2. Domain Boundaries
- **Aggregate Root: LessonPlan**: A reusable pedagogical template for a specific topic.
- **Entity: LessonResource**: A document, file, or instruction set attached to a plan.

## 3. Module Responsibilities
- Life-cycle management of lesson plans (Draft -> Published).
- Organizing resources into a searchable library.
- Linking lesson plans to specific curriculum subjects.

## 4. Integration & Dependencies
- **Inbound Events**: None.
- **Outbound Events**:
    - `LessonPlanPublishedEvent`
    - `LessonResourceAttachedEvent`
- **External Dependencies**: Uses `IObjectStorageService` for external files.

## 5. Security & Authorization
- **Permission Constants**: `LessonPermissions.cs`
- **Policies**: `RequireLessonAuthor`.

## 6. Cross-Module Interactions
- **Scheduling**: Lesson plans are associated with `Slot` instances via the `Diary` (Indirect) or directly for preparation.
- **DiaryManagement**: When a diary is recorded, a Published `LessonPlan` can be referenced to import its content.
