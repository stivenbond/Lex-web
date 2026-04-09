# Module Specification: GoogleIntegration

## 1. Overview
- **Name**: GoogleIntegration
- **Purpose**: Serve as the integration gateway between the Lex platform and Google Workspace services (Calendar, Drive), enabling teachers to synchronize their schedules and import external resources.
- **Schema**: `google_integration.*`
- **Primary Stakeholders**: Teachers, Admins.

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module.

- **Aggregate Root: GoogleAccountLink**: Manages the link between a Lex `UserId` and a Google Account, including encrypted OAuth tokens.
- **Entity: SyncLog**: Tracks individual synchronization operations (success/failure/retries).
- **Entity: DriveImportJob**: Tracks the progress of multi-file imports from Google Drive.

## 3. Module Responsibilities
- Implement the OAuth2 Authorization Code flow for per-user account linking.
- Manage secure token lifecycle (persistence, refresh, revocation).
- Synchronize scheduling slots and diary entries with Google Calendar.
- Provide a file-picker bridge for importing resources from Google Drive into `ObjectStorage`.
- Enforce resilience patterns (circuit breakers) to protect the monolith from Google API outages.

## 4. Integration & Dependencies
- **Inbound Events**: 
    - `SlotAssignedEvent`: Triggers a Calendar write.
    - `SlotRemovedEvent`: Triggers a Calendar delete/cancel.
- **Outbound Events**: 
    - `GoogleAccountLinkedEvent`
    - `GoogleImportCompletedEvent`
- **External Dependencies**: 
    - **Google Identity Service**: For OAuth.
    - **Google Calendar API**.
    - **Google Drive API**.

## 5. Security & Authorization
- **Permission Constants**: `GoogleIntegrationPermissions.cs`
- **Policies**: 
    - `CanLinkGoogleAccount`: Permission to initiate OAuth flow.
- **Token Protection**: Access and Refresh tokens MUST be encrypted at rest using the application's Data Protection keys.

## 6. Cross-Module Interactions
- **Scheduling**: Supplies the data for calendar synchronization.
- **ObjectStorage**: Receives files imported from Google Drive.
- **Lex.API**: Hosts the OAuth callback redirect URI.
