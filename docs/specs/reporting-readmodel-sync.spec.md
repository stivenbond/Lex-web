# Specification: Reporting Read Model Synchronization

## 1. Overview
- **Category**: Event-Driven Projection logic
- **Parent Module**: Reporting
- **Purpose**: Define the logic for updating the denormalized read model based on asynchronous domain events from source modules.

## 2. Event-to-Projection Mapping

| Source Event | Projection Table | Update Logic |
| :--- | :--- | :--- |
| **LessonManagement** | `LessonPublished` | `ClassPerformanceSummary`: Increment `LessonsCount`. |
| **AssessmentDelivery** | `AssessmentGraded` | `ClassPerformanceSummary`: Re-calculate `AverageScore`. `StudentProgressOverview`: Update individual grade. |
| **DiaryManagement** | `DiaryEntrySubmitted` | `ClassPerformanceSummary`: Increment `PendingDiaryEntries`. |
| **DiaryManagement** | `DiaryEntryApproved` | `ClassPerformanceSummary`: Decrement `PendingDiaryEntries`. |

## 3. High-Performance Aggregation Logic
To avoid race conditions and ensure accuracy, the consumers will follow these rules:

1.  **Idempotency**: Every consumer must check if the `EventId` has already been processed using an `Inbox` pattern or by checking a `LastEventId` column on the projection row.
2.  **Atomicity**: Updates to the `reporting` schema must be performed within a local transaction.
3.  **Aggregation Strategy**:
    - For averages (e.g., Average Score): Store `RunningSum` and `RunningCount` on the row, and calculate `Average = Sum / Count` on read (or pre-calculate on write).

## 4. Full Synchronization (Rebuild)
- **Trigger**: Scripted/Admin CLI.
- **Workflow**:
    - Clear the `reporting` tables.
    - Execute "Pull Queries" against source modules (via internal service references) to fetch current state.
    - Note: This bypasses the event bus for speed during initial migration or recovery.

## 5. Resilience & Dead Letters
- If a projection update fails (e.g., deadlock), MassTransit will retry.
- Persistent failures land in the `reporting_error` queue for manual inspection. Correcting the logic and re-trying ensures eventual consistency.
