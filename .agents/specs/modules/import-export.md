# Module Specification: ImportExport

## 1. Overview
- **Name**: ImportExport
- **Purpose**: Massive data ingestion and extraction for interoperability with other SIS/LMS systems.
- **Schema**: `import_export.*`
- **Primary Stakeholders**: Admins.

## 2. Domain Boundaries
- **Aggregate Root: DataJob**: Tracks the orchestration of a large-scale import or export.
- **Entity: DataMapping**: Rules for converting external CSV/JSON to internal domain models.

## 3. Module Responsibilities
- Processing high-volume CSV/Excel files.
- Orchestrating multi-module writes (e.g., creating 1000 slots and 50 classes at once).
- Exporting full system states for backup or migration.

## 4. Integration & Dependencies
- **Inbound Events**: None.
- **Outbound Events**:
    - `ImportCompletedEvent`
- **External Dependencies**: SignalR (to report progress %).

## 5. Security & Authorization
- **Permission Constants**: `ImportPermissions.cs`
- **Policies**: `RequireImportAdmin`.

## 6. Cross-Module Interactions
- **Multiple Modules**: Calls Commands in `Scheduling`, `AssessmentCreation`, etc., to execute the data load.
