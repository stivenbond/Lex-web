# Specification: Playwright E2E Smoke Tests

## 1. Overview
- **Category**: End-to-End Testing (UI)
- **Scope**: Verification of the "Happy Path" user journeys across the full-stack architecture.

## 2. Infrastructure: Playwright Base
- **Test Runner**: Playwright (v1.45+).
- **Environment**: Execution against a running `docker compose` stack.
- **Traceability**: Every test run must capture:
    - `trace.zip`: Full action timeline and network interception.
    - `video`: MP4 of the browser interaction.
    - `logs`: Combined console output from web and API.

## 3. High-Value Journeys (Smoke Tests)

### Journey 1: The New Student Onboarding
- **Steps**:
    1.  Login as Admin.
    2.  Navigate to `/import`.
    3.  Upload a CSV student list.
    4.  Wait for "Import Completed" push notification.
    5.  Logout/Login as one of the new students.
    6.  Verify dashboard renders correctly.

### Journey 2: Assessment Lifetime
- **Steps**:
    1.  Teacher creates an Assessment with 3 Multiple Choice questions.
    2.  Teacher publishes the assessment.
    3.  Student navigates to the assessment link.
    4.  Student completes the assessment.
    5.  Teacher navigates to `/grading` and verifies the submission appeared.

### Journey 3: Real-Time Diary Sync
- **Steps**:
    1.  Open two browser windows (Teacher A and Teacher B).
    2.  Teacher A adds a schedule entry.
    3.  Verify Teacher B sees the new entry appear instantly via SignalR without page refresh.

## 4. Visual Regression
- **Baseline**: App Shell Sidebar and Navigation header.
- **Verification**: `expect(page).toHaveScreenshot()` to ensure layout stability across major infrastructure changes.

## 5. Mocking Strategy
- **Keycloak**: Tests will use a pre-bootstrapped Keycloak instance with a "Test" realm containing stable generic users.
- **Google Drive**: Intercepted at the network layer in Playwright to return mock JSON file metadata.

## 6. Cleanup
- No explicit DB cleanup required between E2E runs (since each run uses a unique organization partition), but the stack is fresh for each CI execution.
