# Frontend Specification: Import/Export Wizard & Dashboard

## 1. Overview
- **Parent Module**: ImportExport
- **Target Pages**: 
    - `/admin/import` (The Migration Wizard)
    - `/jobs` (User-wide background task dashboard)

## 2. Views & Components

### The Import Wizard (Step-by-Step)
- **Function**: Provide a user-friendly way to map complex Office files into Lex.
- **Steps**:
    1.  **Select Target**: Choose what is being imported (e.g. "Lesson Plans").
    2.  **Upload File**: Drag-and-drop Office document or CSV.
    3.  **Map Columns (Excel/CSV)**: A table where users drag and drop Excel headers to Lex fields.
    4.  **Preview & Validate**: Show a sample of 5 rows with validation status.
    5.  **Finalize**: Run the background job.

### Background Jobs Dashboard
- **Function**: Track long-running exports/imports.
- **Features**:
    - List of recent jobs with status badges.
    - Real-time progress bars using the `useAsyncJob` status hook.
    - "Download Result" button for completed exports.
    - "View Errors" modal for failed imports.

## 3. Data Fetching & State
- **Store**: `useImportExportStore` (Zustand).
- **Hooks**:
    - `useJobStatus(id)`: Polling/SignalR hook for real-time progress.
    - `useSchemaRequirements(domain)`: Fetches expected columns for the mapping wizard.

## 4. UI Library & Styling
- **Shared Components**: `Stepper` (for the wizard), `DataTable` (for mapping), `Progress` (for status).
- **Icons**: `HardDriveDownload` (Import), `FileExport` (Export).

## 5. Security
- Only Admins see the `/admin/import` page.
- CSRF protection: Every import request must include the session token.
