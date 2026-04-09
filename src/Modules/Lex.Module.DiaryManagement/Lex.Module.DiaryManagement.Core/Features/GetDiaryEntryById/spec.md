# Feature Specification: Get Diary Entry by ID

## 1. Feature Overview
- **Parent Module**: DiaryManagement
- **Description**: Retrieve the full details of a specific diary entry including its BlockContent body and attachments.
- **Type**: Query
- **User Story**: As a Teacher, I want to open a specific diary entry so that I can edit it or review its content.

## 2. Request Representation
- **Request Class**: `GetDiaryEntryByIdQuery`
- **Payload Structure**:
    - `Id`: GUID
- **Validation Rules**:
    - `Id` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/diary/{id}`
- **Domain Logic**:
    - Load `DiaryEntry` aggregate by `Id`.
    - Populate attachment metadata if IDs are present (look up in ObjectStorage read model if available, or just return IDs).
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `diary.diary_entries`
- **Repository Methods**: `GetByIdAsync`

## 5. Domain Objects (High-Level)
- **DiaryEntryFullDto**: Contains metadata, status, `BlockContent` body, and list of `FileId`s.

## 6. API / UI Contracts
- **Route**: `GET /api/diary/{id}`
- **Response**: `Result<DiaryEntryFullDto>`
- **UI Interaction**: Opens the Diary Editor for a specific lesson/slot.

## 7. Security
- **Required Permission**: `DiaryPermissions.View`
- **Auth Policy**: `GeneralAccess` (Specific ownership checks apply for internal/draft entries)
