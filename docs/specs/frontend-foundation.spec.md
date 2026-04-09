# Specification: Frontend Infrastructure Foundation

## 1. Core Technology Stack (Latest Stable)
- **Framework**: Next.js 15.1+ (App Router).
- **Runtime**: React 19 (Enabling new `use` and metadata APIs).
- **Styling**: Tailwind CSS v3.4+ | shadcn/ui (Radix Primitives).
- **State Management**: Zustand (Global) | TanStack Query v5 (Server).
- **Forms**: React Hook Form v7 | Zod (Validation schemas).

## 2. Infrastructure Principles
- **The "Internal Only" Rule**: The frontend should never have a `NEXT_PUBLIC_API_URL`. All requests are relative to the current origin, routed via the .NET YARP gateway.
- **Type-Strict API**: Every API call must use a generic response wrapper that handles the .NET `Result<T>` and `ProblemDetails` patterns.
- **Layered State**: 
    1.  **Server State (React Query)**: For all data originating from backend modules.
    2.  **Global UI State (Zustand)**: For cross-cutting concerns (Auth, SignalR status).
    3.  **Local State (useState)**: For transient UI interactions.

## 3. Directory & Routing Strategy
- **Route Groups**:
    - `(auth)`: Public pages (Login, Recovery).
    - `(app)`: Authenticated platform pages, wrapped in the `AppShell`.
- **Module Parity**: The structure inside `(app)` must mirror the backend modules (e.g., `app/(app)/diary-management`).

## 4. Development Standards
- **Strict Linting**: TypeScript-aware ESLint rules + Prettier.
- **Component Ownership**: shadcn/ui components are copied to `components/ui` and owned by the project.
- **Testing**: Vitest for unit/logic, Playwright for E2E.

## 5. Security Baseline
- **CSRF**: Automatic token attachment via the `apiFetch` wrapper.
- **XSS**: Native React escaping + strict Sanitization of Rich Text content from Tiptap.
- **Content Security Policy (CSP)**: Nonce-based CSP configured in `next.config.ts`.
