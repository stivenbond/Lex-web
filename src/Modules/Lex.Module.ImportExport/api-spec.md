# API Specification: ImportExport Module

## 1. Overview
- **Parent Module**: ImportExport
- **Base Route**: `/api/importexport`
- **Purpose**: Manage the lifecycle of bulk data movement, orchestration, and status tracking.

## 2. API Endpoints

### Jobs Management
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/import` | `StartImport` | Start a bulk ingestion job. |
| `POST` | `/export` | `StartExport` | Start a document generation job. |
| `GET` | `/jobs/{id}` | - | Check job status and progress. |
| `DELETE` | `/jobs/{id}` | `CancelJob` | Abort a running or queued job. |

### Configuration (Helpers)
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/schemas/{domain}` | - | Get the required columns and data types for a specific import target. |

## 3. Request/Response Shapes

### Resource: ImportJobDto
```json
{
  "id": "guid",
  "status": "Processing | Mapping | Completed | Failed",
  "progress": 75,
  "result": {
    "totalProcessed": 100,
    "successCount": 98,
    "errors": [
      { "row": 15, "column": "Email", "error": "Invalid format" }
    ]
  }
}
```

## 4. Security & Authorization
- **Administrative Rights**: `/api/import` is restricted to users with `CanBulkImport` permission.
- **Resource Ownership**: Users can only see job status for exports or imports they initiated.

## 5. Integration with YARP
- Match: `/api/importexport/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
