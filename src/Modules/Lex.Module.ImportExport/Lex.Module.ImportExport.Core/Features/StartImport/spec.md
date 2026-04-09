# Feature Specification: Start Bulk Import

## 1. Feature Overview
- **Parent Module**: ImportExport
- **Description**: Initiate a bulk data ingestion task from an uploaded Office or CSV file.
- **Type**: Command
- **User Story**: As an Admin, I want to import a spreadsheet of students so that I can quickly populate the platform with user accounts.

## 2. Request Representation
- **Request Class**: `StartImportCommand`
- **Payload Structure**:
    - `FileId`: GUID (The ID of the file uploaded to ObjectStorage).
    - `TargetDomain`: Enum (`UserAccounts`, `Grades`, `LessonPlan`, `Schedule`).
    - `MappingRules`: JSON (optional mapper configuration for Excel/CSV).
- **Validation Rules**:
    - `FileId` must exist and be accessible by the user.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/import`
- **Domain Logic**:
    - Create a new `ImportJob` aggregate in `Pending` status.
    - Validate that the file extension matches the expectations for the `TargetDomain`.
- **Side Effects**:
    - Dispatches the heavy processing to the background worker (`ImportWorker`).
    - API returns `202 Accepted` with the `JobId`.

## 4. Persistence
- **Affected Tables**: `import_export.import_jobs`
- **Repository Methods**: `AddAsync(ImportJob job)`

## 5. Domain Objects (High-Level)
- **ImportJob**: Aggregate root tracking the state machine.

## 6. API / UI Contracts
- **Route**: `POST /api/import`
- **Response**: `Result<Guid>` (The Job ID)
- **UI Interaction**: Triggered by the "Start Import" button in the Import Wizard.

## 7. Security
- **Required Permission**: `ImportExportPermissions.BulkImport`
- **Auth Policy**: `AdminOnly`
