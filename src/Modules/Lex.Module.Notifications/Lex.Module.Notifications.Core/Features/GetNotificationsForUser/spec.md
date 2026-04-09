# Feature Specification: Get Notifications for User

## 1. Feature Overview
- **Parent Module**: Notifications
- **Description**: Retrieve a paged list of notifications for the current authenticated user.
- **Type**: Query
- **User Story**: As a User, I want to see my notification history so that I can review past alerts.

## 2. Request Representation
- **Request Class**: `GetNotificationsForUserQuery`
- **Payload Structure**:
    - `Page`: Integer
    - `PageSize`: Integer
    - `UnreadOnly`: Boolean
- **Validation Rules**:
    - `PageSize` max 100.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/notifications`
- **Domain Logic**:
    - Query `notifications.notifications` table for the current `UserId`.
    - Apply sorting (Newest first).
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `notifications.notifications`
- **Repository Methods**: `GetPagedByUserIdAsync`

## 5. Domain Objects (High-Level)
- **NotificationDto**: Contains ID, Type, Content (rendered), CreatedDate, and ReadStatus.

## 6. API / UI Contracts
- **Route**: `GET /api/notifications?unreadOnly=true&page=1`
- **Response**: `Result<PagedList<NotificationDto>>`
- **UI Interaction**: Primary data source for the bell drawer and the full "Notifications" page.

## 7. Security
- **Required Permission**: `NotificationsPermissions.Receive`
- **Auth Policy**: `GeneralAccess`
