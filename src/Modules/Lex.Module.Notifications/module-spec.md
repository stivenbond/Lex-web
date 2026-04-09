# Module Specification: Notifications

## 1. Overview
- **Name**: Notifications
- **Purpose**: Serve as the communication hub for the platform, transforming domain events into user-actionable alerts delivered via in-app real-time pushes and email.
- **Schema**: `notifications.*`
- **Primary Stakeholders**: All authenticated users.

## 2. Domain Boundaries
- **Aggregate Root: Notification**: Represents an individual message sent to a specific user.
- **Aggregate Root: NotificationTemplate**: Manages the content and variables for different types of alerts.
- **Value Object: UserPreference**: Defines which channels (In-App, Email) a user wants for specific alert types.

## 3. Module Responsibilities
- Monitor the system-wide message bus for significant domain events.
- Match events against notification rules and templates.
- Orchestrate real-time delivery via SignalR for active users.
- Manage asynchronous email delivery for offline notifications or high-importance alerts.
- Track read/unread status for all in-app notifications.

## 4. Integration & Dependencies
- **Inbound Events**: Consumes events from `Scheduling`, `DiaryManagement`, `AssessmentCreation`, etc. (See mapping spec).
- **Outbound Events**: None.
- **External Dependencies**: 
    - **Redis**: For SignalR backplane and real-time push.
    - **SMTP/SendGrid/SES**: For email delivery.

## 5. Security & Authorization
- **Permission Constants**: `NotificationsPermissions.cs`
- **Policies**: 
    - `CanManageTemplates`: Admin-only right to edit notification content.
    - `CanReceiveNotifications`: Default right for all authenticated users.

## 6. Cross-Module Interactions
- **All Modules**: This module is a "Sink" for events produced across the monolith. It provides the final visibility layer for business processes initiated elsewhere.
