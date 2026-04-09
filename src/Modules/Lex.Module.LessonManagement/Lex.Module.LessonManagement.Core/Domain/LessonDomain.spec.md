# Domain Specification: Lesson Planning

## 1. Context
- **Module**: LessonManagement
- **Scope**: Lifecycle management of pedagogical content and associated digital assets.

## 2. Core Ubiquitous Language
- **LessonPlan**: A structured document defining the goals, strategy, and content for a lesson.
- **Published Lesson**: A frozen, versioned snapshot of a LessonPlan ready for use in the timetable.
- **Resource**: A digital asset (document, video, image) associated with a lesson to support teaching.
- **Draft**: The initial, editable state of a lesson plan.

## 3. Domain Model Hierarchy

- **Aggregate Root: LessonPlan**
    - **Purpose**: Manage the lifecycle and content of a lesson.
    - **Invariants**:
        - Only a `Draft` or `Rejected` lesson can be modified.
        - A `Published` lesson plan is immutable. Any modification requires a new version.
        - Must be associated with a `SubjectId`.
- **Entity: LessonResource**
    - **Purpose**: Associate a specific file (from ObjectStorage) with a lesson plan.
    - **Invariants**:
        - Must reference a valid `FileId` GUID.
        - Max file size and type restrictions are handled by the `ObjectStorage` module.

## 4. Value Objects
- **SubjectReference**: Contains `SubjectId` and cached `SubjectName` (for read-only convenience).
- **BlockContent**: The polymorphic body of the lesson plan (text, diagrams, etc.).

## 5. Domain Events
- **LessonPlanPublished**: Occurs when a draft is finalized. Triggers snapshotting.
- **LessonResourceAttached**: Occurs when a teacher links a file to a plan.

## 6. Business Operations (Conceptual)
- **Op: Create Draft**
    - **Resulting State**: `Draft` status.
- **Op: Publish Version**
    - **Invariants Checked**: Lesson body must not be empty.
    - **Resulting State**: `Published` status. Immutable snapshot created.
