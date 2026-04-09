# Feature Specification: Update Notification Preferences

## 1. Feature Overview
- **Parent Module**: Notifications
- **Description**: Configure which types of notifications are delivered through which channels (In-App, Email).
- **Type**: Command
- **User Story**: As a User, I want to control my alert settings so that I don't get overwhelmed by emails for minor events.

## 2. Request Representation
- **Request Class**: `UpdateNotificationPreferencesCommand`
- **Payload Structure**:
    - `Preferences`: List of `{ Type: NotificationType, Channels: List<ChannelType> }`
- **Validation Rules**:
    - At least one channel should remain enabled for critical types (e.g., `AssessmentPublished`).

## 3. Business Logic (The Slice)
- **Trigger**: REST `PUT /api/notifications/preferences`
- **Domain Logic**:
    - Load the user's current `UserPreference` records.
    - Upsert the new configuration.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `notifications.user_preferences`
- **Repository Methods**: `UpsertPreferencesAsync`

## 5. Domain Objects (High-Level)
- **UserPreference**: Defines a (User, Type, Channels) mapping.

## 6. API / UI Contracts
- **Route**: `PUT /api/notifications/preferences`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Save Settings" button in the Notifications Preferences page.

## 7. Security
- **Required Permission**: `NotificationsPermissions.Receive`
- **Auth Policy**: `GeneralAccess`
