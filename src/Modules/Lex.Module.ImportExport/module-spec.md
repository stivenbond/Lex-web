# Module Specification: ImportExport

## 1. Overview
- **Name**: ImportExport
- **Purpose**: Serve as the platform's primary data orchestration engine, managing the lifecycle of bulk imports and exports. It handles complex format transformations (e.g., DOCX to Lex BlockContent) and schema mapping.
- **Schema**: `import_export.*`
- **Primary Stakeholders**: Admins, Teachers.

## 2. Domain Boundaries
- **Aggregate Root: ImportJob**: Tracks the state machine of a bulk data ingestion task.
- **Aggregate Root: ExportJob**: Tracks the generation and delivery of a bulk data document.
- **Value Object: MappingConfig**: Defines how external columns/structures correlate to internal Lex domain properties.

## 3. Module Responsibilities
- Provide a robust state machine for long-running asynchronous jobs.
- Transform Microsoft Office formats (DOCX, XLSX) into Lex internal models (`BlockContent` and relational schemas).
- Orchestrate dependencies between `ObjectStorage` (source files) and `FileProcessing` (initial extraction).
- Provide structural validation for incoming data before persistence.
- Manage "Result Packages" (zipped reports or exported lessons).

## 4. Integration & Dependencies
- **Inbound Events**: None (Job initiation is command-driven).
- **Outbound Events**: 
    - `ImportCompletedEvent`: Notifies target modules (e.g., Diary, Lesson) that new data is ready for their schema.
    - `ExportReadyEvent`: Notifies the user with a download link.
- **External Dependencies**: 
    - **OpenXML SDK / NPOI**: For parsing Microsoft Office documents.
    - **GoogleDriveService**: For direct cloud imports (see Phase 10).

## 5. Security & Authorization
- **Permission Constants**: `ImportExportPermissions.cs`
- **Policies**: 
    - `CanBulkImport`: Admin-only or specialized supervisor role.
    - `CanExportPersonalData`: Scoped to the user's authored content.

## 6. Cross-Module Interactions
- **ObjectStorage**: Source and destination for all job-related files.
- **FileProcessing**: Initial extraction for PDFs.
- **Domain Modules (Lesson/Diary/Identity)**: Receives the final mapped data for persistence in their own schemas.
