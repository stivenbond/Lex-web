# Feature Specification: Mark Notification as Read

## 1. Feature Overview
- **Parent Module**: Notifications
- **Description**: Mark an individual notification or all notifications for a user as "Read".
- **Type**: Command
- **User Story**: As a User, I want to clear my notifications so that I can stay focused on new alerts.

## 2. Request Representation
- **Request Class**: `MarkNotificationReadCommand`
- **Payload Structure**:
    - `Id`: GUID (Optional, if null then "Mark All as Read")
- **Validation Rules**:
    - `Id` must exist and belong to the current user if provided.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/notifications/{id}/read` or `POST /api/notifications/read-all`
- **Domain Logic**:
    - Load the target `Notification`(s).
    - Set `ReadDate = DateTimeOffset.UtcNow`.
- **Side Effects**:
    - Publish `NotificationRead` event.
    - Push an "Unread Count Update" via SignalR to synchronization the UI badges.

## 4. Persistence
- **Affected Tables**: `notifications.notifications`
- **Repository Methods**: `GetByIdAsync`, `UpdateRangeAsync`

## 5. Domain Objects (High-Level)
- **Notification**: Entity updated with read status.

## 6. API / UI Contracts
- **Route**: `POST /api/notifications/{id}/read`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered when a user clicks a notification in the bell drawer or clicks "Mark all as read".

## 7. Security
- **Required Permission**: `NotificationsPermissions.Receive`
- **Auth Policy**: `GeneralAccess` (implicitly scoped to the logged-in user).
