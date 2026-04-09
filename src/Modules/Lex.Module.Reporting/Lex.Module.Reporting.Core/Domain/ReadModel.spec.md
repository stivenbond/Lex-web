# Domain Specification: Reporting Read Models

## 1. Context
- **Module**: Reporting
- **Scope**: Design of denormalized tables optimized for analytical queries and dashboards.

## 2. Core Ubiquitous Language
- **Projection**: The process of transforming a sequence of domain events into a current state table.
- **ReadModel**: A data structure designed specifically for reading, not for serving as a source of truth for business logic.
- **Snapshot (Metric)**: A recorded value of a metric at a specific point in time (e.g., "Weekly Average Grade").

## 3. Read Model Entities

### Entity: ClassPerformanceSummary
- **Purpose**: High-level overview of a class's progress.
- **Properties**:
    - `ClassId`: GUID
    - `SubjectId`: GUID
    - `AverageAssessmentScore`: Decimal
    - `CompletedLessonsCount`: Integer
    - `PendingDiaryEntriesCount`: Integer
    - `LastUpdated`: DateTime

### Entity: StudentProgressOverview
- **Purpose**: Student-centric view for parents and teachers.
- **Properties**:
    - `StudentId`: GUID
    - `OverallGrade`: Decimal
    - `AttendancePercentage`: Decimal
    - `RecentActivities`: JSON (List of top 5 recent events)

### Entity: HistoricalMetric
- **Purpose**: Track trends over time.
- **Properties**:
    - `MetricKey`: String (e.g., "SystemAverageScore")
    - `Value`: Decimal
    - `Date`: Date (Frequency: Daily/Weekly)

## 4. Technical Strategy
- **Isolation**: These tables belong to the `reporting` schema. No other module should write to them.
- **Persistence**: Managed via EF Core, but with a focus on indexed `SELECT` performance.
- **Aggregation**: Complex counts/averages are pre-calculated by the event consumers and stored as simple numeric properties.

## 5. Lifecycle
- **Initialization**: On deploy, these tables are populated from the current state of other modules (via an initial migration or sync job).
- **Maintenance**: Incremental updates triggered by Domain Events.
- **Full Rebuild**: If the schema changes significantly, a background task can re-hydrate the entire schema from source module databases (guaranteeing eventual consistency).
