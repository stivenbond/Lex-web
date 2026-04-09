  
   
**WEB CLIENT ARCHITECTURE REFERENCE**  
Next.js · React · TypeScript · PWA  
 

Version 1.0  |  Classification: Internal  |  Companion to: System Architecture Reference

| Purpose of This Document This document is the authoritative architecture reference for the web client. It is a companion to the System Architecture Reference (which covers the .NET backend) and must be read alongside it. It defines the project structure, rendering strategy, state management, authentication flow, real-time integration, PWA compliance, and the architectural rules that govern every UI feature. Feature-level specifics belong in spec-docs inside the codebase. |
| :---- |

# **Table of Contents**

# **1\. Architectural Philosophy & Guiding Principles**

The web client is a first-class architectural component of the platform, not an afterthought. Every structural decision mirrors the discipline applied to the .NET backend: explicit over implicit, separation of concerns, and a layered design that makes features easy to add without touching existing code.

## **1.1 Core Principles**

### **The Client is a Delivery Mechanism, Not a Backend**

The web client renders UI and manages user interaction. It does not own business logic, does not perform data transformations that belong in the API, and does not duplicate validation rules that exist in FluentValidation on the server. The .NET backend is the authoritative source of truth for all domain logic. The client reflects it.

### **Server Components for Structure, Client Components for Interaction**

Next.js App Router provides two component types. Server Components run on the Node.js server, have no JavaScript bundle impact on the client, and are used for layout, navigation shells, and any content that does not require browser APIs or user interaction. Client Components run in the browser and are used for all interactive features: forms, real-time updates, diagrams, document editors, and anything that uses React state or SignalR. This boundary is architectural — it is not chosen per file based on convenience.

### **One Codebase, Two Surfaces**

The same Next.js project serves both the browser and the installed PWA. There is no separate codebase for the desktop install experience. PWA compliance is an infrastructure concern (manifest, service worker, HTTPS) layered on top of the standard web application, not a separate build target.

### **YARP is the Only Door**

The web client never calls the .NET API directly using a separate hostname or port. All API calls, SignalR connections, and authentication redirects are made to the same origin as the page itself. YARP, running inside the .NET API container, routes those requests internally. This means zero CORS configuration, a single TLS certificate, and a network topology that is invisible to the browser.

### **State Has a Single Owner**

Every piece of state in the application has exactly one owner. Server state (data from the API) is owned by TanStack Query. Global client state (current user, UI preferences, SignalR connection) is owned by Zustand. Local component state (form inputs, toggle state, transient UI) is owned by React useState. These three layers never overlap. Data flows in one direction and is never duplicated across layers.

## **1.2 What This Architecture Is Not**

* It is not a full-stack Next.js application. API routes are not used. The Node.js server renders HTML and serves the client bundle — it does not own business logic or data access.

* It is not a micro-frontend architecture. All modules share one Next.js project, one routing tree, and one build.

* It is not a native desktop app. The PWA install experience provides desktop presence without Electron or Tauri. Native OS integrations beyond what the browser provides are out of scope.

* It is not server-side rendered in the traditional sense. Server Components render the structural shell. Interactive content is hydrated or fetched client-side after the initial paint.

# **2\. System Context: How the Client Fits into the Platform**

Before describing the client in isolation, it is essential to understand its position in the full system. This section describes the network topology and the boundaries between the client and the .NET backend. Full details of the backend are in the System Architecture Reference.

## **2.1 Network Topology**

There is one externally visible address for the entire platform. The browser always talks to a single origin. Internally, Docker routes requests between containers. The client has no awareness of this internal routing.

`BROWSER / INSTALLED PWA`

        `|`

        `|  HTTPS :443  (single origin — all requests go here)`

        `v`

`┌────────────────────────────────────────────────┐`

`│           YourApp.API container                │`

`│                                                │`

`│   ┌──────────────────────────────────────┐    │`

`│   │          YARP middleware              │    │`

`│   └────────────────┬─────────────────────┘    │`

`│                    │                           │`

`│         ┌──────────┴──────────┐               │`

`│         │                     │               │`

`│    /** (all pages)       /api/**              │`

`│    /hubs/** (SignalR)    .NET handlers        │`

`│         │                MediatR              │`

`│         │                SignalR              │`

`│         │ http://web:3000                     │`

`│         v  (internal Docker network)          │`

`│  ┌──────────────┐                             │`

`│  │ web container│                             │`

`│  │ Next.js :3000│                             │`

`│  │ (no public   │                             │`

`│  │  port)       │                             │`

`│  └──────────────┘                             │`

`└────────────────────────────────────────────────┘`

The web container has no exposed port. It is reachable only from within the Docker network by YARP. Institutions cannot directly access the Next.js server. This is intentional — it prevents bypassing YARP's TLS termination, rate limiting, and audit logging.

## **2.2 Request Routing Rules**

| URL Pattern | Routed To |
| :---- | :---- |
| /\*\*  (all page routes) | Next.js server (web container) — returns HTML |
| /api/\*\* | .NET MediatR handlers — returns JSON |
| /hubs/\*\* | .NET SignalR hubs — upgrades to WebSocket |
| /\_next/\*\* | Next.js static assets (JS, CSS chunks) |
| /manifest.webmanifest | Next.js public directory — PWA manifest |
| /sw.js | Next.js public directory — service worker |

## **2.3 Container Relationship**

The web container is defined alongside all other platform containers in infra/docker-compose.yml. It is not a separate repository or a separately deployed project. The client and the backend are one product with one version number, one CI pipeline, and one release artifact. See the System Architecture Reference Section 10 and 11 for container inventory and CI/CD pipeline detail.

| Cross-Reference For YARP routing configuration, container Dockerfile, Docker Compose structure, and the CI/CD pipeline that builds and publishes the web container, refer to the System Architecture Reference Sections 5.5, 10, 11\. |
| :---- |

# **3\. Project Structure**

The Next.js project lives in the client/ directory at the repository root. It is a self-contained Node.js project with its own package.json, TypeScript config, and Dockerfile. It shares no source files with the .NET projects, but it does share type contracts through generated API types.

## **3.1 Directory Layout**

`client/`

`├── app/                          <- Next.js App Router root`

`│   ├── layout.tsx                <- root layout (providers, fonts, PWA meta)`

`│   ├── page.tsx                  <- root page (redirect to dashboard or login)`

`│   ├── (auth)/                   <- route group: unauthenticated pages`

`│   │   └── login/`

`│   │       └── page.tsx`

`│   └── (app)/                    <- route group: authenticated pages`

`│       ├── layout.tsx            <- authenticated shell (nav, sidebar)`

`│       ├── dashboard/`

`│       │   └── page.tsx`

`│       └── [module]/             <- one folder per backend module`

`│           └── ...`

`│`

`├── components/`

`│   ├── ui/                       <- shadcn/ui base components (owned, not imported)`

`│   ├── layout/                   <- nav, sidebar, topbar, shell components`

`│   ├── forms/                    <- reusable form primitives`

`│   ├── diagrams/                 <- React Flow wrappers and custom nodes`

`│   ├── documents/                <- Tiptap editor wrappers`

`│   ├── presentation/             <- slideshow/presentation view components`

`│   └── shared/                   <- generic reusable components`

`│`

`├── lib/`

`│   ├── api/                      <- TanStack Query hooks (one file per module)`

`│   ├── signalr/                  <- SignalR connection manager`

`│   ├── auth/                     <- Keycloak client utilities`

`│   ├── store/                    <- Zustand stores`

`│   ├── utils/                    <- pure utility functions`

`│   └── types/                    <- shared TypeScript types and API DTOs`

`│`

`├── public/`

`│   ├── manifest.webmanifest      <- PWA manifest`

`│   ├── icons/                    <- PWA icon set (all required sizes)`

`│   └── screenshots/              <- PWA install screenshots`

`│`

`├── styles/`

`│   └── globals.css               <- Tailwind base + CSS variables`

`│`

`├── middleware.ts                 <- Next.js middleware (auth guard)`

`├── next.config.ts                <- Next.js config (PWA, headers, rewrites)`

`├── tailwind.config.ts`

`├── tsconfig.json`

`├── package.json`

`└── Dockerfile`

## **3.2 Route Group Strategy**

Next.js App Router supports route groups — folders wrapped in parentheses that create layout boundaries without affecting the URL. Two groups are used:

* (auth): Pages that do not require authentication — login, OIDC callback. These render without the authenticated shell layout and redirect to the app if the user is already logged in.

* (app): All authenticated pages. The layout.tsx at this level enforces authentication via middleware before rendering. It wraps all pages in the navigation shell, the SignalR provider, and the TanStack Query client.

The module folder structure inside (app) mirrors the .NET module structure. A module named Billing in the backend has a corresponding billing/ folder in the client. This alignment makes it easy to reason about which frontend code corresponds to which backend module.

## **3.3 Component Directory Rules**

* components/ui/ contains shadcn/ui components. These are generated once by the shadcn CLI and then owned by the project. They are modified freely to match the design system. They are never re-generated over the top of local modifications.

* components/layout/ contains the application shell. These components are server components where possible — the nav tree and sidebar do not need client-side interactivity in most cases.

* components/diagrams/, components/documents/, and components/presentation/ each contain the complex view types described in Section 7\. Each is a self-contained directory with its own sub-components, hooks, and types.

* No business logic lives in any component. Components render data and dispatch user actions. Logic lives in hooks in lib/api/ or lib/store/.

# **4\. Rendering Strategy**

Next.js App Router supports multiple rendering modes. The correct mode is determined by the nature of the content, not by developer preference. This section defines the rules for which mode applies where.

## **4.1 Rendering Mode Decision Table**

| Component Type | Rendering Mode | When to Use | Example |
| :---- | :---- | :---- | :---- |
| Server Component | Server-side (no JS sent) | Layout, nav, static structure, no browser APIs needed | AppShell, Sidebar, PageHeader |
| Client Component | Hydrated in browser | Forms, real-time data, SignalR, diagrams, editors | InvoiceForm, DiagramEditor, DocEditor |
| Page with static data | SSR on request | Pages whose data rarely changes and can be fetched server-side | Dashboard layout shell |
| Page with dynamic data | Client fetch via TanStack Query | Pages whose data changes frequently or requires auth token in browser | Data grids, live feeds |

## **4.2 The Server/Client Boundary**

The boundary between server and client components is defined by where the "use client" directive appears. The rule is: push this boundary as far down the component tree as possible. The outer shell of a page should be a server component. Only the interactive leaf nodes need to be client components.

`// page.tsx — server component (no "use client")`

`// Renders the static shell server-side`

`export default async function InvoicePage() {`

  `return (`

    `<PageShell title="Invoices">`

      `<InvoiceTable />   {/* This is the client component */}`

    `</PageShell>`

  `);`

`}`

`// InvoiceTable.tsx — client component`

`"use client";`

`// Fetches data via TanStack Query, renders interactive table`

## **4.3 Data Fetching Rules**

* Server Components may fetch data directly from the API using fetch() with the user's token passed via Next.js's cookies/headers. This is used only for structural data that is needed for the initial HTML (page titles, breadcrumb data, non-sensitive layout data).

* Client Components never fetch data with raw fetch() calls. All API communication in client components goes through TanStack Query hooks defined in lib/api/. This is non-negotiable — it ensures consistent caching, error handling, and loading states.

* Mutations (creating, updating, deleting data) always use TanStack Query useMutation hooks. They never use Server Actions. Server Actions are not used in this project because the .NET API is the authoritative mutation handler.

| Rule: No Next.js API Routes, No Server Actions Next.js API routes (app/api/ folder) and Server Actions are not used in this project. The .NET backend is the API. Adding Next.js API routes would create a redundant server layer between the client and the .NET handlers, complicating authentication, error handling, and the single-origin topology. If you find yourself creating app/api/something/route.ts, stop and route the request to the .NET API instead. |
| :---- |

# **5\. Authentication & Authorisation**

Authentication is handled entirely by Keycloak, which runs as a container in the platform stack. The web client never stores passwords, never issues tokens, and never validates tokens. It obtains tokens from Keycloak, holds them in memory, and presents them on every API request. Full Keycloak configuration is described in the System Architecture Reference Section 7\.

## **5.1 PKCE Flow**

The client uses the Authorization Code flow with PKCE (Proof Key for Code Exchange). This is the correct and only acceptable flow for a browser-based SPA. Implicit flow and client credentials are not used.

`1. User navigates to protected route`

   `-> middleware.ts redirects to Keycloak login page`

`2. User authenticates with Keycloak`

   `-> Keycloak redirects back to /auth/callback?code=...`

`3. Client exchanges code for tokens`

   `-> access_token (short-lived, 5-15 min)`

   `-> refresh_token (longer-lived, stored in httpOnly cookie)`

`4. access_token stored in memory only (never localStorage)`

   `-> attached to all API requests as Authorization: Bearer <token>`

`5. Token refresh handled silently before expiry`

   `-> lib/auth/tokenManager.ts manages refresh lifecycle`

## **5.2 Token Storage Rules**

| Security Rule: Tokens Never in localStorage or sessionStorage Access tokens stored in localStorage are readable by any JavaScript on the page, including injected scripts. The access token is stored in memory (a module-level variable in lib/auth/tokenManager.ts) and is lost on page refresh. The refresh token is stored in an httpOnly, Secure, SameSite=Strict cookie set by the server, making it inaccessible to JavaScript entirely. On page load, the silent refresh flow uses the refresh token cookie to obtain a new access token without user interaction. |
| :---- |

## **5.3 Middleware Auth Guard**

Next.js middleware (middleware.ts at the project root) intercepts every request before it reaches a page component. It checks for a valid session and redirects unauthenticated users to the Keycloak login page.

`// middleware.ts`

`export function middleware(request: NextRequest) {`

  `const { pathname } = request.nextUrl;`

  `// Public paths bypass the auth check`

  `if (pathname.startsWith("/(auth)") || pathname === "/") {`

    `return NextResponse.next();`

  `}`

  `// Check for session cookie — redirect if absent`

  `const session = request.cookies.get("session");`

  `if (!session) {`

    `return NextResponse.redirect(keycloakLoginUrl(request));`

  `}`

  `return NextResponse.next();`

`}`

## **5.4 SignalR Authentication**

SignalR WebSocket connections cannot send HTTP headers during the initial handshake. The access token is passed as a query string parameter on the hub connection URL. This is the standard ASP.NET SignalR pattern and is handled automatically by the SignalR connection manager in lib/signalr/.

`// lib/signalr/connectionManager.ts`

`const connection = new HubConnectionBuilder()`

  `.withUrl("/hubs/notifications", {`

    `accessTokenFactory: () => tokenManager.getAccessToken()`

  `})`

  `.withAutomaticReconnect()`

  `.build();`

The token passed to accessTokenFactory is retrieved from the in-memory token manager and refreshed automatically if expired before the factory returns it. The .NET server validates it as a standard JWT bearer token via the OnMessageReceived event in its JWT bearer options — see System Architecture Reference Section 7.3.

## **5.5 Authorisation in the UI**

The client performs UI-level authorisation checks to show or hide elements based on the user's role claims in the JWT. This is cosmetic only — the .NET API enforces real authorisation on every request. The client must never assume a hidden button means an action is forbidden; the server is the authority.

* Role claims are decoded from the JWT access token by lib/auth/claims.ts

* A usePermissions() hook provides a clean interface for checking permissions in components

* Permission constants in the client mirror the permission constants defined in each Module.Core project on the .NET side

* Showing a spinner or disabled state while a permission check loads is preferred over a flash of unauthorised content

# **6\. State Management**

State is divided into three non-overlapping layers. Each layer has a single owner, a defined scope, and a defined lifetime. Mixing these layers is the primary source of state-related bugs and is explicitly forbidden.

## **6.1 The Three Layers**

| Layer | Owner | Examples |
| :---- | :---- | :---- |
| Server State | TanStack Query (React Query) | Invoice list, user profile, module data from API |
| Global Client State | Zustand | Current user identity, SignalR connection status, global UI preferences |
| Local Component State | React useState / useReducer | Form field values, modal open/closed, accordion expanded |

## **6.2 Server State: TanStack Query**

TanStack Query is the standard for managing asynchronous server state in React. It handles fetching, caching, background refetching, optimistic updates, and error states. Every API call in the application goes through a TanStack Query hook.

***Query Hook Structure***

Each backend module has one corresponding file in lib/api/ that defines all queries and mutations for that module:

`// lib/api/billing.ts`

`export function useInvoices(filters: InvoiceFilters) {`

  `return useQuery({`

    `queryKey: ["invoices", filters],`

    `queryFn: () => apiFetch<Invoice[]>("/api/billing/invoices", { params: filters }),`

    `staleTime: 30_000,   // 30 seconds before background refetch`

  `});`

`}`

`export function useCreateInvoice() {`

  `const queryClient = useQueryClient();`

  `return useMutation({`

    `mutationFn: (cmd: CreateInvoiceCommand) =>`

      `apiFetch<InvoiceId>("/api/billing/invoices", { method: "POST", body: cmd }),`

    `onSuccess: () => queryClient.invalidateQueries({ queryKey: ["invoices"] }),`

  `});`

`}`

***SignalR Integration with TanStack Query***

When the backend pushes a real-time update via SignalR, the client invalidates the relevant TanStack Query cache rather than manually updating the local state. This keeps server state ownership clean and avoids stale data.

`// In a component that listens to SignalR events:`

`useSignalREvent("InvoiceUpdated", (invoiceId: string) => {`

  `queryClient.invalidateQueries({ queryKey: ["invoices"] });`

  `queryClient.invalidateQueries({ queryKey: ["invoice", invoiceId] });`

`});`

## **6.3 Global Client State: Zustand**

Zustand manages state that is global to the application session and not derived from the API. It is intentionally small — most things that developers reach for global state to solve are actually server state that belongs in TanStack Query.

***Store Structure***

`lib/store/`

`├── useAuthStore.ts       <- current user identity, token, auth status`

`├── useSignalRStore.ts    <- connection status, reconnect attempts`

`└── useUIStore.ts         <- sidebar collapsed, theme, notification drawer open`

***Store Rules***

* Each store is a single useXxxStore hook exported from one file.

* Stores do not call the API directly. Data that comes from the API is server state and belongs in TanStack Query.

* Stores do not duplicate TanStack Query cache data. If a component needs both the current user (Zustand) and the user's invoices (TanStack Query), it uses both hooks independently.

## **6.4 Local Component State**

React useState and useReducer handle state that is local to a single component or a tightly coupled component tree. This is the default choice for anything that does not need to be shared. Form field values, controlled input state, and UI toggles belong here. Before reaching for Zustand or TanStack Query, ask: does any other component need this state? If not, it stays local.

# **7\. Complex View Types**

The platform features four distinct view types beyond standard forms and data tables. Each has a defined library, a defined component location, and architectural rules for how it integrates with the state and API layers.

## **7.1 Standard Forms**

***Library: React Hook Form \+ Zod***

All forms in the application use React Hook Form for field registration and submission, and Zod for client-side schema validation. Zod schemas are defined alongside the form component and mirror (but do not depend on) the FluentValidation rules on the .NET side. The server always validates — client-side validation is a UX improvement only.

`// components/forms/CreateInvoiceForm.tsx`

`"use client";`

`const schema = z.object({`

  `clientId: z.string().uuid(),`

  `amount:   z.number().positive(),`

  `dueDate:  z.string().datetime(),`

`});`

`export function CreateInvoiceForm() {`

  `const { register, handleSubmit, formState } = useForm({`

    `resolver: zodResolver(schema),`

  `});`

  `const createInvoice = useCreateInvoice();  // TanStack Query mutation`

  `return (`

    `<form onSubmit={handleSubmit(data => createInvoice.mutate(data))}>`

      `...`

    `</form>`

  `);`

`}`

* Form components live in components/forms/ for reusable primitives (inputs, selects, date pickers) and inside the feature folder for feature-specific forms.

* Errors returned from the .NET API (as Result\<T\> error payloads) are mapped to React Hook Form field errors via the mutation's onError callback.

* Form submissions always go through a TanStack Query useMutation hook. Never use fetch() directly inside a form submit handler.

## **7.2 Diagram Views (Editable & Read-Only)**

***Library: React Flow***

React Flow is used for all node-based diagram views. It handles drag-and-drop node positioning, edge connections, custom node rendering, and zoom/pan. Diagram state (node positions, edge connections, node data) is persisted to the .NET API and loaded via TanStack Query.

`components/diagrams/`

`├── DiagramCanvas.tsx       <- React Flow root, handles layout and viewport`

`├── DiagramToolbar.tsx      <- add node, delete, zoom controls`

`├── nodes/`

`│   ├── BaseNode.tsx        <- shared node chrome (selection, handles)`

`│   └── [NodeType].tsx      <- one file per custom node type`

`├── edges/`

`│   └── [EdgeType].tsx      <- one file per custom edge type`

`└── hooks/`

    `├── useDiagramData.ts   <- TanStack Query hooks for diagram CRUD`

    `└── useDiagramSync.ts   <- SignalR sync for collaborative updates`

***Real-Time Diagram Sync***

When multiple users view the same diagram, updates from one user are broadcast via SignalR to all others. The receiving client applies updates by invalidating the TanStack Query cache, which triggers a refetch and re-render of the diagram. Full collaborative editing (operational transform or Y.js CRDT) is not implemented in the initial version — last-write-wins via cache invalidation is sufficient for the described use cases.

## **7.3 Presentation Views (Slideshow)**

***Library: Custom React components \+ Framer Motion***

Presentation views are full-screen, slide-based displays of structured content. There is no external library for this — it is implemented as a controlled React component with Framer Motion for slide transitions. Slides are data structures fetched from the API and rendered by a slide renderer component.

`components/presentation/`

`├── PresentationViewer.tsx   <- full-screen shell, keyboard nav, controls`

`├── SlideRenderer.tsx        <- maps slide type to the correct slide component`

`├── slides/`

`│   ├── TitleSlide.tsx`

`│   ├── ContentSlide.tsx`

`│   ├── DiagramSlide.tsx     <- embeds DiagramCanvas in read-only mode`

`│   └── ChartSlide.tsx`

`└── hooks/`

    `└── usePresentationData.ts`

* Keyboard navigation (arrow keys, Escape to exit) is handled via useEffect event listeners in PresentationViewer.tsx.

* Presentation state (current slide index, fullscreen status) is local component state — it does not belong in Zustand.

* Slide data is fetched once when the presentation opens and held in TanStack Query cache. Real-time updates during a live presentation are pushed via SignalR.

## **7.4 Document Views (Rich Text Editor)**

***Library: Tiptap***

Tiptap is a headless, extensible rich text editor built on ProseMirror. It is used for all document editing views. It is headless — it has no default styling, which means the design system's Tailwind styles apply uniformly. It supports extensions for custom node types (tables, diagrams embedded in documents, custom blocks).

`components/documents/`

`├── DocumentEditor.tsx       <- Tiptap editor root, toolbar, save state`

`├── DocumentToolbar.tsx      <- formatting controls (bold, headings, lists...)`

`├── extensions/`

`│   ├── DiagramBlock.tsx     <- custom Tiptap node: embeds a diagram by ID`

`│   └── CustomBlock.tsx      <- any other domain-specific block types`

`└── hooks/`

    `├── useDocumentData.ts   <- TanStack Query for document load/save`

    `└── useAutoSave.ts       <- debounced auto-save to API`

***Auto-Save Strategy***

Documents auto-save on a debounced interval (default: 2 seconds after the last keystroke) using useMutation from TanStack Query. The save status (saving, saved, error) is displayed in the toolbar. Optimistic updates are not used for document saves — the saved state reflects only confirmed server writes.

Collaboration (simultaneous multi-user editing) is not in scope for the initial version. The architecture uses Tiptap, which supports Y.js CRDT collaboration as a future extension when the need arises.

# **8\. Real-Time Communication**

Real-time features are powered by the same SignalR infrastructure described in the System Architecture Reference Section 4 and 5\. This section covers the client-side implementation only.

## **8.1 Connection Manager**

SignalR connection lifecycle is managed by a singleton connection manager in lib/signalr/connectionManager.ts. There is one connection per hub. The manager handles connection startup, automatic reconnect with backoff, token refresh before reconnect, and connection status reporting to the Zustand store.

`lib/signalr/`

`├── connectionManager.ts   <- singleton, manages connection lifecycle`

`├── useNotifications.ts    <- hook: subscribes to NotificationHub events`

`├── useJobStatus.ts        <- hook: subscribes to JobStatusHub events`

`└── types.ts               <- TypeScript types for all hub message payloads`

## **8.2 SignalR React Provider**

A SignalRProvider component wraps the authenticated application shell. It starts all hub connections after login, tears them down on logout, and exposes the connection manager via React context. Components never import the connection manager directly — they use hooks.

`// app/(app)/layout.tsx`

`<SignalRProvider>`

  `<AppShell>`

    `{children}`

  `</AppShell>`

`</SignalRProvider>`

## **8.3 The Async Job Pattern**

For long-running operations, the client follows the async result pattern defined in the System Architecture Reference Section 5.2:

1. Client sends REST POST — receives 202 Accepted with a jobId (GUID)

2. Client subscribes to JobStatusHub for updates keyed to the jobId

3. Server processes asynchronously via MassTransit consumer

4. Server pushes job completion or failure to the hub

5. Client receives the push, invalidates relevant TanStack Query cache, shows result

`// hooks/useAsyncJob.ts`

`export function useAsyncJob(jobId: string | null) {`

  `const [status, setStatus] = useState<JobStatus>("pending");`

  `useSignalREvent("JobCompleted", (payload) => {`

    `if (payload.jobId === jobId) setStatus("completed");`

  `});`

  `useSignalREvent("JobFailed", (payload) => {`

    `if (payload.jobId === jobId) setStatus("failed");`

  `});`

  `return status;`

`}`

## **8.4 Reconnection Handling**

* withAutomaticReconnect() is configured on all hub connections with exponential backoff.

* On reconnect, the token factory is called again — ensuring a fresh token is used rather than the expired one that may have caused the disconnect.

* On reconnect, all TanStack Query caches are marked stale and refetched — ensuring any missed real-time updates are recovered via polling fallback.

* The UI displays a connection status indicator sourced from Zustand useSignalRStore, so users are informed of degraded real-time connectivity.

# **9\. Progressive Web App (PWA) Compliance**

The web client is a fully compliant PWA. This means it is installable on Windows, macOS, Linux, iOS, and Android as a standalone app without an app store, it works offline for previously loaded content, and it receives push notifications. PWA compliance is achieved through three components: the web app manifest, the service worker, and HTTPS.

## **9.1 Web App Manifest**

The manifest.webmanifest file in the public/ directory declares the application's installability metadata. It must be served with the correct MIME type (application/manifest+json) and linked in the root layout.

`// public/manifest.webmanifest`

`{`

  `"name": "YourApp",`

  `"short_name": "YourApp",`

  `"description": "...",`

  `"start_url": "/dashboard",`

  `"display": "standalone",`

  `"background_color": "#1A3A52",`

  `"theme_color": "#2176AE",`

  `"icons": [`

    `{ "src": "/icons/icon-192.png", "sizes": "192x192", "type": "image/png" },`

    `{ "src": "/icons/icon-512.png", "sizes": "512x512", "type": "image/png" },`

    `{ "src": "/icons/icon-512-maskable.png", "sizes": "512x512",`

      `"type": "image/png", "purpose": "maskable" }`

  `],`

  `"screenshots": [`

    `{ "src": "/screenshots/dashboard.png", "sizes": "1280x720",`

      `"type": "image/png", "form_factor": "wide" }`

  `]`

`}`

The display: "standalone" value is what causes the browser to offer an install prompt and what removes the browser chrome when the app is launched from the desktop. The screenshots array enables the richer install dialog on supported browsers.

## **9.2 Service Worker**

The service worker is generated automatically by the vite-plugin-pwa integration (or next-pwa wrapper). Manual service worker code is avoided unless custom caching strategies are required. The service worker handles:

| Capability | Strategy |
| :---- | :---- |
| Static assets (JS, CSS, fonts, icons) | Cache-first: serve from cache, update in background |
| API responses (/api/\*\*) | Network-first: always try network, fall back to cache if offline |
| Page routes | Network-first: serve latest HTML from server, fall back to cached shell |
| Push notifications | Background sync via Push API — requires server-side Web Push setup |
| Offline fallback | Cached app shell shown when network is unavailable with offline indicator |

| Important: Service Worker and Authentication The service worker must not intercept or cache requests that carry the Authorization header. API requests with tokens must always go to the network. Caching authenticated responses creates a security risk where one user's data could be served to another. The service worker config must explicitly exclude /api/\*\* from cache-first strategies. |
| :---- |

## **9.3 HTTPS Requirement**

Service workers require HTTPS. In production this is handled by YARP's TLS termination — the browser always connects via HTTPS. In local development, Next.js runs on HTTP but the service worker is disabled in development mode to avoid caching issues during hot reload. The vite-plugin-pwa / next-pwa configuration handles this automatically via the disable: process.env.NODE\_ENV \=== "development" flag.

## **9.4 Install Experience**

Browsers show an install prompt when three conditions are met: the page is served over HTTPS, a valid manifest is linked, and a service worker is registered. The prompt is browser-controlled and cannot be forced. The application may intercept the beforeinstallprompt event to show a custom "Install App" button in the UI at an appropriate moment (e.g., in the user menu or a first-visit banner).

`// lib/pwa/useInstallPrompt.ts`

`export function useInstallPrompt() {`

  `const [prompt, setPrompt] = useState<BeforeInstallPromptEvent | null>(null);`

  `useEffect(() => {`

    `const handler = (e: BeforeInstallPromptEvent) => {`

      `e.preventDefault();`

      `setPrompt(e);`

    `};`

    `window.addEventListener("beforeinstallprompt", handler);`

    `return () => window.removeEventListener("beforeinstallprompt", handler);`

  `}, []);`

  `const install = () => prompt?.prompt();`

  `return { canInstall: !!prompt, install };`

`}`

## **9.5 PWA Update Strategy**

When a new version of the application is deployed, the service worker detects the change and downloads the new assets in the background. The application does not reload automatically — this would disrupt users mid-task. Instead, a non-intrusive banner is shown: "A new version is available. Refresh to update." The user controls when the update is applied.

# **10\. API Communication Layer**

All communication between the client and the .NET backend goes through a structured API layer in lib/api/. Raw fetch() calls are never made in components or stores.

## **10.1 Base Fetch Wrapper**

lib/api/client.ts defines a thin wrapper around fetch() that handles token attachment, response parsing, and error normalisation. All TanStack Query hooks use this wrapper as their queryFn.

`// lib/api/client.ts`

`export async function apiFetch<T>(`

  `url: string,`

  `options: ApiFetchOptions = {}`

`): Promise<T> {`

  `const token = await tokenManager.getAccessToken();`

  `const response = await fetch(url, {`

    `...options,`

    `headers: {`

      `"Content-Type": "application/json",`

      ``"Authorization": `Bearer ${token}`,``

      `...options.headers,`

    `},`

    `body: options.body ? JSON.stringify(options.body) : undefined,`

  `});`

  `if (!response.ok) {`

    `const error = await response.json() as ProblemDetails;`

    `throw new ApiError(error);  // caught by TanStack Query`

  `}`

  `return response.json() as T;`

`}`

## **10.2 TypeScript DTO Types**

TypeScript types for all API request and response shapes live in lib/types/. These types are maintained manually and kept in sync with the .NET DTOs. They are not auto-generated in the initial version, but the architecture supports adding OpenAPI-based code generation later (e.g., via the openapi-typescript package pointed at the .NET Swagger endpoint).

`lib/types/`

`├── billing.ts        <- Invoice, CreateInvoiceCommand, InvoiceDto...`

`├── notifications.ts`

`├── common.ts         <- ProblemDetails, ApiError, PagedResult<T>...`

`└── index.ts          <- re-exports all types`

## **10.3 Error Handling**

The .NET API returns errors as RFC 7807 ProblemDetails JSON objects. The client wraps these in an ApiError class that carries the error code, message, and validation errors. TanStack Query's onError callbacks receive this typed error, making it straightforward to map API validation errors back to React Hook Form field errors.

* Network errors (fetch throws) are caught and converted to a generic NetworkError type.

* 401 Unauthorized responses trigger a silent token refresh. If refresh fails, the user is redirected to login.

* 403 Forbidden responses show a permission denied UI — they do not trigger logout.

* 422 Unprocessable Entity responses (validation failures from FluentValidation) are mapped to form field errors.

* 5xx responses show a generic error state with a retry button.

# **11\. Design System & Component Library**

## **11.1 Foundation: Tailwind CSS**

Tailwind CSS is the styling foundation. All styles are utility classes applied directly in JSX. There is no separate CSS file per component (except globals.css which defines CSS custom properties for the design tokens). The benefit is that styles are co-located with markup, there is no naming convention to invent, and the generated CSS is minimal because Tailwind purges unused classes at build time.

| Rule: No Inline Styles, No CSS Modules Inline style={{ }} attributes and CSS module files are not used in this project. All styling is Tailwind utility classes. Exceptions are permitted only for truly dynamic values that cannot be expressed as Tailwind classes (e.g., a width value calculated from JavaScript). In those cases, a CSS custom property set via style={{ "--width": value }} consumed by a Tailwind arbitrary value class is the preferred pattern. |
| :---- |

## **11.2 Component Library: shadcn/ui**

shadcn/ui provides the base component set. Unlike a traditional component library installed as a node\_modules dependency, shadcn/ui components are copied into components/ui/ and owned by the project. This means:

* Components are fully customisable without overriding library internals.

* The component API is stable because it is local code, not a versioned dependency.

* Updating individual components is done deliberately — pull the latest shadcn source and merge manually.

* Components are built on Radix UI primitives (accessible, keyboard-navigable, ARIA-correct by default).

## **11.3 Design Tokens**

Colour, spacing, typography, and border radius values are defined as CSS custom properties in globals.css and mapped to Tailwind config. This creates a single source of truth for the visual identity that can be themed per institution.

`/* styles/globals.css */`

`:root {`

  `--color-primary:    221 90% 40%;   /* HSL: used as hsl(var(--color-primary)) */`

  `--color-background: 0 0% 100%;`

  `--color-foreground: 222 47% 11%;`

  `--radius:           0.5rem;`

`}`

`/* tailwind.config.ts */`

`theme: {`

  `extend: {`

    `colors: {`

      `primary: "hsl(var(--color-primary))",`

    `}`

  `}`

`}`

## **11.4 Typography**

The type scale is defined in Tailwind config. Body text uses the system font stack for performance. A display font may be loaded via next/font/google for headings. Font loading is always done via next/font — never a \<link\> tag — to eliminate layout shift and ensure fonts are part of the Next.js build optimisation.

# **12\. Containerisation**

## **12.1 Dockerfile**

The web container uses a multi-stage build. The build stage installs dependencies and produces the Next.js standalone output. The runtime stage copies only what is needed to run — no devDependencies, no source files, no build tooling.

`# client/Dockerfile`

`FROM node:20-alpine AS build`

`WORKDIR /app`

`COPY package.json package-lock.json ./`

`RUN npm ci`

`COPY . .`

`RUN npm run build`

`FROM node:20-alpine AS runtime`

`WORKDIR /app`

`ENV NODE_ENV=production`

`COPY --from=build /app/.next/standalone ./`

`COPY --from=build /app/.next/static ./.next/static`

`COPY --from=build /app/public ./public`

`EXPOSE 3000`

`CMD ["node", "server.js"]`

The standalone output mode (set in next.config.ts via output: "standalone") produces a self-contained server.js that includes only the Node.js modules required to run Next.js without a node\_modules directory in the final image. This keeps the image small.

## **12.2 next.config.ts**

`// client/next.config.ts`

`const config: NextConfig = {`

  `output: "standalone",          // enables small Docker image`

  `reactStrictMode: true,`

  `poweredByHeader: false,         // remove X-Powered-By header`

  `async headers() {`

    `return [{`

      `source: "/(.*)",`

      `headers: [`

        `{ key: "X-Frame-Options",         value: "DENY" },`

        `{ key: "X-Content-Type-Options",  value: "nosniff" },`

        `{ key: "Referrer-Policy",          value: "strict-origin-when-cross-origin" },`

      `]`

    `}];`

  `},`

`};`

## **12.3 Environment Variables**

The web container receives configuration via environment variables set in docker-compose.yml from the .env file. Next.js exposes variables to the browser only if prefixed with NEXT\_PUBLIC\_. Variables without this prefix are server-side only.

| Variable | Usage |
| :---- | :---- |
| NEXT\_PUBLIC\_APP\_NAME | Displayed in the UI — public |
| NEXT\_PUBLIC\_KEYCLOAK\_URL | Keycloak OIDC endpoint — public (needed by browser for PKCE redirect) |
| NEXT\_PUBLIC\_KEYCLOAK\_REALM | Keycloak realm name — public |
| NEXT\_PUBLIC\_KEYCLOAK\_CLIENT\_ID | OIDC client ID — public |
| NODE\_ENV | production / development — server-side only |

| Note: No API URL Variable There is no NEXT\_PUBLIC\_API\_URL variable. The API is always at the same origin as the page — this is enforced by the YARP topology. All fetch() calls use relative URLs (/api/...). Hardcoding an absolute API URL would break the same-origin design and reintroduce CORS complexity. |
| :---- |

# **13\. Testing Strategy**

The client testing strategy mirrors the test pyramid applied to the .NET backend. Tests are colocated with the code they test.

## **13.1 Test Layers**

| Layer | What Is Tested | Tools |
| :---- | :---- | :---- |
| Unit: Hooks & Utils | TanStack Query hooks, Zustand stores, utility functions, auth logic. No DOM rendering. | Vitest, React Testing Library (renderHook) |
| Component: Rendering | Individual components render correctly with given props. Form validation. Accessibility. | Vitest, React Testing Library, axe-core |
| Integration: User Flows | Full page interactions: fill form, submit, see result. API calls mocked. | Vitest, React Testing Library, MSW (Mock Service Worker) |
| E2E (optional) | Full stack against running compose stack. Real browser, real API. | Playwright |

## **13.2 Mock Service Worker**

MSW (Mock Service Worker) intercepts fetch() calls at the network level in tests. This means tests exercise the real TanStack Query hooks and the real apiFetch() wrapper — only the network is mocked. MSW handlers are defined in tests/mocks/handlers.ts and mirror the .NET API contract.

## **13.3 Test Naming Convention**

Component and hook tests follow the same naming convention as the .NET tests: {Subject}\_{Scenario}\_{ExpectedResult}

`CreateInvoiceForm_WhenSubmittedWithInvalidAmount_ShowsValidationError()`

`useInvoices_WhenApiReturns401_RedirectsToLogin()`

# **14\. CI/CD Integration**

The web client is built and tested as part of the same CI/CD pipeline as the .NET backend. There is no separate pipeline. The System Architecture Reference Section 11 describes the full pipeline structure. This section covers only the client-specific steps.

## **14.1 Client Steps in ci.yml**

6. npm ci (dependencies installed from package-lock.json, cached by lock file hash)

7. npm run lint (ESLint with TypeScript-aware rules)

8. npx tsc \--noEmit (TypeScript type check without producing output)

9. npm run test (Vitest unit and component tests)

10. docker build client/ (validates the Dockerfile produces a valid image)

## **14.2 Client Steps in cd-release.yml**

11. docker build client/ \--tag yourapp/web:{version}

12. docker push to GHCR

13. docker save yourapp/web:{version} \-o web.tar (included in release ZIP for air-gapped installs)

## **14.3 Type Safety Across the Boundary**

The TypeScript types in lib/types/ must be kept in sync with the .NET DTOs. In the initial version this is a manual process enforced by code review. A future improvement is to enable Swagger/OpenAPI generation in the .NET API and run openapi-typescript in the CI pipeline to generate lib/types/ automatically, failing the build if the generated types differ from the committed ones.

# **15\. Extending the Client: Adding a New Feature**

This section is the procedural guide for adding a new feature to the web client. It mirrors the module addition checklist in the System Architecture Reference Section 15\.

## **15.1 Adding a New Page**

14. Create the route folder under app/(app)/\[module\]/\[feature\]/

15. Create page.tsx as a server component — static shell only

16. Create the interactive content as a client component in components/ or colocated in the route folder

17. Add TanStack Query hooks in lib/api/\[module\].ts if new API endpoints are needed

18. Add TypeScript DTO types in lib/types/\[module\].ts

19. Add the route to the navigation in components/layout/

20. Add MSW mock handlers in tests/mocks/handlers.ts for the new endpoints

21. Write component tests

## **15.2 Adding a New Form**

22. Define the Zod schema alongside the form component

23. Use useForm from React Hook Form with zodResolver

24. Wire submission to a useMutation hook — never raw fetch()

25. Map API validation errors (ProblemDetails) to form field errors in onError

26. Test: submit with invalid data, confirm error display; submit with valid data, confirm mutation called

## **15.3 What You Must Not Do**

* Do not call fetch() directly in a component — use TanStack Query

* Do not create Next.js API routes (app/api/) — the .NET backend is the API

* Do not use Server Actions — mutations go through the API layer

* Do not store access tokens in localStorage or sessionStorage

* Do not add inline styles — use Tailwind classes

* Do not import from another module's components/ subfolder — shared components go in components/shared/

* Do not skip the TypeScript type definition for new API shapes

# **16\. Architecture Decision Records (ADRs)**

Client-specific ADRs are stored in docs/adr/ alongside the backend ADRs. They follow the same template defined in the System Architecture Reference Section 17\. The initial decisions captured by this document are:

| ADR | Decision |
| :---- | :---- |
| ADR-C001 | Next.js App Router over Vite SPA — SSR benefits, same container model |
| ADR-C002 | shadcn/ui over a traditional component library — owned components, design flexibility |
| ADR-C003 | TanStack Query for all server state — caching, consistency, no raw fetch |
| ADR-C004 | Zustand for global client state — minimal, no boilerplate, TypeScript-native |
| ADR-C005 | React Hook Form \+ Zod for all forms — performance, type safety |
| ADR-C006 | React Flow for diagram views — production-proven, extensible |
| ADR-C007 | Tiptap for document editing — headless, ProseMirror foundation, Y.js-ready |
| ADR-C008 | PWA over Tauri/Electron — one codebase, no native packaging, sufficient capability |
| ADR-C009 | No Next.js API routes or Server Actions — .NET is the authoritative API |
| ADR-C010 | Access tokens in memory only — security over convenience |
| ADR-C011 | MSW for API mocking in tests — tests real fetch/query behaviour |
| ADR-C012 | Vitest over Jest — Vite-native, faster, same API |

# **Appendix A: Technology Quick Reference**

| Concern | Technology | Version Policy | Notes |
| :---- | :---- | :---- | :---- |
| Framework | Next.js 15 (App Router) | Pin major version | output: standalone for Docker |
| Language | TypeScript 5 | Latest stable | strict mode enabled |
| Build tool | Turbopack (via Next.js) | Bundled with Next.js | No separate Webpack config |
| Styling | Tailwind CSS 3 | Pin minor version | No CSS modules, no inline styles |
| Components | shadcn/ui \+ Radix UI | Manually updated | Owned in components/ui/ |
| Forms | React Hook Form 7 | Latest stable | Always paired with Zod |
| Validation | Zod 3 | Latest stable | Client-side mirror of server validators |
| Server state | TanStack Query 5 | Latest stable | All API calls through this |
| Client state | Zustand 4 | Latest stable | Auth, SignalR status, UI prefs |
| Diagrams | React Flow 11 | Pin minor version | Node-based editable diagrams |
| Rich text | Tiptap 2 | Pin minor version | Headless, ProseMirror-based |
| Animations | Framer Motion 11 | Latest stable | Presentation transitions, micro-interactions |
| Real-time | @microsoft/signalr 8 | Match .NET version | Managed by connectionManager.ts |
| Auth | keycloak-js | Match Keycloak server version | PKCE flow only |
| HTTP client | native fetch() via apiFetch wrapper | N/A | No Axios |
| Testing: unit | Vitest \+ React Testing Library | Latest stable |  |
| Testing: mocks | MSW 2 | Latest stable | Network-level API mocking |
| Testing: E2E | Playwright | Latest stable | Optional, against compose stack |
| PWA | next-pwa or vite-plugin-pwa | Latest stable | Service worker \+ manifest |
| Linting | ESLint \+ eslint-config-next | Bundled with Next.js |  |
| Formatting | Prettier | Latest stable | Enforced in CI |

*— End of Document —*