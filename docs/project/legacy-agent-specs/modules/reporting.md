# Module Specification: Reporting

## 1. Overview
- **Name**: Reporting
- **Purpose**: Consolidating data across module boundaries for dashboards, summaries, and analytics.
- **Schema**: `reporting.*`
- **Primary Stakeholders**: Admins, Teachers.

## 2. Domain Boundaries
- **Aggregate Root: ReportDefinition**: Metadata defining a report's structure and filters.
- **Value Object: ReadModel**: Denormalized tables populated by cross-module events.

## 3. Module Responsibilities
- Implementing the "Read Model" pattern.
- Providing complex views that would otherwise require forbidden cross-schema joins.
- Generating CSV/PDF exports of consolidated data.

## 4. Integration & Dependencies
- **Inbound Events**: Subscribes to events from `Scheduling`, `Diary`, `AssessmentDelivery`, etc.
- **Outbound Events**: None.
- **External Dependencies**: Uses PDF generation libraries.

## 5. Security & Authorization
- **Permission Constants**: `ReportingPermissions.cs`
- **Policies**: `RequireReportViewer`.

## 6. Cross-Module Interactions
- **Strict Isolation**: Never queries other module schemas. It must "wait" for data to arrive via the message bus or initial sync jobs.
