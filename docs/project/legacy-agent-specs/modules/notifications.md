# Module Specification: Notifications

## 1. Overview
- **Name**: Notifications
- **Purpose**: Unified system for alerting users across in-app and external channels.
- **Schema**: `notifications.*`
- **Primary Stakeholders**: All Users.

## 2. Domain Boundaries
- **Aggregate Root: Notification**: A single message intended for a user.
- **Entity: NotificationChannel**: Configuration and state for (InApp, Email, SMS).
- **Value Object: Template**: Reusable content structures for specific event types.

## 3. Module Responsibilities
- Subscribing to system-wide domain events and generating user-facing alerts.
- Distribution via SignalR (In-app) and SMTP (Email).
- Tracking read/unread status.

## 4. Integration & Dependencies
- **Inbound Events**: Subscribes to almost every `IDomainEvent` marked for notification.
- **Outbound Events**: None (Leaf module).
- **External Dependencies**: SignalR (Real-time), SMTP.

## 5. Security & Authorization
- **Permission Constants**: `NotificationPermissions.cs`
- **Policies**: `RequireNotificationOwner`.

## 6. Cross-Module Interactions
- **Host (SignalR)**: Uses `NotificationHub` to push messages to active clients.
- **SharedKernel**: Provides the event envelope for identity extraction.
