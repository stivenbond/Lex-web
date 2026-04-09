# Infrastructure Specification: Garage Storage (S3-Compatible)

## 1. Overview
- **Category**: File Provider
- **Parent Module**: ObjectStorage
- **Component Name**: GarageS3Provider

## 2. Technical Strategy
- **Base Technology**: AWS SDK for .NET (S3 Client).
- **Storage/Transport**: Garage (S3-compatible API).

## 3. Implementation Details
Communicates with the Garage container via S3 protocol. High availability and distributed storage support.

- **Interface Realized**: `IObjectStorageProvider`
- **Classes/Interfaces**:
    - **GarageS3Provider**: Implementation using `IAmazonS3` client.

## 4. Configuration & Settings
- **Settings Class**: `GarageStorageSettings`
- **Key in `appsettings.json`**: `ObjectStorage:Garage`
- **Required Secrets**: `AccessKey`, `SecretKey`, `ServiceUrl`.

## 5. Resilience & Performance
- **Polly Policies**: Retry with exponential backoff for transient HTTP/Network errors.
- **Streaming**: Multipart upload support for files > 5MB.
- **Timeout Model**: Long-running (up to 30s per part).

## 6. Integration Points
- **Health Checks**: S3 `ListBuckets` or `GetBucketLocation` ping.
- **Metrics/Logging**: `module.objectstorage.garage.requests_total` (counter, tagged by status).

## 7. Migration & Deployment
- **Post-Deploy Verification**: Connect to Garage container and verify bucket existence.
