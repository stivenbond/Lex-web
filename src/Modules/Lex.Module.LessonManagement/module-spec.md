# Module Specification: LessonManagement

## 1. Overview
- **Name**: LessonManagement
- **Purpose**: Manage the lifecycle of lesson plans, including content creation, resource attachment, and publication for use in the timetable.
- **Schema**: `lesson.*`
- **Primary Stakeholders**: Teachers (prepare/manage lessons).

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module.

- **Aggregate Root: LessonPlan**: The primary definition of a lesson's goals and content.
- **Entity: LessonResource**: An attachment or reference linked to a specific lesson plan.
- **Value Object: SubjectReference**: Reference to the subject this lesson belongs to.

## 3. Module Responsibilities
- Provide a rich-text editor (BlockContent) for planning lessons.
- Manage the library of lesson plans and their resources.
- Support publishing lessons so they become immutable versions for delivery.
- Mediate access to lesson resources through `ObjectStorage`.

## 4. Integration & Dependencies
- **Inbound Events**: None (standalone planning).
- **Outbound Events**: 
    - `LessonPlanPublishedEvent`
    - `LessonResourceAttachedEvent`
- **External Dependencies**: 
    - `ObjectStorage` module for file assets.

## 5. Security & Authorization
- **Permission Constants**: `LessonPermissions.cs`
- **Policies**: 
    - `CanManageLessons`: Required for creating and editing plans.
    - `CanPublishLessons`: Required to finalize a plan.

## 6. Cross-Module Interactions
- **Scheduling**: Lesson plans reference a `SubjectId` which is shared across the platform.
- **AssessmentCreation**: Lessons may eventually link to assessments (future phase).
- **DiaryManagement**: Teachers can reference a published Lesson Plan when filling out a Diary Entry.
