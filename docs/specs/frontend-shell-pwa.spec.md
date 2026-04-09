# Specification: Frontend App Shell & PWA Compliance

## 1. Overview
- **Category**: UI Shell / Application Capability
- **Scope**: Layout architecture and standalone "Mobile-First" PWA features.

## 2. App Shell (`components/layout/`)
- **Structure**:
    - **Sidebar**: High-level Navigation (Server Component where possible).
    - **Header**: User breadcrumbs, Search, Notification Bell, and Auth Menu.
    - **Main Content**: Scrollable area with responsive padding.
- **Layout Logic**:
    - Root `layout.tsx`: Global Providers (TanStack, SignalR, Toast).
    - App `layout.tsx`: Authenticated shell + Navigation Guard.

## 3. Navigation
- **Architecture**: Defined as a configuration object in `lib/nav/config.ts`.
- **Visibility**: Items are filtered through the `usePermissions()` hook before rendering.

## 4. PWA Manifest (`public/manifest.webmanifest`)
- **Philosophy**: Enable "Add to Home Screen" on all platforms.
- **Properties**:
    - `name`: "Lex Education Platform"
    - `display`: `standalone` (Removes browser address bar).
    - `start_url`: `/dashboard`.
    - `background_color`: Institutional Brand (defaulting to dark slate).
    - `icons`: Comprehensive set (192, 512, maskable).

## 5. Service Worker (`public/sw.js`)
- **Framework**: Managed via `next-pwa` (Workbox).
- **Caching Strategy (Stale-While-Revalidate)**:
    - **Assets**: Cache-first for `.js`, `.css`, and `.webp` chunks.
    - **Pages**: Network-first for HTML routes.
    - **API**: NO CACHING for `/api/*` to ensure authenticated data is never stale or leaked across sessions.
- **Offline Fallback**: Serve a custom `/offline` page if the network is down and the user navigates.

## 6. Notification Center
- **Tray**: A slide-over component (`Sheet`) displaying the history of unread alerts.
- **Bell Icon**: Real-time counter badge linked to `useSignalRStore`.
- **Toasts**: Non-disruptive notifications for "Success" or "Info" events (e.g., "Import Job Finished").

## 7. Responsive Design
- **Breakpoint Rules**:
    - Mobile (< 768px): Sidebar becomes a hamburger menu / drawer.
    - Large Screen: Sidebar is fixed and persistent.
