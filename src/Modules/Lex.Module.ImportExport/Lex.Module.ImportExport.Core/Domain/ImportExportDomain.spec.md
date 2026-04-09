# Domain Specification: Bulk Import & Export Orchestration

## 1. Context
- **Module**: ImportExport
- **Scope**: Lifecycle management of complex data ingestion and extraction jobs.

## 2. Core Ubiquitous Language
- **ImportJob**: A stateful process of moving data from an external file into the Lex platform.
- **ExportJob**: A stateful process of bundling Lex data into an external document format.
- **Mapper**: An internal engine that translates external fields (Excel columns, Word headings) into Lex Domain Models.
- **ValidationPackage**: The report of successes and failures generated at the end of an import.

## 3. Domain Model Hierarchy

- **Aggregate Root: ImportJob**
    - **Purpose**: Manage the multi-stage ingestion workflow.
    - **States**: `Pending` -> `ExtractingContent` -> `ValidatingSchema` -> `MappingData` -> `PersistenceInProgress` -> `Completed` | `Failed`.
    - **Invariants**: 
        - Must encompass a `SourceFileId`.
        - Must record `ProgressPercentage`.
- **Aggregate Root: ExportJob**
    - **Purpose**: Manage document generation.
    - **States**: `Pending` -> `CollectingData` -> `GeneratingFile` -> `StoringResult` -> `ReadyForDownload`.
- **Entity: MappingRule**
    - **Purpose**: Define a (SourceField -> DestinationProperty) association. Supports simple transformations (e.g., date format normalization).

## 4. Value Objects
- **ImportResult**: Contains counts of records processed, created, updated, and failed, along with an `ErrorLog`.
- **LexBlockContent**: The target schema for DOCX imports (standardized Tiptap JSON).

## 5. Domain Events
- **ImportJobStarted** / **ImportJobFinished**: Triggered at the boundaries of the ingestion.
- **DataMappedToDomain**: Published once the `Mapping` stage finishes, allowing target modules to prepare for persistence.

## 6. Business Operations (Conceptual)
- **Op: Map Word to BlockContent**
    - **Logic**: Use the `DocxToBlockContentMapper` to iterate through OpenXML nodes and emit `Heading`, `Text`, `BulletList`, and `Image` blocks.
- **Op: Validate Spreadsheet Schema**
    - **Logic**: Verify that the columns in the XLSX match the required fields for the target domain (e.g., "StudentName" and "Email" for Student import).
