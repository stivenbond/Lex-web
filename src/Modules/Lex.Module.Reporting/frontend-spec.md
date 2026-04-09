# Frontend Specification: Reporting Dashboards & Charts

## 1. Overview
- **Parent Module**: Reporting
- **Target Pages**: 
    - `/dashboard` (Role-based landing page)
    - `/students/[id]/progress` (Detailed performance view)
    - `/admin/reports` (High-level usage stats)

## 2. Views & Components

### Teacher Dashboard
- **Function**: Provide a "Central Command" view for educators.
- **Components**:
    - **Class Cards**: Summary metrics (Avg Grade, % Lessons Completed).
    - **Grading Alerts**: Counter for pending assessments.
    - **Diary Feed**: Quick links to the latest submissions from students/supervisors.

### Progress Charts (Recharts)
- **Function**: Visualize time-series performance data.
- **Variants**:
    - **Grade Trend**: Line chart showing average grade over time.
    - **Attendance Bar**: Comparison of attendance across different subjects.
    - **Activity Heatmap**: Showing student engagement levels.

### Summary Tables
- **Function**: Detailed lists for drill-down.
- **Features**:
    - "Top 10 Performers" and "At-Risk Students" lists.
    - Click-through navigation from a dashboard card to the specific data source (e.g., clicking "3 Pending" opens the Assessment Grading queue).

## 3. Data Fetching & State
- **Store**: `useReportingStore` (Zustand).
- **Hooks**:
    - `useDashboardData()`: Fetches context-specific summary.
    - `useStudentTrends(id)`: Fetches data for the progress chart.
- **Optimization**: Use React Query (or similar) with a 5-minute stale-time, as reporting data is eventually consistent and doesn't require sub-second updates.

## 4. UI Library & Styling
- **Charts**: `recharts` for consistent, accessible SVG graphs.
- **Shared Components**: `Card` (for stats), `ProgressBar` (for goal completion).
- **Colors**: Use the semantic palette (e.g., Green for high performance, Amber for at-risk).

## 5. Security
- Use the `usePermissions()` hook to verify that the user is only viewing data they are authorized to see (e.g., Parents only see their child's trend).
- Mask PII (Personal Identifiable Information) in global admin reports.
