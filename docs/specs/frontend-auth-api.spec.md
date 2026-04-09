# Specification: Frontend Authentication & API Layer

## 1. Overview
- **Category**: Infrastructure / Security
- **Scope**: Lifecycle management of OIDC tokens, API request orchestration, and route-level authorization.

## 2. Token Management (`lib/auth/tokenManager.ts`)
- **Philosophy**: Extreme security. Access tokens are transient and stored only in memory.
- **Rules**:
    - **Storage**: Module-level variable in a non-exported context.
    - **Refresh Token**: Stored in a `httpOnly`, `Secure`, `SameSite=Strict` cookie (set by the backend during the OAuth callback).
    - **Silent Refresh**: On page load (or 30 seconds before expiry), the `tokenManager` calls `/api/auth/refresh` to obtain a new `accessToken` using the cookie.

## 3. The API Fetch Wrapper (`lib/api/client.ts`)
- **Interface**: `apiFetch<T>(url: string, options: RequestInit): Promise<T>`
- **Logic**:
    1.  **Token Attachment**: Call `tokenManager.getAccessToken()`. If null, trigger refresh.
    2.  **Request**: Perform `fetch(url)`.
    3.  **Error Normalization**: 
        - If `response.status === 401`: Trigger refresh and retry ONCE.
        - If still 401: Redirect to `/login`.
        - If `response.status === 422`: Throw `ValidationError` containing the .NET `ProblemDetails` payload.
        - If `response.status >= 500`: Throw `ServerError`.

## 4. Auth Middleware (`middleware.ts`)
- **Location**: Project root.
- **Workflow**:
    1.  **Public Paths Check**: Bypass for `/login`, `/auth/callback`, `/public/*`.
    2.  **Session Check**: Look for the `session` existence cookie.
    3.  **Redirect**: If unauthenticated, redirect to Keycloak login URI (passing the current path as `redirect_uri`).

## 5. Claim Extraction (`lib/auth/claims.ts`)
- **Duty**: Decode the JWT access token to extract `roles`, `sub` (UserId), and `orgId`.
- **Zustand Integration**: The `useAuthStore` updates when a new token is stored, exposing `user` and `isAuthenticated`.

## 6. Type Safety
- Every API endpoint has a corresponding TypeScript interface in `lib/types/`.
- All `apiFetch` calls are generic: `apiFetch<InvoiceDto>('/api/billing/...');`
