# Feature Specification: Get Student Progress

## 1. Feature Overview
- **Parent Module**: Reporting
- **Description**: Provide a unified view of an individual student's performance, attendance trends, and recent feedback.
- **Type**: Query
- **User Story**: As a Parent, I want to see my child's progress report so that I can support their learning in areas where they are struggling.

## 2. Request Representation
- **Request Class**: `GetStudentProgressQuery`
- **Payload Structure**:
    - `StudentId`: GUID
- **Validation Rules**:
    - `StudentId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/reporting/students/{id}/progress`
- **Domain Logic**:
    - Query `reporting.student_progress_overviews` and `reporting.historical_metrics`.
    - Join with recent activity logs from the same schema.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `reporting.student_progress_overviews`, `reporting.historical_metrics`
- **Repository Methods**: `GetOverviewByStudentIdAsync`

## 5. Domain Objects (High-Level)
- **StudentProgressDto**: Nested DTO with stats and time-series data for charts.

## 6. API / UI Contracts
- **Route**: `GET /api/reporting/students/{id}/progress`
- **Response**: `Result<StudentProgressDto>`
- **UI Interaction**: Displayed in the student profile page or parent mobile app.

## 7. Security
- **Required Permission**: `ReportingPermissions.ViewStudentProgress`
- **Auth Policy**: `ParentOrTeacher` (Scoped via relationship check in Core).
