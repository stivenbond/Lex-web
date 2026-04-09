# Frontend Specification: Google Integration Components

## 1. Overview
- **Parent Module**: GoogleIntegration
- **Target Components**: 
    - `GoogleLinkCard`: Displayed in user settings.
    - `GoogleDrivePicker`: Modal for selecting resources.
    - `SyncStatusBanner`: Notification area for sync failures.

## 2. Views & Components

### Google Account Linking (Settings)
- **Function**: Manage the connection between Lex and Google.
- **Features**:
    - "Connect Google Workspace" button (OAuth initiation).
    - "Unlink" button + Confirmation dialog.
    - Status display: "Connected as [email]" and "Last synced [time]".

### Google Drive Picker Modal
- **Function**: Client-side browsing of Drive.
- **Features**:
    - **Trigger**: Click "Import from Drive" in Lesson Editor or File Library.
    - **Modal**: Embeds the official Google Picker UI.
    - **Callback**: Once the user selects a file, the `FileId` is passed back to the Lex UI, which then triggers the background import command.

### Sync Indicators
- **Function**: Provide visibility into background tasks.
- **Features**:
    - "Syncing with Calendar..." status in the Scheduling module footer.
    - Toast notifications for successful Imports.

## 3. Data Fetching & State
- **Store**: `useGoogleIntegrationStore` (Zustand).
- **Hooks**:
    - `useGoogleLink()`: Returns connectivity status.
    - `useGoogleDriveImport()`: Mutation to trigger the import command.
- **External Scripts**:
    - Dynamically load `https://accounts.google.com/gsi/client` and `https://apis.google.com/js/api.js` only when needed.

## 4. UI Library & Styling
- **Shared Components**: `Card` (Settings), `Dialog` (Confirmation), `Alert` (Error scenarios).
- **Icons**: Official Google Icons for Drive and Calendar.

## 5. Security
- **Strict Content Security Policy (CSP)**: Ensure the CSP allows communication with `*.google.com` and `*.googleapis.com`.
- **Credential Storage**: No OAuth tokens or Client Secrets are stored in the frontend. All auth is handled via the backend's Secure Link.
