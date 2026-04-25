# Domain Specification: Lesson Planning

## 1. Context
- **Module**: LessonManagement
- **Scope**: Intellectual property and pedagogical preparation mapping.

## 2. Core Ubiquitous Language
- **LessonPlan**: The structural blueprint of a lesson.
- **Resource**: External materials (PDFs, PPTs, Videos) linked to the plan.
- **Subject**: The curriculum area the plan belongs to.

## 3. Domain Model Hierarchy

- **Aggregate Root: LessonPlan**
    - **Purpose**: A reusable plan containing objectives, activities, and resources.
    - **Invariants**:
        - A plan must have a title and a subject.
        - Resources must be tagged with a type.
        - Only Published plans can be seen by Students.

## 4. Value Objects
- **BlockContent**: The formatted body of the plan (objectives, methodology).
- **ResourceMetadata**: Name, Type, ExternalLink/FileId.

## 5. Domain Events
- **LessonPlanPublishedEvent**: Triggered when a plan is ready for use.
- **LessonResourceAttachedEvent**: Triggered when a new material is linked.

## 6. Business Operations (Conceptual)
- **Op: AuthorPlan**: Creates or updates draft content.
- **Op: FinalizePlan**: Sets state to Published.
- **Op: ClonePlan**: Creates a copy of an existing plan for modification.
