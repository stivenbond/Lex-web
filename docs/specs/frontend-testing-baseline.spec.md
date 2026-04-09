# Specification: Frontend Testing Baseline (Mocking)

## 1. Overview
- **Category**: Quality Assurance / Documentation Integrity
- **Scope**: Standardization of network mocking and component validation in the Next.js client.

## 2. Mock Service Worker (MSW)
- **Library**: `msw` (v2+).
- **Core Principle**: Intercept `fetch` at the network layer. Ensure tests exercise the real `apiFetch` and `TanStack Query` hooks without needing a running backend.
- **Handler Structure (`tests/mocks/handlers.ts`)**:
    - Every backend module specced (Diary, Lesson, etc.) must have a corresponding "Handler Group".
    - Handlers return standard JSON payloads matching the TypeScript DTOs.
    - Handlers simulate error codes (`422`, `500`) to test UI resilience.

## 3. Component Testing (Vitest + RTL)
- **Tooling**: `Vitest` (Vite-native test runner) + `React Testing Library`.
- **Naming Convention**: `{ComponentName}_{Scenario}_{ExpectedResult}.test.tsx`.
- **Mocking**: 
    - Use `vi.mock` for external libraries (e.g., SignalR, React Flow) to isolate component rendering.
    - Provide a `TestWrapper` containing `QueryClientProvider` and a `MockZustandState`.

## 4. Integration Flows
- **User Stories**: Test high-value flows like "Student opens lesson -> Attaches resource -> Receives Toast".
- **MSW Logic**: Use MSW to simulate the full request/response lifecycle, including the `202 Accepted` -> `SignalR Push` flow.

## 5. UI Snapshot Testing
- Use Vitest's `toMatchInlineSnapshot()` sparingly for structural components that rarely change (e.g., Nav bar links).

## 6. Continuous Integration (CI)
- **Constraint**: All unit and component tests MUST pass in CI before the Docker build begins.
- **Commands**: 
    - `npm run test` (Vitest).
    - `npm run lint` (ESLint).
    - `npm run tsc` (Type checking).
