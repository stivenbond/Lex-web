# Domain Specification: Railway-Oriented Results

## 1. Context
- **Module**: SharedKernel
- **Scope**: Platform-wide error handling and result propagation.

## 2. Core Ubiquitous Language
- **Result<T>**: A wrapper for a successful value or a failure error. Used to eliminate exceptions for expected business failures.
- **Error**: A structured container for failure information (Code, Message, Type).
- **ErrorType**: Categorization of errors (NotFound, Validation, Conflict, unauthorized, Failure).

## 3. Domain Model Hierarchy

- **Aggregate Root: Result<T>**
    - **Purpose**: To provide a unified return type for all application handlers, ensuring consistent error handling.
    - **Invariants**: 
        - Cannot be both Success and Failure simultaneously.
        - A Success must contain a non-null Value (unless T is void/nullable).
        - A Failure must contain a non-null Error.

- **Entity: Error**
    - **Purpose**: To carry diagnostic information across module boundaries.
    - **Properties**:
        - `Code`: Machine-readable unique string (e.g., "USER_NOT_FOUND").
        - `Message`: Human-readable description.
        - `Type`: Influence HTTP status code mapping and retry logic.

## 4. Value Objects
- **ErrorType (Enum)**:
    - `Failure`: Generic infrastructure or logic failure.
    - `NotFound`: Resource does not exist.
    - `Validation`: Input data violates business rules.
    - `Conflict`: State conflict (e.g., duplicate email).
    - `Unauthorized`: Missing or insufficient permissions.

## 5. Domain Events
N/A (Result is a primitive for communication, not a state-bearing entity).

## 6. Business Operations (Conceptual)
- **Op: Success Assignment**
    - **Resulting State**: `IsSuccess` is true, `Value` is populated.
- **Op: Failure Assignment**
    - **Resulting State**: `IsSuccess` is false, `Error` is populated.
- **Op: Implicit Conversion**
    - **Purpose**: Allow handlers to return `T` or `Error` directly, and have it transparently wrapped in `Result<T>`.
