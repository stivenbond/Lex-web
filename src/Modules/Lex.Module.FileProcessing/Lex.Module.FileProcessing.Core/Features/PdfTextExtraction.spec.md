# Feature Specification: PDF Text Extraction Job

## 1. Feature Overview
- **Parent Module**: FileProcessing
- **Description**: Deep extraction of semantic text content from PDF documents, including support for image-only PDFs via OCR.
- **Type**: Background Job / Event Consumer
- **User Story**: As a System, I want to extract text from every uploaded PDF so that it can be indexed for full-text search and previewed in lesson plans.

## 2. Request Representation
- **Trigger**: `FileUploadedEvent` (from ObjectStorage).
- **Processing Request**: `EnqueueProcessingJobCommand` containing the `FileId` and `ProcessingType.PdfTextExtraction`.

## 3. Business Logic (The Slice)
- **Workflow**:
    1.  **Consumer**: `FileUploadedEventConsumer` filters for `.pdf` extension and dispatches `EnqueueProcessingJobCommand`.
    2.  **Job Initialization**: Create a `ProcessingJob` record in `Queued` state.
    3.  **Background Processing**:
        - Download PDF stream from `ObjectStorage`.
        - Use `PdfBox` or `iText` for raw text extraction.
        - **OCR Fallback**: If raw text is insufficient (< 100 characters), trigger a Tesseract OCR pass on the document images.
    4.  **Completion**: Store extracted text in `ProcessingResult`.
- **Side Effects**:
    - Push progress updates (e.g., "Page 1/10 processed") via SignalR.
    - Publish `FileProcessingCompletedEvent`.

## 4. Persistence
- **Affected Tables**: `file_processing.processing_jobs`, `file_processing.processing_results`.
- **Repository Methods**: `AddJobAsync`, `UpdateJobAsync`.

## 5. Domain Objects (High-Level)
- **PdfTextExtractor**: Internal service implementing the extraction logic.
- **FileProcessingCompletedEvent**: Contains `FileId` and the `ExtractedText`.

## 6. API / UI Contracts
- **Route**: N/A (Reactive). Monitoring via `GET /api/processing/jobs/{id}`.
- **Response**: `ProcessingJobDto`.
- **UI Interaction**: "Scanning document..." indicator on the file card in the frontend.

## 7. Security
- **Required Permission**: System Internal.
- **Auth Policy**: None (Internal bus).
