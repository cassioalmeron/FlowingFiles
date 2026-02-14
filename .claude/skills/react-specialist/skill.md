---
name: react-specialist
description: Expert in React development with TypeScript. Specializes in building React applications, components, hooks, state management, and following React best practices. Use for any React-related development tasks.
allowed-tools: Read, Grep, Glob, Bash, Write, Edit, AskUserQuestion
---

# React Specialist Skill

## Purpose

Expert assistance for all React development tasks:
- Build React applications from scratch
- Create and refactor React components
- Implement custom hooks and state management
- Add features to existing React projects
- Debug React issues and optimize performance
- Setup and configure React projects
- Implement routing, forms, and data fetching
- Follow React best practices and patterns
- Ensure TypeScript type safety
- Optimize component performance

## When to Use

Invoke this skill for any React development task:
- "Build a React dashboard application"
- "Create a custom hook for authentication"
- "Add a new feature to my React app"
- "Refactor this component to use hooks"
- "Setup React Router in my app"
- "Implement form validation with React Hook Form"
- "Add state management with Zustand"
- "Create a reusable component library"
- "Debug this React performance issue"
- "Setup a new React + TypeScript project"

## ⚠️ CRITICAL: Read Reference Documentation First

**BEFORE implementing ANY patterns or components, you MUST read:**
- `../../references/REACT-REFERENCE.md` - Contains project-specific patterns and conventions

This reference file defines the EXACT patterns to use (e.g., icon organization in single file, component structure, folder organization). Do NOT assume patterns from general React knowledge - always follow the documented patterns in REACT-REFERENCE.md.

**This is a mandatory prerequisite - not optional!**

## Analysis Steps

1. **UNDERSTAND THE TASK**
   - Clarify the React development goal
   - Identify if it's a new project or existing codebase
   - Determine required features and functionality
   - Ask about technical requirements and constraints

2. **ANALYZE EXISTING CODEBASE**
   - **FIRST: Read ../../references/REACT-REFERENCE.md for project-specific patterns**
   - Check for existing React project structure
   - Review package.json for dependencies
   - Identify current React patterns and conventions
   - Check TypeScript configuration
   - Review existing components and architecture
   - Understand current state management approach

3. **GATHER REQUIREMENTS**
   - Ask about TypeScript preference (recommended)
   - Determine styling approach (Tailwind, CSS Modules, styled-components)
   - Identify state management needs (local, Context, Zustand, Redux)
   - Check routing requirements (React Router)
   - Ask about form handling needs
   - Determine data fetching strategy
   - Identify testing requirements

4. **PLAN ARCHITECTURE**
   - Design component hierarchy
   - Plan state management strategy
   - Determine folder structure
   - Identify reusable components
   - Plan custom hooks
   - Design data flow
   - Consider performance optimizations

5. **IMPLEMENT SOLUTION**
   - Setup or update project configuration
   - Install necessary dependencies
   - Create component structure
   - Implement components with TypeScript
   - Add custom hooks
   - Setup routing if needed
   - Implement state management
   - Add styling
   - Ensure accessibility

6. **ENSURE QUALITY**
   - Follow React best practices
   - Implement proper TypeScript typing
   - Optimize performance (memo, callback, lazy loading)
   - Ensure accessibility (ARIA, semantic HTML)
   - Add error handling
   - Write clean, maintainable code
   - Add helpful comments where needed

## React Project Setup

### New Project with Vite (Recommended)
```bash
# Create React + TypeScript project
npm create vite@latest my-app -- --template react-ts
cd my-app
npm install

# Install common dependencies
npm install react-router-dom
npm install -D @types/react-router-dom

# Install Tailwind (optional)
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

### Existing Project
- Follow existing patterns and conventions
- Match current styling approach
- Use existing state management
- Maintain consistent code style

## Component Patterns

### Functional Component with TypeScript
```typescript
import React from 'react';

export interface ButtonProps {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary';
  onClick?: () => void;
  disabled?: boolean;
}

export const Button: React.FC<ButtonProps> = ({
  children,
  variant = 'primary',
  onClick,
  disabled = false
}) => {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      className={`btn btn-${variant}`}
    >
      {children}
    </button>
  );
};
```

### Component with State
```typescript
import React, { useState } from 'react';

export const Counter: React.FC = () => {
  const [count, setCount] = useState(0);

  const increment = () => setCount(prev => prev + 1);
  const decrement = () => setCount(prev => prev - 1);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>+</button>
      <button onClick={decrement}>-</button>
    </div>
  );
};
```

### Component with Effects
```typescript
import React, { useState, useEffect } from 'react';

interface User {
  id: number;
  name: string;
}

export const UserProfile: React.FC<{ userId: number }> = ({ userId }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await fetch(`/api/users/${userId}`);
        const data = await response.json();
        setUser(data);
      } catch (error) {
        console.error('Failed to fetch user', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, [userId]);

  if (loading) return <div>Loading...</div>;
  if (!user) return <div>User not found</div>;

  return <div>{user.name}</div>;
};
```

## Custom Hooks

### useToggle
```typescript
import { useState, useCallback } from 'react';

export const useToggle = (initialValue = false) => {
  const [value, setValue] = useState(initialValue);

  const toggle = useCallback(() => setValue(v => !v), []);
  const setTrue = useCallback(() => setValue(true), []);
  const setFalse = useCallback(() => setValue(false), []);

  return [value, toggle, setTrue, setFalse] as const;
};

// Usage
const [isOpen, toggleOpen, open, close] = useToggle(false);
```

### useForm
```typescript
import { useState, ChangeEvent } from 'react';

export const useForm = <T extends Record<string, any>>(initialValues: T) => {
  const [values, setValues] = useState<T>(initialValues);

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setValues(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const reset = () => setValues(initialValues);

  return { values, handleChange, reset, setValues };
};
```

### useLocalStorage
```typescript
import { useState, useEffect } from 'react';

export const useLocalStorage = <T,>(key: string, initialValue: T) => {
  const [value, setValue] = useState<T>(() => {
    try {
      const item = localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch {
      return initialValue;
    }
  });

  useEffect(() => {
    try {
      localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error('Failed to save to localStorage', error);
    }
  }, [key, value]);

  return [value, setValue] as const;
};
```

## State Management

### Local State (useState)
Best for: Component-specific state
```typescript
const [count, setCount] = useState(0);
const [user, setUser] = useState<User | null>(null);
```

### Context API
Best for: Sharing state across multiple components
```typescript
import { createContext, useContext, useState } from 'react';

interface AuthContextType {
  user: User | null;
  login: (user: User) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  const login = (user: User) => setUser(user);
  const logout = () => setUser(null);

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within AuthProvider');
  return context;
};
```

### Zustand (Recommended for Global State)
```bash
npm install zustand
```

```typescript
import { create } from 'zustand';

interface StoreState {
  count: number;
  user: User | null;
  increment: () => void;
  decrement: () => void;
  setUser: (user: User | null) => void;
}

export const useStore = create<StoreState>((set) => ({
  count: 0,
  user: null,
  increment: () => set((state) => ({ count: state.count + 1 })),
  decrement: () => set((state) => ({ count: state.count - 1 })),
  setUser: (user) => set({ user }),
}));

// Usage
const count = useStore(state => state.count);
const increment = useStore(state => state.increment);
```

## Routing with React Router

```bash
npm install react-router-dom
```

```typescript
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
        <Link to="/users">Users</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/users" element={<Users />} />
        <Route path="/users/:id" element={<UserDetail />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}
```

### Protected Routes
```typescript
import { Navigate } from 'react-router-dom';
import { useAuth } from './hooks/useAuth';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

// Usage
<Route path="/dashboard" element={
  <ProtectedRoute>
    <Dashboard />
  </ProtectedRoute>
} />
```

## Form Handling

### React Hook Form (Recommended)
```bash
npm install react-hook-form
npm install zod @hookform/resolvers
```

```typescript
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

const schema = z.object({
  email: z.string().email('Invalid email'),
  password: z.string().min(8, 'Password must be at least 8 characters'),
});

type FormData = z.infer<typeof schema>;

export const LoginForm: React.FC = () => {
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = (data: FormData) => {
    console.log(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register('email')} type="email" />
      {errors.email && <span>{errors.email.message}</span>}

      <input {...register('password')} type="password" />
      {errors.password && <span>{errors.password.message}</span>}

      <button type="submit">Login</button>
    </form>
  );
};
```

## Data Fetching

### TanStack Query (React Query)
```bash
npm install @tanstack/react-query
```

```typescript
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

// Fetch data
export const useUsers = () => {
  return useQuery({
    queryKey: ['users'],
    queryFn: async () => {
      const response = await fetch('/api/users');
      return response.json();
    },
  });
};

// Mutate data
export const useCreateUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (user: User) => {
      const response = await fetch('/api/users', {
        method: 'POST',
        body: JSON.stringify(user),
      });
      return response.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });
};

// Usage
const { data, isLoading, error } = useUsers();
const createUser = useCreateUser();
```

## Performance Optimization

### React.memo
```typescript
export const ExpensiveComponent = React.memo<Props>(({ data }) => {
  // Only re-renders if props change
  return <div>{/* expensive render */}</div>;
});
```

### useMemo
```typescript
const sortedUsers = useMemo(() => {
  return users.sort((a, b) => a.name.localeCompare(b.name));
}, [users]);
```

### useCallback
```typescript
const handleClick = useCallback(() => {
  console.log('clicked');
}, []);
```

### Lazy Loading
```typescript
import { lazy, Suspense } from 'react';

const HeavyComponent = lazy(() => import('./HeavyComponent'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <HeavyComponent />
    </Suspense>
  );
}
```

## Best Practices Checklist

> **CRITICAL:** You MUST read [REACT-REFERENCE.md](../../references/REACT-REFERENCE.md) for the required project structure, component patterns, and folder organization BEFORE creating any components. The reference file contains mandatory patterns that override general React conventions, including:
> - **Icons:** All icons must be in a single `icons.tsx` file
> - **User Notifications:** Always use **react-toastify** for toast notifications (never use alert() or custom systems)
> - Component structure, folder organization, and other project-specific patterns

1. **TypeScript:**
   - [ ] Use TypeScript for all components
   - [ ] Define proper interfaces for props
   - [ ] Avoid using `any` type
   - [ ] Use type inference where appropriate

2. **Components:**
   - [ ] Keep components small and focused
   - [ ] Use functional components with hooks
   - [ ] Extract reusable components
   - [ ] Proper prop validation

3. **State Management:**
   - [ ] Colocate state when possible
   - [ ] Lift state only when needed
   - [ ] Use appropriate state solution
   - [ ] Avoid prop drilling

4. **Performance:**
   - [ ] Use React.memo for expensive components
   - [ ] Implement useMemo for expensive calculations
   - [ ] Use useCallback for function props
   - [ ] Lazy load heavy components

5. **Accessibility:**
   - [ ] Use semantic HTML
   - [ ] Add ARIA labels where needed
   - [ ] Ensure keyboard navigation
   - [ ] Maintain focus management

6. **Code Quality:**
   - [ ] Follow consistent naming conventions
   - [ ] Add helpful comments
   - [ ] Handle errors properly
   - [ ] Keep code DRY (Don't Repeat Yourself)

## Common Dependencies

```json
{
  "dependencies": {
    "react": "^18.3.1",
    "react-dom": "^18.3.1",
    "react-router-dom": "^6.20.0",
    "@tanstack/react-query": "^5.17.0",
    "zustand": "^4.4.7",
    "react-hook-form": "^7.49.0",
    "zod": "^3.22.4"
  },
  "devDependencies": {
    "@types/react": "^18.3.1",
    "@types/react-dom": "^18.3.0",
    "typescript": "^5.3.3",
    "vite": "^5.0.8",
    "tailwindcss": "^3.4.0"
  }
}
```

## Workflow Example

```bash
# STEP 0: READ REFERENCE DOCUMENTATION FIRST (MANDATORY!)
read ../../references/REACT-REFERENCE.md

# STEP 1: Analyze existing project
glob src/**/*.tsx
read package.json
read tsconfig.json

# STEP 2: Ask user for requirements
# Use AskUserQuestion for features, styling, state management

# STEP 3: Install dependencies (if needed)
bash npm install react-router-dom zustand

# STEP 4: Create component structure (follow REACT-REFERENCE.md patterns)
bash mkdir -p src/components/{ui,layout,features}
bash mkdir -p src/{hooks,types,pages}

# STEP 5: Implement components (using patterns from REACT-REFERENCE.md)
write src/components/ui/Button.tsx
write src/hooks/useAuth.ts

# STEP 6: Setup routing (if needed)
edit src/App.tsx

# STEP 7: Test the application
bash npm run dev
```

## Error Prevention

1. **CRITICAL:** Read ../../references/REACT-REFERENCE.md BEFORE implementing any patterns
2. **CRITICAL:** Always use TypeScript interfaces for props
3. **CRITICAL:** Import React types correctly
4. **CRITICAL:** Handle async operations with proper error states
5. **CRITICAL:** Use react-toastify for user notifications (never use alert() or console.log())
6. **CRITICAL:** Add keys to mapped elements
7. Export components properly (named or default)
8. Use useEffect dependencies correctly
9. Avoid infinite loops in effects
10. Handle edge cases (loading, error, empty states)
11. Clean up effects (return cleanup function)
12. Follow hooks rules (only at top level, only in React functions)

## Tips for Success

1. **Read the Reference First:** Always consult REACT-REFERENCE.md before starting
2. **Start Simple:** Build incrementally, test frequently
3. **Think in Components:** Break UI into reusable pieces
4. **Type Everything:** Use TypeScript for better DX
5. **Custom Hooks:** Extract reusable logic
6. **Performance:** Optimize only when needed
7. **Accessibility:** Build it in from the start
8. **Error Handling:** Always handle loading and error states
9. **Code Organization:** Keep related files together
10. **Documentation:** Comment complex logic
11. **Testing:** Write testable code

---

**REMEMBER:** Always consult [REACT-REFERENCE.md](../../references/REACT-REFERENCE.md) BEFORE implementing any components. It contains the authoritative patterns and conventions for this project that must be followed.
