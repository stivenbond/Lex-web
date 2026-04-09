# Feature Specification: Create Academic Year

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Setup a new academic year with its corresponding terms.
- **Type**: Command
- **User Story**: As an Admin, I want to define a new academic year so that classes can be scheduled within it.

## 2. Request Representation
- **Request Class**: `CreateAcademicYearCommand`
- **Payload Structure**: 
    - `Name` (string): e.g. "2026-2027"
    - `StartDate` (Date), `EndDate` (Date)
    - `Terms` (List): { Name, StartDate, EndDate }
- **Validation Rules**: 
    - `StartDate` < `EndDate`.
    - Terms must be within Year dates.
    - Terms must not overlap.

## 3. Business Logic (The Slice)
- **Trigger**: `REST POST /api/scheduling/academic-years`
- **Domain Logic**: Calls `AcademicYear.Create(...)` factory method.
- **Side Effects**: Publishes `AcademicYearCreatedEvent`.

## 4. Persistence
- **Affected Tables**: `scheduling.academic_years`, `scheduling.terms`
- **Repository Methods**: `AddAsync(AcademicYear year)`

## 5. Domain Objects (High-Level)
- **AcademicYear**: Aggregate root.
- **Term**: Child entity.

## 6. API / UI Contracts
- **Route**: `POST /api/scheduling/academic-years`
- **Response**: `Result<Guid>` (The new Year ID).

## 7. Security
- **Required Permission**: `SchedulingPermissions.ManageCalendar`
- **Auth Policy**: `RequireAdmin`
