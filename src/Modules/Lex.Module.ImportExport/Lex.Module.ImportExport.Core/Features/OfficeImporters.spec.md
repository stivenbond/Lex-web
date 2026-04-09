# Specification: Office Format Importers (DOCX & XLSX)

## 1. Overview
- **Category**: Data Transformation / Domain Mapping
- **Parent Module**: ImportExport
- **Purpose**: Define the detailed logic for converting Microsoft Office documents into the Lex platform's internal data structures.

## 2. DocxToBlockContentMapper (The "Lesson Importer")
- **Input**: Microsoft Word (.docx) file.
- **Output**: `LexBlockContent` (List of Tiptap-compatible JSON nodes).

### Extraction Rules:
- **Headings**: Map `Heading 1` to `{"type": "heading", "attrs": {"level": 1}}`. Map `Heading 2` to `level 2`, etc.
- **Paragraphs**: Map standard text blocks to `{"type": "paragraph"}`.
- **Images**: 
    - Extract embedded images from the `word/media` package.
    - Upload each image to `ObjectStorage` and obtain a `FileId`.
    - Insert an `image` block: `{"type": "image", "attrs": {"src": "/api/storage/files/{fileId}"}}`.
- **Lists**: Map `list-item` nodes to `bulletList` or `orderedList` structures.

## 3. XlsxToSchemaImporter (The "Data Importer")
- **Input**: Microsoft Excel (.xlsx) or CSV file.
- **Output**: Bulk commands to domain modules.

### Mapping Engine:
- **Phase 1: Headers Detection**: Read the first row to determine column names.
- **Phase 2: User Mapping**: Match headers to the target schema fields (e.g., "Full Name" -> `FullName`).
- **Phase 3: Row Parsing**: 
    - Convert Excel serial dates to `DateTimeOffset`.
    - Validate data types (e.g., numeric check for Grades).
- **Phase 4: Schema Alignment**: 
    - **Identity**: Create Lex Accounts and set default OIDC roles.
    - **Scheduling**: Insert TimeSlots and RoomAssignments.
    - **Gradebook**: Insert AssessmentResults into the `AssessmentDelivery` module records.

## 4. Technical Stack
- **Library**: `DocumentFormat.OpenXml` (OpenXML SDK) for server-side reflection and extraction without the need for interop/Office installation.
- **Architecture**: The mappers follow a standard `IFormatParser` interface, allowing new formats (e.g., Markdown, Google Sheets) to be added without changing the `ImportJob` state machine.

## 5. Security & Isolation
- **Memory Management**: Use `Stream` based parsing to avoid loading large spreadsheets into RAM all at once.
- **Validation**: Every cell is sanitized to prevent CSV Injection and XSS before being mapped into JSON or SQL.
