# Feature Specification: Get Teacher Dashboard

## 1. Feature Overview
- **Parent Module**: Reporting
- **Description**: Retrieve a consolidated overview of class performance, pending tasks, and recent activity for a specific teacher.
- **Type**: Query
- **User Story**: As a Teacher, I want to see a single dashboard with all my class statuses so that I can prioritize grading and lesson planning.

## 2. Request Representation
- **Request Class**: `GetTeacherDashboardQuery`
- **Payload Structure**:
    - `TeacherId`: GUID (Implicit from context)
- **Validation Rules**: None.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/reporting/dashboards/teacher`
- **Domain Logic**:
    - Query the `reporting.class_performance_summaries` table for all classes where the user is an instructor.
    - Aggregates the data into a high-level DTO.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `reporting.class_performance_summaries`
- **Repository Methods**: `GetSummariesByTeacherIdAsync`

## 5. Domain Objects (High-Level)
- **TeacherDashboardDto**: Contains class list, average scores, and unread counts.

## 6. API / UI Contracts
- **Route**: `GET /api/reporting/dashboards/teacher`
- **Response**: `Result<TeacherDashboardDto>`
- **UI Interaction**: The main landing page for Teachers after login.

## 7. Security
- **Required Permission**: `ReportingPermissions.ViewTeacherDashboards`
- **Auth Policy**: `TeacherOnly` (Implicitly scoped to current ID).
