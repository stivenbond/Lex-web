# Domain Specification: User Notifications

## 1. Context
- **Module**: Notifications
- **Scope**: User attention management and cross-channel delivery logic.

## 2. Core Ubiquitous Language
- **Notification**: A message targeted at a unique User.
- **Channel**: The medium (InApp, Email).
- **Template**: The pre-defined wording and structure for a specific event.

## 3. Domain Model Hierarchy

- **Aggregate Root: Notification**
    - **Purpose**: Manages the content and read status for a user.
    - **Invariants**: 
        - Must have a target `UserId`.
        - Must have a `Category` (Success, Warning, Info, ActionRequired).

## 4. Delivery Logic
- **InApp (SignalR)**: Always sent immediately to the hub if the user is online.
- **Email (SMTP)**: Sent based on user preferences and event priority.

## 5. Domain Events
- **NotificationSentEvent**: Triggered when a message is successfully dispatched to all channels.
- **NotificationReadEvent**: Triggered when a user interacts with the message.

## 6. Business Operations (Conceptual)
- **Op: Dispatch**: Evaluates templates and user preferences to send a notification.
- **Op: MarkRead**: Updates the state and clears the bell count.
