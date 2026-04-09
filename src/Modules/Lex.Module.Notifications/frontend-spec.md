# Frontend Specification: Notifications UI Components

## 1. Overview
- **Parent Module**: Notifications
- **Target Components**: 
    - `NotificationBell`: Present in the main app shell.
    - `NotificationDrawer`: Slide-over for quick review.
    - `NotificationIndex`: Full page history.
    - `NotificationSettings`: Preference management.

## 2. Views & Components

### Notification Bell & Drawer
- **Function**: Provide immediate visibility of new alerts.
- **Features**:
    - **Badge**: Dynamic numeric indicator for unread count.
    - **List Item**: Renders each notification with an icon (based on type), message, and relative time (e.g., "5m ago").
    - **Actions**: "Mark as read" icon, "View All" link.

### Real-Time Update Strategy
- **SignalR Hub**: `/hubs/notifications`.
- **Workflow**:
    - On mount, `LexAppShell` connects to the hub.
    - When a `NewNotification` event is received, the `NotificationStore` is updated, and a toast/notification appears if the user is on a different page.

### Preferences Page
- **Function**: Manage channel settings.
- **Features**:
    - A table/list of notification categories (Diary, Assessments, System).
    - Toggle switches for "In-App" and "Email" channels for each category.

## 3. Data Fetching & State
- **Store**: `useNotificationStore` (Zustand).
- **Hooks**:
    - `useNotifications()`: Returns paged list and provides `markAsRead` functions.
    - `useUnreadCount()`: Reactive count from SignalR.

## 4. UI Library & Styling
- **Shared Components**: `Sheet` (for drawer), `Badge` (for count), `Switch` (for preferences).
- **Icons**: `Bell` (Bell), `Mail` (Email), `Circle` (Unread indicator).

## 5. Security
- Use the `usePermissions()` hook to hide categories that the user doesn't have access to (e.g., "Diary Review" for students).
