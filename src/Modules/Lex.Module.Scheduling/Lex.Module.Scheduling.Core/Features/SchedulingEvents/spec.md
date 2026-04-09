# Feature Specification: Scheduling Events

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Definitions for events published by the Scheduling module for internal and external consumption.
- **Type**: Event Consumer | Published Event
- **User Story**: N/A (Integration concern)

## 2. Request Representation
- **Event Classes**:
    - `AcademicYearCreatedEvent` { YearId, Name }
    - `SlotAssignedEvent` { SlotId, ClassId, TeacherId, Date, PeriodId }
    - `SlotRemovedEvent` { SlotId, ClassId, TeacherId }

## 3. Business Logic (The Slice)
- **Publication**: Triggered inside handlers (e.g., `AssignClassToSlotHandler`) after a successful DB commit.
- **Consumption**:
    - `DiaryManagement` module consumes `SlotAssignedEvent` to prepare content placeholders.
    - `Notifications` module may consume these to notify teachers of schedule changes.

## 4. Persistence
N/A (Events are transient messages).

## 5. Domain Objects (High-Level)
- **IDomainEvent**: Base interface implementation.

## 6. API / UI Contracts
- **Transport**: MassTransit / RabbitMQ.

## 7. Security
- **Access**: Only Scheduling module services have permission to publish these events.
