# Feature Specification: Get Daily Schedule

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Retrieves the list of slots for a specific date and target (Class or Teacher).
- **Type**: Query
- **User Story**: As a Teacher, I want to see my timetable for today so I know which classes I'm teaching.

## 2. Request Representation
- **Request Class**: `GetScheduleQuery`
- **Payload Structure**:
    - `Date` (DateTime)
    - `TargetId` (Guid - either ClassId or TeacherId)
    - `TargetType` (Enum: Class, Teacher)

## 3. Business Logic (The Slice)
- **Trigger**: REST GET `/api/scheduling/schedule?date=...&targetId=...`
- **Logic**: Performs a synchronous read from the `scheduling.slots` table, joining with `Period` details.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `scheduling.slots`, `scheduling.periods`
- **Repository Methods**: `IScheduleRepository.GetByTargetAndDateAsync()`

## 5. Domain Objects (High-Level)
- **ScheduleDto**: Flattened representation of the slot and period for the UI.

## 6. API / UI Contracts
- **Route**: `GET /api/scheduling/schedule`
- **Response**: `Result<List<ScheduleDto>>`

## 7. Security
- **Required Permission**: `SchedulingPermissions.ViewTimetable`
- **Auth Policy**: `RequireAnyAuthenticated`
