# API Specification: GoogleIntegration Module

## 1. Overview
- **Parent Module**: GoogleIntegration
- **Base Route**: `/api/google`
- **Purpose**: Expose endpoints for OAuth2 lifecycle management, manual sync triggers, and Drive import orchestration.

## 2. API Endpoints

### OAuth Lifecycle
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/auth/link` | - | Initiates the Google OAuth flow by returning the `AuthorizationUrl`. |
| `GET` | `/auth/callback` | `FinalizeGoogleLink` | The redirect URI that receives the `code` and stores tokens. |
| `DELETE` | `/auth` | `UnlinkGoogleAccount` | Removes the account link and revokes tokens. |

### Drive Integration
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/drive/import` | `ImportFromGoogleDrive` | Starts the background download of a Google Drive file into `ObjectStorage`. |
| `GET` | `/drive/picker-config` | - | Returns the configuration needed for the client-side Google Picker (API Key, Scopes). |

### Calendar & Control
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/sync/calendar` | `SyncCalendarNow` | Manually triggers a full sync of future events. |
| `GET` | `/status` | - | Returns connectivity status (e.g., "Linked", "Token Expired", "Google Unreachable"). |

## 3. Request/Response Shapes

### Resource: GoogleLinkStatusDto
```json
{
  "isLinked": true,
  "googleEmail": "teacher@gmail.com",
  "lastSyncAt": "iso-date",
  "isReady": true
}
```

## 4. Security & Authorization
- **State Check**: The `/auth/link` endpoint generates a secure `state` parameter to prevent CSRF during the OAuth flow.
- **Token Protection**: No access/refresh tokens are ever returned by these APIs. All operations are server-side using the encrypted keys in the database.

## 5. Integration with YARP
- Match: `/api/google/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
