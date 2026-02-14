---
name: react-converter
description: Convert UI prototypes (HTML/CSS) into modern React applications with TypeScript, proper component architecture, and best practices. Specializes in creating production-ready React code from mockups and prototypes.
allowed-tools: Read, Grep, Glob, Bash, Write, Edit, AskUserQuestion
---

# React Converter Skill

## Purpose

Transform UI prototypes and mockups into production-ready React applications:
- Convert HTML prototypes to React components
- Implement proper React component architecture
- Add TypeScript support with proper typing
- Set up modern styling solutions (CSS Modules, Tailwind, styled-components)
- Create reusable component libraries
- Implement React hooks and state management
- Follow React best practices and patterns
- Ensure accessibility (WCAG compliance)
- Generate proper project structure

## When to Use

Invoke this skill when you need to:
- "Convert this HTML prototype to React"
- "Turn this UI mockup into React components"
- "Create a React app from this prototype"
- "Migrate HTML/CSS to React with TypeScript"
- "Build React components from this design"
- "Convert this dashboard to a React application"
- "Transform this static site into React"
- "Create React version of this prototype"

## Analysis Steps

1. **UNDERSTAND SOURCE MATERIAL**
   - Identify source files (HTML, CSS, images, JavaScript)
   - Analyze UI structure and component hierarchy
   - Identify interactive elements and state requirements
   - Review existing functionality and behaviors
   - Note styling approach used in prototype

2. **AUDIT CSS ENVIRONMENT** ⚠️ CRITICAL FOR EXISTING PROJECTS
   - **Read ALL CSS files** (index.css, App.css, globals.css, etc.)
   - **Identify global selectors** that may cause conflicts:
     - Universal selector: `*`
     - Element selectors: `button`, `input`, `div`, `body`
     - Utility classes that override component styles
   - **Document potential conflicts** before starting conversion
   - **Check for CSS frameworks** (Tailwind, Bootstrap) that may conflict
   - **Plan isolation strategy** (!important, CSS modules, scoped styles)

3. **GATHER REQUIREMENTS**
   - Ask about TypeScript preference (recommended)
   - Determine styling approach (Tailwind, CSS Modules, styled-components)
   - Identify state management needs (useState, Context, Zustand, Redux)
   - Check if part of existing React project or new standalone app
   - Ask about additional features (routing, forms, data fetching)
   - Determine target React version (default to latest)

4. **PLAN COMPONENT ARCHITECTURE**
   - Break down UI into component hierarchy
   - Identify reusable components
   - Determine component types (presentational vs. container)
   - Plan props interfaces and state management
   - Identify shared utilities and hooks
   - Plan folder structure (components, hooks, utils, types)

5. **CHECK PROJECT CONTEXT**
   - Look for existing React project structure
   - Check for package.json and dependencies
   - Review existing components for consistency
   - Identify naming conventions and patterns
   - Check for existing TypeScript configuration
   - Review existing styling setup

6. **SETUP PROJECT STRUCTURE**
   - Initialize React project if needed (Vite recommended)
   - Install necessary dependencies
   - Configure TypeScript if requested
   - Setup styling solution
   - Create folder structure
   - Configure ESLint/Prettier if requested

7. **CONVERT TO REACT COMPONENTS**
   - Create component files with proper naming
   - Extract HTML into JSX
   - Convert class names to camelCase where needed
   - Add TypeScript interfaces for props
   - Implement state management with hooks
   - Extract inline styles to appropriate solution
   - Add proper imports and exports

8. **IMPLEMENT INTERACTIVITY**
   - Convert vanilla JS to React hooks
   - Implement event handlers
   - Add form handling with controlled components
   - Create custom hooks for reusable logic
   - Implement data fetching if needed
   - Add loading and error states

9. **ENSURE QUALITY & DEBUG CSS**
   - Follow React best practices
   - Implement proper TypeScript typing
   - Ensure accessibility (ARIA, semantic HTML)
   - Add PropTypes or TypeScript validation
   - Optimize performance (useMemo, useCallback where needed)
   - Add helpful code comments
   - Ensure responsive design is maintained
   - **Use Browser DevTools to verify styles match prototype**
   - **Check for CSS conflicts and overrides**

## Project Setup Options

### Option 1: Vite (Recommended for New Projects)
```bash
npm create vite@latest my-app -- --template react-ts
cd my-app
npm install
```

### Option 2: Existing Project
- Add components to existing structure
- Follow existing patterns and conventions
- Match existing styling approach

### Option 3: Component Library
- Create standalone component files
- Export components for reuse
- Include Storybook setup if requested

## Component Architecture Patterns

> **Note:** For recommended project structure and folder organization, see the [Project Structure](../../references/REACT-REFERENCE.md#project-structure) section in REACT-REFERENCE.md.

## TypeScript Patterns

### Props Interface
```typescript
interface ButtonProps {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary' | 'outline';
  size?: 'sm' | 'md' | 'lg';
  onClick?: () => void;
  disabled?: boolean;
  className?: string;
}

export const Button: React.FC<ButtonProps> = ({
  children,
  variant = 'primary',
  size = 'md',
  onClick,
  disabled = false,
  className = ''
}) => {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      className={`btn btn-${variant} btn-${size} ${className}`}
    >
      {children}
    </button>
  );
};
```

### Component with State
```typescript
interface FormData {
  email: string;
  password: string;
}

export const LoginForm: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    email: '',
    password: ''
  });
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    // Handle submission
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Form fields */}
    </form>
  );
};
```

## Styling Approaches

### Tailwind CSS (Recommended for Prototypes)
```typescript
export const Card: React.FC<CardProps> = ({ title, children }) => {
  return (
    <div className="rounded-lg border border-slate-200 bg-white shadow-sm">
      <div className="p-6">
        <h3 className="text-lg font-semibold">{title}</h3>
        <div className="mt-2">{children}</div>
      </div>
    </div>
  );
};
```

### CSS Modules
```typescript
import styles from './Card.module.css';

export const Card: React.FC<CardProps> = ({ title, children }) => {
  return (
    <div className={styles.card}>
      <div className={styles.cardContent}>
        <h3 className={styles.cardTitle}>{title}</h3>
        <div className={styles.cardBody}>{children}</div>
      </div>
    </div>
  );
};
```

### styled-components
```typescript
import styled from 'styled-components';

const StyledCard = styled.div`
  border-radius: 0.5rem;
  border: 1px solid #e2e8f0;
  background-color: white;
  box-shadow: 0 1px 2px 0 rgb(0 0 0 / 0.05);
`;

export const Card: React.FC<CardProps> = ({ title, children }) => {
  return (
    <StyledCard>
      <h3>{title}</h3>
      {children}
    </StyledCard>
  );
};
```

## React Hooks Patterns

### Custom Hook for Modal
```typescript
export const useModal = () => {
  const [isOpen, setIsOpen] = useState(false);

  const open = () => setIsOpen(true);
  const close = () => setIsOpen(false);
  const toggle = () => setIsOpen(prev => !prev);

  return { isOpen, open, close, toggle };
};

// Usage
const { isOpen, open, close } = useModal();
```

### Custom Hook for Form
```typescript
export const useForm = <T extends Record<string, any>>(initialValues: T) => {
  const [values, setValues] = useState<T>(initialValues);
  const [errors, setErrors] = useState<Partial<Record<keyof T, string>>>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
  };

  const reset = () => setValues(initialValues);

  return { values, errors, handleChange, setErrors, reset };
};
```

### Custom Hook for Data Fetching
```typescript
interface UseFetchResult<T> {
  data: T | null;
  loading: boolean;
  error: Error | null;
  refetch: () => void;
}

export const useFetch = <T,>(url: string): UseFetchResult<T> => {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const fetchData = async () => {
    try {
      setLoading(true);
      const response = await fetch(url);
      const json = await response.json();
      setData(json);
    } catch (err) {
      setError(err as Error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [url]);

  return { data, loading, error, refetch: fetchData };
};
```

## Conversion Workflow

### Step 1: Analyze Source
```bash
# Find HTML prototypes
glob **/*.html
glob **/prototype*.html

# Read source files
read prototype-dashboard.html

# Check for CSS files
glob **/*.css
```

### Step 2: Setup React Project (if new)
```bash
# Create Vite project with TypeScript
npm create vite@latest my-app -- --template react-ts
cd my-app

# Install Tailwind (if needed)
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# Or install other styling solution
npm install styled-components
npm install -D @types/styled-components
```

### Step 3: Create Component Structure
```bash
# Create directories
mkdir -p src/components/ui
mkdir -p src/components/layout
mkdir -p src/components/features
mkdir -p src/hooks
mkdir -p src/types
mkdir -p src/utils
```

### Step 4: Convert Components
- Extract HTML sections into separate components
- Convert to JSX syntax
- Add TypeScript interfaces
- Implement React hooks for interactivity
- Extract styles to chosen styling solution

### Step 5: Wire Up Application
- Create main App component
- Add routing if needed (React Router)
- Connect components together
- Implement state management
- Add data fetching

### Step 6: Test and Verify
```bash
# Run development server
npm run dev

# Build for production
npm run build

# Type check
npx tsc --noEmit

# Lint
npm run lint
```

## HTML to JSX Conversion Rules

### 1. Self-Closing Tags
```html
<!-- HTML -->
<input type="text">
<img src="...">
<br>

<!-- JSX -->
<input type="text" />
<img src="..." />
<br />
```

### 2. ClassName Instead of Class
```html
<!-- HTML -->
<div class="card">

<!-- JSX -->
<div className="card">
```

### 3. CamelCase Attributes
```html
<!-- HTML -->
<button onclick="handleClick()">
<label for="email">

<!-- JSX -->
<button onClick={handleClick}>
<label htmlFor="email">
```

### 4. Style Objects
```html
<!-- HTML -->
<div style="background-color: blue; font-size: 16px">

<!-- JSX -->
<div style={{ backgroundColor: 'blue', fontSize: '16px' }}>
```

### 5. Boolean Attributes
```html
<!-- HTML -->
<input disabled>

<!-- JSX -->
<input disabled={true} />
// or simply
<input disabled />
```

### 6. Comments
```html
<!-- HTML -->
<!-- This is a comment -->

<!-- JSX -->
{/* This is a comment */}
```

## State Management Patterns

### Local State (useState)
```typescript
const [count, setCount] = useState(0);
const [user, setUser] = useState<User | null>(null);
const [formData, setFormData] = useState({ name: '', email: '' });
```

### Context API
```typescript
interface ThemeContextType {
  theme: 'light' | 'dark';
  toggleTheme: () => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [theme, setTheme] = useState<'light' | 'dark'>('light');

  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context) throw new Error('useTheme must be used within ThemeProvider');
  return context;
};
```

### Zustand (Lightweight Alternative)
```typescript
import { create } from 'zustand';

interface StoreState {
  count: number;
  increment: () => void;
  decrement: () => void;
}

export const useStore = create<StoreState>((set) => ({
  count: 0,
  increment: () => set((state) => ({ count: state.count + 1 })),
  decrement: () => set((state) => ({ count: state.count - 1 })),
}));
```

## Accessibility Checklist

1. **Semantic HTML in JSX:**
   - [ ] Use semantic elements (header, nav, main, section, article, footer)
   - [ ] Proper heading hierarchy (h1 → h2 → h3)
   - [ ] Use button elements for clickable actions (not divs)
   - [ ] Use anchor tags for navigation

2. **ARIA Attributes:**
   - [ ] aria-label for icon-only buttons
   - [ ] aria-labelledby and aria-describedby for associations
   - [ ] aria-hidden for decorative elements
   - [ ] role attributes when semantic HTML isn't enough

3. **Keyboard Navigation:**
   - [ ] All interactive elements are keyboard accessible
   - [ ] Tab order is logical
   - [ ] Focus indicators are visible
   - [ ] Modal focus trapping works
   - [ ] Escape key closes modals/dropdowns

4. **Forms:**
   - [ ] All inputs have associated labels
   - [ ] Error messages are announced
   - [ ] Required fields are indicated
   - [ ] Form validation provides clear feedback

5. **Images and Media:**
   - [ ] All images have alt text
   - [ ] Decorative images have alt=""
   - [ ] Videos have captions

## Best Practices Checklist

1. **Component Design:**
   - [ ] Single Responsibility Principle (one component, one purpose)
   - [ ] Proper component composition
   - [ ] Reusable UI components extracted
   - [ ] Props are well-typed with TypeScript
   - [ ] Default props are provided where appropriate

2. **Performance:**
   - [ ] Use React.memo for expensive components
   - [ ] useMemo for expensive calculations
   - [ ] useCallback for function props
   - [ ] Lazy load routes and heavy components
   - [ ] Avoid unnecessary re-renders

3. **Code Quality:**
   - [ ] Consistent naming conventions
   - [ ] Proper file organization
   - [ ] No console.logs in production code
   - [ ] Error boundaries for error handling
   - [ ] TypeScript strict mode enabled

4. **State Management:**
   - [ ] State is colocated when possible
   - [ ] Lift state only when necessary
   - [ ] Use appropriate state solution (local, context, global)
   - [ ] Avoid prop drilling

5. **Styling:**
   - [ ] Consistent styling approach throughout
   - [ ] Responsive design maintained
   - [ ] No inline styles (unless dynamic)
   - [ ] CSS class naming is consistent

6. **Testing Readiness:**
   - [ ] Components are testable (pure, minimal side effects)
   - [ ] Data-testid attributes for complex queries
   - [ ] Separation of concerns (logic vs presentation)

## Common Conversion Patterns

### Modal Dialog
```typescript
interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

export const Modal: React.FC<ModalProps> = ({ isOpen, onClose, title, children }) => {
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };

    if (isOpen) {
      document.addEventListener('keydown', handleEscape);
      document.body.style.overflow = 'hidden';
    }

    return () => {
      document.removeEventListener('keydown', handleEscape);
      document.body.style.overflow = 'unset';
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="fixed inset-0 bg-black/50" onClick={onClose} />
      <div className="relative z-50 w-full max-w-lg rounded-lg bg-white p-6">
        <h2 className="text-xl font-semibold">{title}</h2>
        <button
          onClick={onClose}
          className="absolute right-4 top-4"
          aria-label="Close modal"
        >
          ×
        </button>
        <div className="mt-4">{children}</div>
      </div>
    </div>
  );
};
```

### Form with Validation
```typescript
interface LoginFormData {
  email: string;
  password: string;
}

export const LoginForm: React.FC = () => {
  const { values, errors, handleChange, setErrors } = useForm<LoginFormData>({
    email: '',
    password: ''
  });
  const [loading, setLoading] = useState(false);

  const validate = (): boolean => {
    const newErrors: Partial<Record<keyof LoginFormData, string>> = {};

    if (!values.email.includes('@')) {
      newErrors.email = 'Please enter a valid email';
    }
    if (values.password.length < 8) {
      newErrors.password = 'Password must be at least 8 characters';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    setLoading(true);
    // Handle submission
    setLoading(false);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label htmlFor="email" className="block text-sm font-medium">
          Email
        </label>
        <input
          id="email"
          name="email"
          type="email"
          value={values.email}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border px-3 py-2"
        />
        {errors.email && (
          <p className="mt-1 text-sm text-red-600">{errors.email}</p>
        )}
      </div>

      <div>
        <label htmlFor="password" className="block text-sm font-medium">
          Password
        </label>
        <input
          id="password"
          name="password"
          type="password"
          value={values.password}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border px-3 py-2"
        />
        {errors.password && (
          <p className="mt-1 text-sm text-red-600">{errors.password}</p>
        )}
      </div>

      <button
        type="submit"
        disabled={loading}
        className="w-full rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 disabled:opacity-50"
      >
        {loading ? 'Loading...' : 'Login'}
      </button>
    </form>
  );
};
```

### Data Table
```typescript
interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

interface UserTableProps {
  users: User[];
  onEdit: (user: User) => void;
  onDelete: (userId: number) => void;
}

export const UserTable: React.FC<UserTableProps> = ({ users, onEdit, onDelete }) => {
  const [sortField, setSortField] = useState<keyof User>('name');
  const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc');

  const sortedUsers = useMemo(() => {
    return [...users].sort((a, b) => {
      const aValue = a[sortField];
      const bValue = b[sortField];
      const modifier = sortDirection === 'asc' ? 1 : -1;
      return aValue > bValue ? modifier : -modifier;
    });
  }, [users, sortField, sortDirection]);

  const handleSort = (field: keyof User) => {
    if (field === sortField) {
      setSortDirection(prev => prev === 'asc' ? 'desc' : 'asc');
    } else {
      setSortField(field);
      setSortDirection('asc');
    }
  };

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th onClick={() => handleSort('name')} className="cursor-pointer px-6 py-3 text-left">
              Name {sortField === 'name' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th onClick={() => handleSort('email')} className="cursor-pointer px-6 py-3 text-left">
              Email {sortField === 'email' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th onClick={() => handleSort('role')} className="cursor-pointer px-6 py-3 text-left">
              Role {sortField === 'role' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th className="px-6 py-3 text-right">Actions</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200 bg-white">
          {sortedUsers.map(user => (
            <tr key={user.id}>
              <td className="px-6 py-4">{user.name}</td>
              <td className="px-6 py-4">{user.email}</td>
              <td className="px-6 py-4">{user.role}</td>
              <td className="px-6 py-4 text-right">
                <button
                  onClick={() => onEdit(user)}
                  className="mr-2 text-blue-600 hover:text-blue-800"
                >
                  Edit
                </button>
                <button
                  onClick={() => onDelete(user.id)}
                  className="text-red-600 hover:text-red-800"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
```

## Dependencies to Install

### Core React + TypeScript (Vite)
```bash
npm create vite@latest my-app -- --template react-ts
```

### Styling Options
```bash
# Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# styled-components
npm install styled-components
npm install -D @types/styled-components

# CSS Modules (included with Vite)
```

### Routing
```bash
npm install react-router-dom
npm install -D @types/react-router-dom
```

### State Management
```bash
# Zustand (recommended for medium apps)
npm install zustand

# Redux Toolkit (for complex apps)
npm install @reduxjs/toolkit react-redux
```

### Form Handling
```bash
# React Hook Form
npm install react-hook-form

# Zod for validation
npm install zod
```

### Data Fetching
```bash
# TanStack Query (React Query)
npm install @tanstack/react-query

# Axios
npm install axios
```

### UI Component Libraries (Optional)
```bash
# Radix UI (headless components)
npm install @radix-ui/react-dialog @radix-ui/react-dropdown-menu

# Headless UI
npm install @headlessui/react
```

## Error Prevention

1. **CRITICAL:** Always use TypeScript interfaces for props
2. **CRITICAL:** Convert class to className in JSX
3. **CRITICAL:** Self-close all void elements (img, input, br, etc.)
4. **CRITICAL:** Use camelCase for event handlers (onClick, onChange)
5. **CRITICAL:** Test the React app runs (npm run dev)
6. Ensure all imports are correct
7. Export components properly (named or default)
8. Handle all async operations with proper error states
9. Add key props to mapped elements
10. Validate TypeScript compiles (npx tsc --noEmit)

## Troubleshooting CSS Mismatches

### When Styles Don't Match Prototype

If the converted React app doesn't visually match the prototype, follow this systematic debugging process:

#### Step 1: Identify Global Style Conflicts

**Check for global CSS that may override your component styles:**

```bash
# Read all CSS files
read src/index.css
read src/App.css
read src/globals.css

# Look for global selectors
grep "^button" src/**/*.css
grep "^input" src/**/*.css
grep "^\*" src/**/*.css
```

**Common Culprits:**
```css
/* ❌ These global styles override component styles */
button {
  border-radius: 8px;
  padding: 0.6em 1.2em;
  background-color: #1a1a1a;
}

input {
  border: 1px solid #ccc;
  padding: 0.5rem;
}

* {
  box-sizing: border-box;
  margin: 0;
}
```

**Solution:**
```css
/* ✅ Use !important to override global styles (last resort) */
.microphone-button {
  background-color: #10b981 !important;
  border-radius: 50% !important;
  padding: 0 !important;
}

/* ✅ Or use CSS Modules for isolation */
/* Button.module.css */
.button {
  /* Styles automatically scoped */
}
```

#### Step 2: Use Browser DevTools

**CRITICAL: Always verify styles in the browser before debugging further**

1. Open your React app in browser
2. Right-click the element → "Inspect"
3. Check the "Styles" tab:
   - Look for **crossed-out properties** (overridden styles)
   - Check which file is overriding (shown on right side)
   - Verify the actual computed value
4. Check the "Computed" tab:
   - See the final calculated values
   - Verify what actually rendered
5. Use "Layout" tab:
   - Visualize flexbox/grid layouts
   - See actual dimensions and spacing

**Example Debugging Session:**
```
Element: <button class="microphone-button">

Styles Tab Shows:
  .microphone-button {
    background-color: #10b981;  ← Crossed out
  }
  button {
    background-color: #1a1a1a;  ← This wins (higher specificity)
  }

Solution: Add !important or increase specificity
```

#### Step 3: Check CSS Specificity

**Specificity hierarchy (lowest to highest):**
1. Element selectors: `button { }` (specificity: 0-0-1)
2. Class selectors: `.button { }` (specificity: 0-1-0)
3. ID selectors: `#button { }` (specificity: 1-0-0)
4. Inline styles: `style="..."` (specificity: 1-0-0-0)
5. `!important`: Overrides everything

**Common Issue:**
```css
/* Global style in index.css */
button {  /* specificity: 0-0-1 */
  border-radius: 8px;
}

/* Your component style */
.send-button {  /* specificity: 0-1-0 - Should win but doesn't if applied later */
  border-radius: 50%;
}
```

**Why it fails:** If global styles are loaded AFTER component styles, they can win despite lower specificity.

**Solutions:**
```css
/* Option 1: Use !important */
.send-button {
  border-radius: 50% !important;
}

/* Option 2: Increase specificity */
button.send-button {
  border-radius: 50%;
}

/* Option 3: Use CSS Modules (best) */
/* Button.module.css automatically scoped */
```

### Common CSS Pitfalls

#### Pitfall 1: width: 100% Prevents Flexbox Gap

**❌ WRONG:**
```css
.home-panels {
  display: flex;
  gap: 2rem;  /* This won't show! */
}

.left-panel {
  width: 100%;  /* ❌ Takes full width, ignoring gap */
  flex: 1;
}
```

**Why it fails:** When flex children have `width: 100%`, they try to take 100% of the parent's width, which overrides the gap spacing.

**✅ CORRECT:**
```css
.home-panels {
  display: flex;
  gap: 2rem;  /* ✅ Gap shows correctly */
}

.left-panel {
  /* width: 100% removed */
  flex: 1;  /* Automatically calculates width minus gap */
}
```

**Rule:** Never use `width: 100%` on flex children when parent has `gap`.

#### Pitfall 2: Fixed Dimensions Prevent Responsive Layouts

**❌ WRONG:**
```css
.container {
  width: 600px;   /* ❌ Fixed width */
  height: 800px;  /* ❌ Fixed height */
}
```

**✅ CORRECT:**
```css
.container {
  width: 100%;           /* Or use max-width */
  max-width: 600px;      /* Constrains but allows shrinking */
  height: 100%;          /* Or use min-height */
  min-height: 800px;     /* Sets minimum but allows growing */
}
```

#### Pitfall 3: Conflicting Flex Properties

**❌ WRONG:**
```css
.flex-child {
  flex: 1;
  flex-shrink: 0;  /* ❌ Conflicts with flex: 1 */
  width: 50%;      /* ❌ Overrides flex calculation */
}
```

**✅ CORRECT:**
```css
.flex-child {
  flex: 1;  /* Handles everything automatically */
}

/* Or be explicit */
.flex-child {
  flex-grow: 1;
  flex-shrink: 1;
  flex-basis: 0;
}
```

#### Pitfall 4: Padding/Border Not Included in Width

**❌ WRONG:**
```css
.container {
  width: 400px;
  padding: 20px;    /* Actual width: 440px */
  border: 1px solid;  /* Actual width: 442px */
}
```

**✅ CORRECT:**
```css
.container {
  width: 400px;
  padding: 20px;
  border: 1px solid;
  box-sizing: border-box;  /* ✅ Includes padding/border in width */
}
```

### Systematic CSS Debugging Checklist

When prototype doesn't match React app, check in this order:

- [ ] **Read all global CSS files** (index.css, App.css, globals.css)
- [ ] **Identify element selectors** (button, input, div, etc.)
- [ ] **Check browser DevTools** for crossed-out styles
- [ ] **Verify computed values** match prototype
- [ ] **Check for width: 100% with flexbox gap**
- [ ] **Verify box-sizing: border-box** is set
- [ ] **Check flex properties** aren't conflicting
- [ ] **Validate responsive breakpoints** match
- [ ] **Check z-index and positioning** context
- [ ] **Test in actual browser** (not just assuming it works)

### CSS Isolation Strategies

#### Strategy 1: CSS Modules (Recommended)

```tsx
// Button.module.css
.button {
  background-color: #10b981;
  /* Automatically scoped to component */
}

// Button.tsx
import styles from './Button.module.css';

export const Button = () => (
  <button className={styles.button}>Click me</button>
);
```

#### Strategy 2: Scoped Styles (styled-components)

```tsx
import styled from 'styled-components';

const StyledButton = styled.button`
  background-color: #10b981;
  /* Scoped to this component */
`;

export const Button = () => (
  <StyledButton>Click me</StyledButton>
);
```

#### Strategy 3: !important (Last Resort)

```css
/* Use when global styles cannot be changed */
.component-specific-button {
  background-color: #10b981 !important;
  border-radius: 50% !important;
}
```

## Quality Standards

Every React conversion should:
1. Use TypeScript with proper types
2. Follow React best practices and hooks rules
3. Maintain or improve accessibility
4. Keep responsive design intact
5. Have clean, readable code with comments
6. Use consistent naming conventions
7. Have proper error handling
8. Be performance-optimized
9. Follow the single responsibility principle
10. Be production-ready code

## Example Workflow

```bash
# STEP 1: Analyze source prototype
read prototype-dashboard.html
glob **/*.html

# STEP 2: Ask user for preferences
# Use AskUserQuestion for TypeScript, styling, routing needs

# STEP 3: Setup React project (if new)
bash npm create vite@latest dashboard-app -- --template react-ts

# STEP 4: Install dependencies
bash cd dashboard-app && npm install
bash npm install -D tailwindcss postcss autoprefixer
bash npx tailwindcss init -p

# STEP 5: Create component structure
bash mkdir -p src/components/{ui,layout,features}
bash mkdir -p src/{hooks,types,utils}

# STEP 6: Convert components
write src/components/ui/Card.tsx
write src/components/layout/Header.tsx
write src/components/features/Dashboard.tsx

# STEP 7: Setup types
write src/types/components.ts

# STEP 8: Create custom hooks
write src/hooks/useModal.ts

# STEP 9: Wire up App
write src/App.tsx

# STEP 10: Configure Tailwind
edit tailwind.config.js

# STEP 11: Test
bash npm run dev
```

## Tips for Success

1. **Start with Layout:** Convert layout components first (Header, Sidebar, Footer)
2. **Extract UI Components:** Create reusable UI components early
3. **One Component at a Time:** Don't try to convert everything at once
4. **Test Frequently:** Run `npm run dev` often to catch issues early
5. **TypeScript First:** Write interfaces before implementing components
6. **Keep It Simple:** Don't over-engineer; match the prototype's complexity
7. **Ask Questions:** Use AskUserQuestion for unclear decisions
8. **Follow Patterns:** Maintain consistency across all components
9. **Document Complex Logic:** Add comments for non-obvious code
10. **Think in React:** Use component composition, not direct DOM manipulation

---

See [REACT-REFERENCE.md](../../references/REACT-REFERENCE.md) for additional examples and component templates.
