# Feature Specification: Start Bulk Export

## 1. Feature Overview
- **Parent Module**: ImportExport
- **Description**: Package Lex platform data (e.g., Lesson Plans, Grade Reports) into a downloadable Office document or ZIP archive.
- **Type**: Command
- **User Story**: As a Teacher, I want to export my lesson plan to Word so that I can provide a physical copy to my supervisor or archive it locally.

## 2. Request Representation
- **Request Class**: `StartExportCommand`
- **Payload Structure**:
    - `TargetDomain`: Enum (`LessonPlan`, `Grades`, `Attendance`)
    - `Filters`: JSON (e.g. `LessonId`, `DateRange`)
    - `Format`: Enum (`Docx`, `Xlsx`, `Pdf`)
- **Validation Rules**:
    - `TargetDomain` must be supported.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/export`
- **Domain Logic**:
    - Initialize a new `ExportJob` record.
    - Query the source module for the required data.
    - Transform the data using the `ExportEngine`.
    - Generate the final binary file.
- **Side Effects**:
    - The generated file is uploaded to `ObjectStorage` (Temporary area).
    - API returns `202 Accepted` with the `JobId`.

## 4. Persistence
- **Affected Tables**: `import_export.export_jobs`
- **Repository Methods**: `AddAsync(ExportJob job)`

## 5. Domain Objects (High-Level)
- **ExportJob**: State machine for file generation.

## 6. API / UI Contracts
- **Route**: `POST /api/export`
- **Response**: `Result<Guid>` (The Job ID)
- **UI Interaction**: Triggered by the "Download as..." menu on various resource pages.

## 7. Security
- **Required Permission**: `ImportExportPermissions.ExportData`
- **Auth Policy**: `GeneralAccess` (Scoped to self-authored content).
