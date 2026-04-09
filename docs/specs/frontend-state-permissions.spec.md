# Specification: Frontend State & Permissions

## 1. Overview
- **Category**: State Management / UI Authorisation
- **Scope**: Managing global application state (Zustand) and providing a clean interface for permission-based UI pruning.

## 2. Global Stores (`lib/store/`)

### `useAuthStore`
- **Duty**: Hold the current session's identity and status.
- **Properties**:
    - `user`: Identity object (Name, Email, Avatar).
    - `roles`: List of role claims from JWT.
    - `status`: `Unauthenticated | Authenticating | Authenticated`.
- **Invariants**: Cleared immediately on sign-out. Updated automatically when a token is refreshed.

### `useSignalRStore`
- **Duty**: Track connection health for all hubs.
- **Properties**:
    - `isConnected`: Boolean.
    - `reconnectCount`: Integer.
    - `lastError`: Message.

### `useUIStore`
- **Duty**: Transient non-domain layout state.
- **Properties**:
    - `sidebarCollapsed`: Boolean.
    - `theme`: `Light | Dark | System`.
    - `notificationsOpen`: Boolean.

## 3. Permissions Hook (`hooks/usePermissions.ts`)
- **Duty**: Bridge between JWT role claims and frontend visibility logic.
- **Interface**: `usePermissions(): { hasRole(role: string): boolean, isTeacher(): boolean, can(permission: string): boolean }`
- **Logic**: 
    - Loads the current `roles` from `useAuthStore`.
    - Returns boolean flags based on institutional role mappings (e.g., `Teacher`, `Supervisor`).

## 4. Permission Constants (`lib/auth/permissions.ts`)
- **Philosophy**: This file MUST mirror the `Constants` files in the .NET module cores to ensure naming consistency.
- **Examples**: 
    - `DiaryPermissions.Review`
    - `AssessmentPermissions.Grade`

## 5. UI Application Strategy
- **Pruning**: Use the `can()` check to conditionally render buttons, nav items, and sensitive columns in tables.
- **Redirection**: Higher-level route guarding is handled by `middleware.ts`, but this hook provides the "in-page" experience (e.g., showing an "Access Denied" empty state instead of a broken page).

## 6. Persistence
- **Rules**:
    - `useUIStore` may be persisted to `localStorage` (theme, sidebar status).
    - `useAuthStore` MUST NEVER be persisted to `localStorage`.
