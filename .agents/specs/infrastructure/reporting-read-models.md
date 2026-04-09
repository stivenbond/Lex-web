# Infrastructure Specification: Reporting Read Models

## 1. Overview
- **Category**: Denormalized Data Stores
- **Parent Module**: Reporting
- **Component Name**: Cross-Module Read Models

## 2. Technical Strategy
- **Base Technology**: PostgreSQL Tables (Separate for each view), MassTransit Consumers.
- **Approach**: Event-driven eventual consistency.

## 3. Implementation Details
Defines the structure of the `reporting.*` schema tables.

- **Table: `class_diary_summary`**:
    - Aggregates: `ClassId`, `LastLessonDate`, `TotalLessons`, `AverageScore`.
    - Source Events: `DiaryEntryApprovedEvent`.
- **Table: `student_assessment_results`**:
    - Aggregates: `StudentId`, `AssessmentId`, `FinalScore`, `SubmissionDate`.
    - Source Events: `AssessmentGradedEvent`.

## 4. Consumer List
The reporting module maintains a dedicated "sync" service that subscribes to:
- `SlotAssignedEvent`
- `DiaryEntryApprovedEvent`
- `AssessmentGradedEvent`
- `FileProcessingCompletedEvent`

## 5. Resilience & Performance
- **Consistency**: Data is eventually consistent (latency < 1s).
- **Indexing**: Optimized for the complex filtering required by search dashboards.

## 6. Integration Points
- **Health Checks**: Checks that the Sync Consumers are active and the lag is low.

## 7. Migration & Deployment
- On first deploy, a "Bootstrap" command can be triggered to query other modules' APIs to populate the initial state (Historical Sync).
