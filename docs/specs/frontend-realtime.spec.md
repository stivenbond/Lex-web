# Specification: Frontend Real-Time Infrastructure (SignalR)

## 1. Overview
- **Category**: Real-Time Communication
- **Scope**: Lifecycle management of WebSocket connections with the .NET SignalR Hubs.

## 2. Connection Manager (`lib/signalr/connectionManager.ts`)
- **Singleton Pattern**: Managed as a persistent singleton after initial authentication.
- **Lifecycle**:
    - **Connect**: Started after successful login.
    - **Token Factory**: On every (re)connect, call `tokenManager.getAccessToken()` to ensure the newest bearer token is presented.
    - **Auto-Reconnect**: Configured with exponential backoff (`0s, 2s, 10s, 30s`).
    - **Monitoring**: Reports status (Connecting, Connected, Disconnected) to `useSignalRStore`.

## 3. SignalR Provider (`components/providers/SignalRProvider.tsx`)
- **Duty**: Wraps the `(app)` route group to ensure hubs are active for authenticated users.
- **Implementation**:
    - Initializes the `NotificationsHub` and `JobStatusHub` connections.
    - Provides a React Context for child components to access raw connection objects if needed.

## 4. The Async Job Hook (`hooks/useAsyncJob.ts`)
- **Workflow**:
    1.  Receives a `jobId` from a REST mutation (202 Accepted).
    2.  Subscribes to the `JobStatusHub`.
    3.  Filters incoming `JobStatusUpdated` events for the matching `jobId`.
    4.  Exposes: `status` (Pending/Running/Completed), `progress` (0-100), and `error`.
- **Cache Invalidation**: Upon `JobCompleted`, automatically invalidates the relevant TanStack Query caches to trigger a fresh data fetch.

## 5. Hub Event Definitions
- **`NotificationHub`**: Pushes new in-app alerts and increments unread counters.
- **`DiarySyncHub`**: Pushes updates when a shared diary entry is modified.
- **`DiagramSyncHub`**: Pushes React Flow coordinate updates for collaborative visualization.

## 6. Resilience
- **Re-Sync on Reconnect**: When SignalR reconnects after a drop, it automatically invalidates all active TanStack Query queries (forcing a re-fetch) to ensure no data was missed during the outage.
