# Specification: Google Drive Integration

## 1. Overview
- **Category**: Third-Party Service Integration
- **Parent Module**: GoogleIntegration
- **Feature Area**: Drive / File Management
- **Purpose**: Enable users to select and import files from Google Drive into the Lex `ObjectStorage` module.

## 2. Technical Strategy
- **Client-Side**: Use the `google-picker-api` to allow the user to browse their own Drive in a secure popup.
- **Backend-Side**: Receive the selected `FileId`, fetch the file metadata/bytes via the `v3/files` endpoint, and stream them into `ObjectStorage`.

## 3. Implementation Details

### Import Workflow
1.  **Selection**: The frontend triggers the Google Picker. The user selects a file.
2.  **Handoff**: The frontend sends the `GoogleFileId` to `POST /api/google/drive/import`.
3.  **Authentication**: The backend retrieves the `AccessToken` for the user from `GoogleAccountLink`.
4.  **Download**: The `GoogleDriveService` opens a stream to Google:
    - URL: `https://www.googleapis.com/drive/v3/files/{fileId}?alt=media`
5.  **Persistence**: The stream is passed directly to the `IObjectStorageService.UploadStreamAsync` method.
6.  **Cleanup**: A `FileUploadedEvent` is published, triggering standard processing (OCR, etc.).

### Export Workflow
1.  **Command**: `ExportToGoogleDriveCommand` with `LexFileId`.
2.  **Execution**: 
    - Fetch file from `ObjectStorage`.
    - Upload to a "Lex Workspace" folder on the user's Google Drive.

## 4. Configuration & Settings
- **Scopes**: `https://www.googleapis.com/auth/drive.readonly` (for picker) or `https://www.googleapis.com/auth/drive.file` (limited to files created by the app).
- **MIME Support**: Support conversion of Google Docs/Sheets to PDF/CSV via the `export` endpoint.

## 5. Resilience
- **Large Files**: Use chunked uploads/downloads for files > 5MB to handle network interruptions.
- **Quotas**: Implement rate limiting based on Google API usage headers.
