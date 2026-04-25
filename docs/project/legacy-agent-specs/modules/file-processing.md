# Module Specification: FileProcessing

## 1. Overview
- **Name**: FileProcessing
- **Purpose**: Asynchronous extraction and transformation of uploaded media.
- **Schema**: `file_processing.*`
- **Primary Stakeholders**: Teachers (uploading content), System (maintenance).

## 2. Domain Boundaries
- **Aggregate Root: ProcessingJob**: Tracks the status and results of a file transformation task.
- **Entity: ProcessingResult**: The output metadata or text extracted.

## 3. Module Responsibilities
- PDF-to-Text extraction (for searchability).
- Image thumbnail generation.
- Video transcoding/probing (high level).

## 4. Integration & Dependencies
- **Inbound Events**:
    - `FileUploadedEvent` (ObjectStorage): Triggers a new processing job based on file type.
- **Outbound Events**:
    - `FileProcessingCompletedEvent`
    - `FileProcessingFailedEvent`
- **External Dependencies**: 
    - `ffmpeg` or `SkiaSharp` for media.
    - `Tesseract` or similar for OCR (optional/deferred).

## 5. Security & Authorization
- **Permission Constants**: `ProcessingPermissions.cs`
- **Policies**: `RequireProcessingAdmin`.

## 6. Cross-Module Interactions
- **ObjectStorage**: Fetches the bytes from `IObjectStorageService` and writes back derivative files (thumbnails).
- **Search/Diary**: Subscribes to results for indexing content.
