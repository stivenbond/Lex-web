# Feature Specification: Diary Approval Workflow

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Handles the transitions of a diary entry from Draft to Submitted to Approved.
- **Type**: Command
- **User Story**: As a Teacher, I want to submit my diary for review. As an Admin, I want to approve it so it becomes an official record.

## 2. Request Representation
- **Command 1**: `SubmitDiaryEntryCommand` (Payload: `DiaryEntryId`)
- **Command 2**: `ApproveDiaryEntryCommand` (Payload: `DiaryEntryId`, `Comment`)

## 3. Business Logic (The Slice)
- **Submit Workflow**:
    - Trigger: `POST /api/diary/entries/{id}/submit`
    - Logic: Changes state to `Submitted`. Validates that content is not empty.
    - Side Effect: Publishes `DiaryEntrySubmittedEvent`.
- **Approve Workflow**:
    - Trigger: `POST /api/diary/entries/{id}/approve`
    - Logic: Changes state to `Approved`. Entry becomes read-only.
    - Side Effect: Publishes `DiaryEntryApprovedEvent`.

## 4. Persistence
- **Affected Tables**: `diary.entries`
- **Repository Methods**: `IDiaryRepository.UpdateStateAsync()`

## 5. Domain Objects (High-Level)
- **DiaryEntry**: State machine logic encapsulated in the aggregate.

## 6. API / UI Contracts
- **Routes**:
    - `POST /api/diary/entries/{id}/submit`
    - `POST /api/diary/entries/{id}/approve`
- **Response**: `Result<Success>`

## 7. Security
- **Submission Permission**: `DiaryPermissions.Submit`
- **Approval Permission**: `DiaryPermissions.Approve`
