# Infrastructure Specification: Frontend API & Auth Client

## 1. Overview
- **Category**: Frontend Foundational Client
- **Parent Module**: Client / Web
- **Component Name**: LexApiClient & TokenManager

## 2. Technical Strategy
- **Base Technology**: Fetch API, Axios (optional), Zustand for state.
- **Protocol**: REST + SignalR.

## 3. Implementation Details

- **`apiFetch` Wrapper**:
    - Automatically attaches `Authorization: Bearer [token]` header.
    - Intercepts 401 Unauthorized to trigger a silent refresh via the `TokenManager`.
    - Normalizes `ProblemDetails` responses into typed TypeScript errors.
- **`TokenManager`**:
    - Manages the In-Memory Access Token.
    - Handles the HTTP-Only Refresh Cookie lifecycle.
    - Exposes an `isLoggedIn` state to the UI.

## 4. Configuration & Settings
- **Base URL**: Resolved from environment variable `VITE_API_URL`.
- **Client ID**: `lex-web-client`.

## 5. Resilience & Performance
- **Retries**: Retries idempotent requests (GET) on transient network failure.
- **Silent Refresh**: Triggers refresh 30 seconds before access token expiry.

## 6. Integration Points
- **Zustand `useAuthStore`**: Reactive state for the current user's profile and permissions.
- **Middleware**: React Router guard that checks auth state before rendering protected routes.
