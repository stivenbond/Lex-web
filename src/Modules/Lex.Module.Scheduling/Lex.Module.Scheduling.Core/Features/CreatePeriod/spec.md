# Feature Specification: Create Period

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Define a standard time block for the daily timetable.
- **Type**: Command
- **User Story**: As an Admin, I want to define school periods (e.g., Period 1, Period 2) so that I can structure the daily schedule.

## 2. Request Representation
- **Request Class**: `CreatePeriodCommand`
- **Payload Structure**: 
    - `Name` (string): e.g. "Period 1"
    - `StartTime` (TimeOnly)
    - `EndTime` (TimeOnly)
- **Validation Rules**: `StartTime` must be before `EndTime`.

## 3. Business Logic (The Slice)
- **Trigger**: `REST POST /api/scheduling/periods`
- **Domain Logic**: 
    - Verify no overlap with existing periods (optional policy).
    - Create `Period` entity.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `scheduling.periods`
- **Repository Methods**: `AddAsync(Period period)`.

## 5. Domain Objects (High-Level)
- **Period**: Master entity defining daily time blocks.

## 6. API / UI Contracts
- **Route**: `POST /api/scheduling/periods`
- **Response**: `Result<Guid>` (The Period ID).

## 7. Security
- **Required Permission**: `SchedulingPermissions.ManageCalendar`
- **Auth Policy**: `RequireAdmin`
