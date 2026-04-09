# Specification: Import/Export Orchestration Flow

## 1. Overview
- **Category**: Cross-Module Orchestration
- **Parent Module**: ImportExport
- **Purpose**: Define the choreography of services involved in moving data from external sources into the Lex domain schemas.

## 2. Interaction Diagram (Conceptual)

```mermaid
sequenceDiagram
    participant UI as Lex Frontend
    participant GI as GoogleIntegration
    participant OS as ObjectStorage
    participant IE as ImportExport
    participant DM as Domain Modules (Lesson/Diary)

    UI->>GI: Select File via Google Picker
    GI->>OS: Stream Download & Store
    OS-->>IE: FileUploadedEvent (Topic: BulkImport)
    
    rect rgb(200, 230, 255)
    Note right of IE: Orchestration Start
    IE->>IE: Initialize ImportJob
    IE->>OS: Read File Stream
    
    alt is DOCX
        IE->>IE: Run DocxToBlockMapper
        IE->>DM: Command: AddLessonResource (BlockContent)
    else is XLSX/CSV
        IE->>IE: Run XlsxToSchemaMapper (with MappingWizard rules)
        IE->>DM: Command: UpsertIdentity/DiaryData
    end
    end

    IE->>UI: SignalR: ProgressUpdate / JobCompleted
```

## 3. Detailed Step Descriptions

### Step 1: Format Parsing (The Extraction Layer)
- The `ImportExport` module uses specific parsers for each MIME type.
- **DOCX**: Maps Word paragraphs to `Heading` and `Paragraph` blocks. Maps images embedded in Word to sub-resources in `ObjectStorage`.
- **XLSX**: Maps rows to DTOs for bulk validation against the target database schema.

### Step 2: Mapping & Validation (The Transformation Layer)
- If the column names in a spreadsheet don't match Lex's internal schema exactly, the `MappingWizard` (Frontend) provides a JSON map.
- The backend uses this map to normalize data before persistence.

### Step 3: Persistence (The Load Layer)
- Instead of direct DB access, `ImportExport` sends **Bulk Commands** to the target module's internal services.
- This ensures that all domain invariants (e.g., uniqueness, foreign keys) are enforced by the module that owns the data.

## 4. Resilience & Error Handling
- **Transaction Management**: Each block of 100 rows is processed as a sub-transaction to prevent full rollback on a single error.
- **Error Capture**: Row-level validation errors are collected and returned in the final `ValidationPackage`.

## 5. Security
- Only the `ImportExport` service account has permission to read files specifically tagged for bulk ingestion in `ObjectStorage`.
