# Module Specification: Reporting

## 1. Overview
- **Name**: Reporting
- **Purpose**: Serve as the "Read Side" of the Modular Monolith, providing high-performance analytics and cross-module dashboards by maintaining a denormalized read model synchronized through domain events.
- **Schema**: `reporting.*` (Housed in the primary Postgres instance initially).
- **Primary Stakeholders**: Teachers (Class stats), Admins (Global usage), Students (Progress tracking).

## 2. Domain Boundaries
This module does not manage business transactions. It manages **Read Models** populated via Eventual Consistency.

- **Read Model: TeacherDashboard**: Consolidated view of lesson status, assessment results, and diary submissions for a teacher's classes.
- **Read Model: StudentProgress**: Unified view of a student's grades and activity across all subjects.
- **Read Model: UsageStats**: Aggregated metrics for institutional reporting (Time-series data).

## 3. Module Responsibilities
- Subscribe to specific domain events across all modules.
- Update denormalized projection tables in the `reporting.*` schema.
- Provide a simplified, query-optimized API for dashboards and charts.
- Maintain historical snapshots of metrics for trend analysis.
- Support "Full Rebuild" operations by re-consuming event history if necessary.

## 4. Integration & Dependencies
- **Inbound Events**: 
    - `LessonPublishedEvent`, `AssessmentGradedEvent`, `DiaryEntrySubmittedEvent`, etc.
- **Outbound Events**: None.
- **External Dependencies**: 
    - **Redis**: For caching frequently accessed dashboard summaries.

## 5. Security & Authorization
- **Permission Constants**: `ReportingPermissions.cs`
- **Policies**: 
    - `CanViewTeacherDashboards`: Scoped to classes taught by the user.
    - `CanViewAdminReports`: Global read access.

## 6. Cross-Module Interactions
- This module is logically dependent on every other module emitting domain events.
- It provides data to the frontend that would otherwise require complex, inefficient cross-DB queries.
