# Frontend Component Guide

This guide documents available UI components and best practices for frontend development in Lex using shadcn/ui and custom components.

## Available Components

### shadcn/ui Components

These are pre-installed shadcn/ui components ready to use:

#### Button
```tsx
import { Button } from "@/components/ui/button";

// Variants
<Button>Default</Button>
<Button variant="destructive">Delete</Button>
<Button variant="outline">Outline</Button>
<Button variant="secondary">Secondary</Button>
<Button variant="ghost">Ghost</Button>
<Button variant="link">Link</Button>

// Sizes
<Button size="sm">Small</Button>
<Button size="default">Default</Button>
<Button size="lg">Large</Button>
<Button size="icon">🔍</Button>

// States
<Button disabled>Disabled</Button>
<Button isLoading>Loading...</Button>
```

#### Card
```tsx
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";

<Card>
  <CardHeader>
    <CardTitle>Card Title</CardTitle>
    <CardDescription>Card description goes here</CardDescription>
  </CardHeader>
  <CardContent>
    Card content
  </CardContent>
</Card>
```

#### Table
```tsx
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

<Table>
  <TableHeader>
    <TableRow>
      <TableHead>Header 1</TableHead>
      <TableHead>Header 2</TableHead>
    </TableRow>
  </TableHeader>
  <TableBody>
    <TableRow>
      <TableCell>Data 1</TableCell>
      <TableCell>Data 2</TableCell>
    </TableRow>
  </TableBody>
</Table>
```

### Additional Radix UI Components

These Radix UI primitives are available via shadcn/ui installation:

- **Dialog** — Modal dialogs
- **Dropdown Menu** — Context menus
- **Toast** — Toast notifications

**Import:**
```tsx
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
```

## Component Implementation Standards

### 1. Creating New UI Components

**File Location:** `components/ui/[component-name].tsx`

**Template:**
```tsx
'use client'; // If interactive

import * as React from "react";
import { cn } from "@/lib/utils";

interface ComponentProps extends React.HTMLAttributes<HTMLDivElement> {
  // Your props here
  variant?: "default" | "secondary";
  size?: "sm" | "md" | "lg";
}

const Component = React.forwardRef<HTMLDivElement, ComponentProps>(
  ({ className, variant = "default", size = "md", ...props }, ref) => (
    <div
      ref={ref}
      className={cn(
        "base-styles",
        variant === "default" && "variant-default",
        variant === "secondary" && "variant-secondary",
        size === "sm" && "size-sm",
        size === "md" && "size-md",
        size === "lg" && "size-lg",
        className
      )}
      {...props}
    />
  )
);

Component.displayName = "Component";
export { Component };
```

### 2. Creating Feature Components

**File Location:** `components/[module]/[feature]/[ComponentName].tsx`

**Structure:**
```tsx
'use client';

import { useState } from "react";
import { useAuth } from "@/lib/auth/useAuth";
import { Button } from "@/components/ui/button";

interface ComponentProps {
  itemId: string;
  onSuccess?: () => void;
}

export function FeatureComponent({ itemId, onSuccess }: ComponentProps) {
  const [isLoading, setIsLoading] = useState(false);
  const { hasPermission } = useAuth();

  if (!hasPermission("module.permission")) {
    return <div>Unauthorized</div>;
  }

  const handleAction = async () => {
    setIsLoading(true);
    try {
      // Do something
      onSuccess?.();
    } catch (error) {
      // Handle error
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Button onClick={handleAction} disabled={isLoading}>
      {isLoading ? "Loading..." : "Action"}
    </Button>
  );
}
```

### 3. Form Components

Use React Hook Form + Zod for validation:

```tsx
'use client';

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Button } from "@/components/ui/button";

const schema = z.object({
  title: z.string().min(1, "Title required").max(100),
  email: z.string().email("Invalid email"),
});

type FormData = z.infer<typeof schema>;

export function MyForm() {
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    console.log(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register("title")} />
      {errors.title && <span>{errors.title.message}</span>}
      
      <input {...register("email")} type="email" />
      {errors.email && <span>{errors.email.message}</span>}
      
      <Button type="submit">Submit</Button>
    </form>
  );
}
```

## Component Organization

### File Structure

```
components/
├── ui/                              # shadcn/ui + custom UI primitives
│   ├── button.tsx
│   ├── card.tsx
│   ├── table.tsx
│   └── dialog.tsx
│
├── common/                          # Reusable across features
│   ├── PageHeader.tsx              # Page title + breadcrumbs
│   ├── EmptyState.tsx              # Empty list placeholder
│   ├── LoadingSpinner.tsx
│   ├── ErrorBoundary.tsx
│   └── Navigation.tsx
│
├── auth/                           # Authentication-related
│   ├── LoginForm.tsx
│   ├── ProtectedRoute.tsx
│   └── PermissionGuard.tsx
│
├── lessons/                        # Lesson module components
│   ├── LessonList.tsx
│   ├── LessonCard.tsx
│   ├── LessonEditor.tsx
│   └── ResourceUpload.tsx
│
├── assessments/                    # Assessment module components
│   ├── AssessmentBuilder.tsx
│   ├── QuestionEditor.tsx
│   ├── QuestionList.tsx
│   ├── AssessmentPlayer.tsx        # Student assessment taker
│   └── GradingDashboard.tsx        # Teacher grading interface
│
├── diary/                          # Diary module components
│   ├── DiaryEntryList.tsx
│   ├── DiaryEntryEditor.tsx
│   └── ApprovalPanel.tsx
│
├── schedule/                       # Schedule module components
│   ├── ScheduleGrid.tsx
│   ├── TimetableView.tsx
│   └── PeriodDetail.tsx
│
├── notifications/                  # Notifications
│   ├── NotificationBell.tsx
│   ├── NotificationDrawer.tsx
│   └── NotificationList.tsx
│
└── admin/                          # Admin-specific components
    ├── UserManagementTable.tsx
    ├── ClassManagementPanel.tsx
    └── SystemHealthDashboard.tsx
```

## Styling Standards

### Tailwind CSS Classes

Use Tailwind utility classes consistently:

```tsx
// Good
<div className="flex items-center justify-between gap-4 p-4 rounded-lg border">
  <h2 className="text-lg font-semibold text-gray-900">Title</h2>
  <Button>Action</Button>
</div>

// Avoid custom CSS
<div style={{ display: 'flex', gap: '16px' }}>
```

### Color Palette

Use CSS variables defined in `globals.css`:

```tsx
// In components
<div className="bg-primary text-primary-foreground">Primary color</div>
<div className="bg-secondary text-secondary-foreground">Secondary</div>
<div className="bg-destructive text-destructive-foreground">Destructive</div>
<div className="border border-input">Input border</div>
<div className="bg-muted text-muted-foreground">Muted</div>
```

### Responsive Design

```tsx
// Mobile-first approach
<div className="w-full md:w-1/2 lg:w-1/3">
  <h1 className="text-lg md:text-xl lg:text-2xl">Responsive heading</h1>
</div>
```

## State Management

### Global State (Zustand)

For cross-cutting concerns like auth, notifications:

```tsx
// lib/store/authStore.ts
import { create } from "zustand";

interface AuthState {
  user: User | null;
  token: string | null;
  setUser: (user: User) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: null,
  setUser: (user) => set({ user }),
  logout: () => set({ user: null, token: null }),
}));

// In component
const { user, logout } = useAuthStore();
```

### Server State (TanStack Query)

For data from backend:

```tsx
import { useQuery } from "@tanstack/react-query";
import { apiFetch } from "@/lib/api/client";

export function useAssessments() {
  return useQuery({
    queryKey: ["assessments"],
    queryFn: () => apiFetch<Assessment[]>("/api/assessments"),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

// In component
export function AssessmentList() {
  const { data, isLoading, error } = useAssessments();

  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage error={error} />;
  if (!data?.length) return <EmptyState />;

  return data.map((assessment) => (
    <AssessmentCard key={assessment.id} assessment={assessment} />
  ));
}
```

### Local State (useState)

For transient UI state:

```tsx
const [isOpen, setIsOpen] = useState(false);
const [filters, setFilters] = useState({ sort: "date" });
```

## Data Fetching Patterns

### Using apiFetch

```tsx
// Single fetch
const data = await apiFetch<Assessment>("/api/assessments/123");

// With parameters
const assessments = await apiFetch<Assessment[]>("/api/assessments", {
  params: { page: 1, pageSize: 20, sortBy: "date" },
});

// POST request
const result = await apiFetch<CreatedItem>("/api/assessments", {
  method: "POST",
  body: { title: "New Assessment" },
});

// Error handling
try {
  const data = await apiFetch<Data>("/api/endpoint");
} catch (error) {
  if (error instanceof ApiError) {
    console.log(error.status); // HTTP status
    console.log(error.problem); // ProblemDetails
  }
}
```

### Using Mutations (TanStack Query)

```tsx
import { useMutation, useQueryClient } from "@tanstack/react-query";

export function useCreateAssessment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateAssessmentData) =>
      apiFetch("/api/assessments", { method: "POST", body: data }),
    onSuccess: () => {
      // Invalidate cache to refetch
      queryClient.invalidateQueries({ queryKey: ["assessments"] });
    },
  });
}

// In component
const createAssessment = useCreateAssessment();

const handleCreate = async (data: CreateAssessmentData) => {
  await createAssessment.mutateAsync(data);
  // Data refetched automatically
};
```

## Authorization & Permissions

### Permission Checks

```tsx
import { useAuth } from "@/lib/auth/useAuth";

export function AdminPanel() {
  const { hasPermission, user } = useAuth();

  if (!hasPermission("admin.manage")) {
    return <div>Unauthorized</div>;
  }

  return <div>Admin content</div>;
}
```

### Protected Components

```tsx
interface ProtectedComponentProps {
  policy: string;
  children: React.ReactNode;
  fallback?: React.ReactNode;
}

export function ProtectedComponent({
  policy,
  children,
  fallback,
}: ProtectedComponentProps) {
  const { hasPermission } = useAuth();

  if (!hasPermission(policy)) {
    return fallback || <div>No permission</div>;
  }

  return <>{children}</>;
}

// Usage
<ProtectedComponent policy="lessons.create">
  <CreateLessonButton />
</ProtectedComponent>
```

## Common Patterns

### Loading States

```tsx
<Button disabled={isLoading}>
  {isLoading ? (
    <>
      <LoadingSpinner className="mr-2" />
      Loading...
    </>
  ) : (
    "Submit"
  )}
</Button>
```

### Empty States

```tsx
function EmptyState() {
  return (
    <div className="flex flex-col items-center justify-center py-12">
      <EmptyBoxIcon className="h-12 w-12 text-gray-400 mb-4" />
      <h3 className="text-lg font-medium text-gray-900">No items</h3>
      <p className="text-sm text-gray-500">Get started by creating one</p>
      <Button className="mt-4">Create</Button>
    </div>
  );
}
```

### Error Handling

```tsx
function ErrorMessage({ error }: { error: Error }) {
  return (
    <div className="rounded-lg bg-red-50 p-4 text-red-800">
      <h3 className="font-semibold mb-1">Something went wrong</h3>
      <p className="text-sm">{error.message}</p>
    </div>
  );
}
```

## Testing Components

### Unit Tests (Vitest)

```tsx
import { render, screen } from "@testing-library/react";
import { MyComponent } from "./MyComponent";

describe("MyComponent", () => {
  it("renders correctly", () => {
    render(<MyComponent />);
    expect(screen.getByText("Expected text")).toBeInTheDocument();
  });
});
```

### E2E Tests (Playwright)

```typescript
import { test, expect } from "@playwright/test";

test("create assessment", async ({ page }) => {
  await page.goto("/assessments/create");
  await page.fill('input[name="title"]', "New Assessment");
  await page.click('button:has-text("Create")');
  await expect(page).toHaveURL("/assessments/123");
});
```

## Best Practices

1. **Use Client Components Sparingly**
   - Only use `'use client'` when necessary (interactivity, state, hooks)
   - Prefer server components for data fetching

2. **Prop Types**
   - Always define TypeScript interfaces
   - Export types for reusability

3. **Accessibility**
   - Use semantic HTML
   - Include ARIA attributes
   - Test with keyboard navigation

4. **Performance**
   - Memoize expensive components: `React.memo()`
   - Lazy load routes: `next/dynamic`
   - Optimize images

5. **Component Cohesion**
   - Keep components focused and single-purpose
   - Extract logic into custom hooks
   - Reuse UI components widely

6. **Naming Conventions**
   - Component files: PascalCase (Button.tsx)
   - Utility files: camelCase (useAuth.ts)
   - Folders: kebab-case (assessment-builder/)

## Adding New shadcn/ui Components

To add a new shadcn/ui component:

```bash
# Run init first (if not already done)
npx shadcn-ui@latest init

# Add component
npx shadcn-ui@latest add [component-name]

# Examples
npx shadcn-ui@latest add form
npx shadcn-ui@latest add input
npx shadcn-ui@latest add select
npx shadcn-ui@latest add tabs
npx shadcn-ui@latest add dropdown-menu
```

Available components: https://ui.shadcn.com/

## Troubleshooting

### Component not importing
- Check file path spelling
- Ensure component is exported

### Styling not applying
- Verify Tailwind CSS is processing the file
- Check for conflicting CSS
- Clear `.next/` build cache

### Permission denied errors
- Check `hasPermission()` before rendering
- Verify JWT token has required role

---

For more information, see:
- [shadcn/ui Documentation](https://ui.shadcn.com/)
- [Next.js Documentation](https://nextjs.org/docs)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [React Hook Form Documentation](https://react-hook-form.com/)
- [TanStack Query Documentation](https://tanstack.com/query/latest)
