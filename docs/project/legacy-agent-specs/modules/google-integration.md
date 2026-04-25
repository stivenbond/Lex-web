# Module Specification: GoogleIntegration

## 1. Overview
- **Name**: GoogleIntegration
- **Purpose**: Bridge between the platform and the Google Workspace ecosystem (Calendar, Drive).
- **Schema**: `google_integration.*`
- **Primary Stakeholders**: Teachers (syncing calendars), Admins (configuring OAuth).

## 2. Domain Boundaries
- **Aggregate Root: ExternalConnection**: Stores OAuth tokens and link status for a user.
- **Value Object: GoogleResource**: Reference to a file or event on the Google side.

## 3. Module Responsibilities
- Handling OAuth2 authorization code flows.
- Pushing events (like Slots) to Google Calendar.
- Importing documents from Google Drive.

## 4. Integration & Dependencies
- **Inbound Events**: 
    - `SlotAssignedEvent` (Scheduling): Triggers calendar write.
- **Outbound Events**:
    - `GoogleSyncCompletedEvent`
- **External Dependencies**: Google APIs (Calendar v3, Drive v3).

## 5. Security & Authorization
- **Permission Constants**: `GooglePermissions.cs`
- **Policies**: `RequireExternalLinkOwner`.

## 6. Cross-Module Interactions
- **Scheduling/Diary**: Provides the source data for sync.
- **ObjectStorage**: Receives files pulled from Drive.
