# Feature Specification: Manage Academic Year

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Allows admins to define the school calendar boundaries.
- **Type**: Command
- **User Story**: As an Admin, I want to create academic years and terms so that teachers can start scheduling lessons.

## 2. Request Representation
- **Request Class**: `CreateAcademicYearCommand`
- **Payload Structure**:
    - `Name` (String)
    - `StartDate` (DateTime)
    - `EndDate` (DateTime)
    - `Terms` (List of Term Objects: Name, StartDate, EndDate)
- **Validation Rules**:
    - StartDate must be < EndDate.
    - Terms must reside within the Year boundaries.

## 3. Business Logic (The Slice)
- **Trigger**: REST POST `/api/scheduling/years`
- **Domain Logic**: Instantiates the `AcademicYear` aggregate. Validates term overlaps via domain logic.
- **Side Effects**: Publishes `AcademicYearCreatedEvent`.

## 4. Persistence
- **Affected Tables**: `scheduling.academic_years`, `scheduling.terms`
- **Repository Methods**: `IAcademicYearRepository.AddAsync()`

## 5. Domain Objects (High-Level)
- **AcademicYear**: Aggregate Root.
- **Term**: Child Entity.

## 6. API / UI Contracts
- **Route**: `POST /api/scheduling/years`
- **Response**: `Result<Guid>` (The YearId)

## 7. Security
- **Required Permission**: `SchedulingPermissions.ManageCalendar`
- **Auth Policy**: `RequireAdmin`
