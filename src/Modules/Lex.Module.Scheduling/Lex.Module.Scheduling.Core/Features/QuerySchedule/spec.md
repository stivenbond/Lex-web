# Feature Specification: Scheduling Queries

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Read-only access to the timetable for various entities (Classes, Teachers, Dates).
- **Type**: Query
- **User Story**: As a User (Teacher/Student), I want to view my upcoming schedule so that I can prepare for classes.

## 2. Request Representation
- **Request Classes**:
    - `GetScheduleForClassQuery` { ClassId, StartDate, EndDate }
    - `GetScheduleForTeacherQuery` { TeacherId, StartDate, EndDate }
    - `GetPeriodsForDateQuery` { Date }
- **Validation Rules**: IDs must be valid. Date ranges should not exceed 3 months.

## 3. Business Logic (The Slice)
- **Trigger**: `REST GET /api/scheduling/schedule?classId=...` etc.
- **Logic**: Performs optimized SQL queries joining `slots` and `class_allocations`.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `scheduling.slots`, `scheduling.class_allocations`, `scheduling.periods`.
- **Repository Methods**: `GetClassScheduleAsync`, `GetTeacherScheduleAsync`.

## 5. Domain Objects (High-Level)
- **ScheduleDto**: Flattened representation of a slot with its allocation details.

## 6. API / UI Contracts
- **Route**: `GET /api/scheduling/schedule`
- **Response**: `Result<List<ScheduleDto>>`
- **UI Interaction**: Displayed in a weekly/daily timetable grid.

## 7. Security
- **Required Permission**: `SchedulingPermissions.ViewSchedule`
- **Auth Policy**: Users can only query their own schedule or their class's schedule (enforced in handler).
