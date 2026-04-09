# Domain Specification: Google Account Linking & Sync States

## 1. Context
- **Module**: GoogleIntegration
- **Scope**: Management of third-party account relationships, secure token storage, and synchronization state tracking.

## 2. Core Ubiquitous Language
- **AccountLink**: The persistent association between a Lex User and a Google Identity.
- **TokenVault**: The secure storage mechanism for OIDC tokens.
- **RefreshFlow**: The automatic logic to obtain a new AccessToken when the current one expires.
- **Provider**: The specific Google service (e.g., "Calendar", "Drive").

## 3. Domain Model Hierarchy

- **Aggregate Root: GoogleAccountLink**
    - **Purpose**: Manage the OIDC relationship and tokens.
    - **Invariants**:
        - `UserId` must be unique in this collection (One-to-One).
        - `RefreshToken` must be present for the link to be `Active`.
        - `Email` (Google-side) is stored for display purposes but not as a primary key.
- **Entity: SyncOperation**
    - **Purpose**: Tracks a single background task (e.g., "Pushing Lecture to Calendar").
    - **Properties**: `SourceEventId`, `Provider`, `Status` (Pending, Success, Failed, Retrying).

## 4. Value Objects
- **OAuthToken**: Encapsulates `AccessToken`, `RefreshToken`, `ExpiresAt`, and `Scope`.
- **EncryptedBlob**: Represents a string that has been processed by the `IDataProtector` service.

## 5. Domain Events
- **GoogleAccountLinked**: Published after successful OAuth callback validation.
- **GoogleTokenRevoked**: Published if a `401 Unauthorized` is received and refresh fails.
- **GoogleSyncFailed**: Published for monitoring/reporting if an integration error persists.

## 6. Business Operations (Conceptual)
- **Op: Initialize Link**
    - **Logic**: Store the initial tokens obtained from the redirect URI.
- **Op: Refresh Token**
    - **Logic**: Use the `RefreshToken` to update the aggregate state with a new `AccessToken`.
- **Op: Sync Event to Calendar**
    - **Invariants Checked**: Link must be `Active`. Circuit breaker must be `Closed`.
