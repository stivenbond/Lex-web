# Domain Specification: Notifications & Templates

## 1. Context
- **Module**: Notifications
- **Scope**: Lifecycle of user alerts and the management of their delivery channels and templates.

## 2. Core Ubiquitous Language
- **Notification**: A specific piece of information directed to a user.
- **Template**: A parameterized message structure with placeholders for domain data.
- **Channel**: The medium of delivery (In-App, Email).
- **Preference**: A user-defined rule for which notifications are sent via which channels.

## 3. Domain Model Hierarchy

- **Aggregate Root: Notification**
    - **Purpose**: Represent an individual alert.
    - **Invariants**:
        - Must have a target `UserId`.
        - Must have a `Type` (linked to a template).
        - Recorded `CreatedDate` and optional `ReadDate`.
- **Aggregate Root: NotificationTemplate**
    - **Purpose**: Centralize the definition of messages.
    - **Content**: Supports Liquid syntax for variable injection (e.g., `Hello {{teacherName}}, your lesson has been published.`).
- **Entity: DeliveryRecord**
    - **Purpose**: Tracks whether a notification was successfully pushed via SignalR or sent via SMTP.

## 4. Value Objects
- **NotificationType**: Enum (e.g., `LessonPublished`, `AssessmentSubmitted`, `DiaryReviewRequired`).
- **ChannelType**: Enum (`InApp`, `Email`).

## 5. Domain Events
- **NotificationSent**: Published when a notification is successfully queued for all requested channels.
- **NotificationRead**: Published when a user acknowledges a message in the UI.

## 6. Business Operations (Conceptual)
- **Op: Dispatch Notification**
    - **Logic**: 
        1. Resolve template by type.
        2. Inject variables from the domain event.
        3. Check recipient's preferences.
        4. Create `Notification` record and trigger `InApp` or `Email` adapters.
- **Op: Mark as Read**
    - **Logic**: Sets `ReadDate` and emits `NotificationRead`.
