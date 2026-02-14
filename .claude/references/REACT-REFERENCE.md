# React Converter Reference Guide

Complete reference for converting UI prototypes to React applications with TypeScript.

## Table of Contents
- [Project Setup](#project-setup)
- [Project Structure](#project-structure)
- [Icons Pattern](#icons-pattern)
- [Component Templates](#component-templates)
- [Hook Patterns](#hook-patterns)
- [User Notifications](#user-notifications)
- [Styling Solutions](#styling-solutions)
- [Common Conversions](#common-conversions)
- [Advanced Patterns](#advanced-patterns)

---

## Project Setup

### Vite React TypeScript Template

```bash
# Create new project
npm create vite@latest my-app -- --template react-ts
cd my-app
npm install

# Add Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# Add common project dependencies
npm install react-router-dom react-toastify
```

### Tailwind Configuration

**tailwind.config.js:**
```javascript
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        border: "hsl(214.3 31.8% 91.4%)",
        input: "hsl(214.3 31.8% 91.4%)",
        ring: "hsl(222.2 84% 4.9%)",
        background: "hsl(0 0% 100%)",
        foreground: "hsl(222.2 84% 4.9%)",
        primary: {
          DEFAULT: "hsl(222.2 47.4% 11.2%)",
          foreground: "hsl(210 40% 98%)",
        },
        secondary: {
          DEFAULT: "hsl(210 40% 96.1%)",
          foreground: "hsl(222.2 47.4% 11.2%)",
        },
        muted: {
          DEFAULT: "hsl(210 40% 96.1%)",
          foreground: "hsl(215.4 16.3% 46.9%)",
        },
        accent: {
          DEFAULT: "hsl(210 40% 96.1%)",
          foreground: "hsl(222.2 47.4% 11.2%)",
        },
        destructive: {
          DEFAULT: "hsl(0 84.2% 60.2%)",
          foreground: "hsl(210 40% 98%)",
        },
      },
      borderRadius: {
        lg: "0.5rem",
        md: "0.375rem",
        sm: "0.25rem",
      },
    },
  },
  plugins: [],
}
```

**src/index.css:**
```css
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer base {
  :root {
    --background: 0 0% 100%;
    --foreground: 222.2 84% 4.9%;
    --card: 0 0% 100%;
    --card-foreground: 222.2 84% 4.9%;
    --popover: 0 0% 100%;
    --popover-foreground: 222.2 84% 4.9%;
    --primary: 222.2 47.4% 11.2%;
    --primary-foreground: 210 40% 98%;
    --secondary: 210 40% 96.1%;
    --secondary-foreground: 222.2 47.4% 11.2%;
    --muted: 210 40% 96.1%;
    --muted-foreground: 215.4 16.3% 46.9%;
    --accent: 210 40% 96.1%;
    --accent-foreground: 222.2 47.4% 11.2%;
    --destructive: 0 84.2% 60.2%;
    --destructive-foreground: 210 40% 98%;
    --border: 214.3 31.8% 91.4%;
    --input: 214.3 31.8% 91.4%;
    --ring: 222.2 84% 4.9%;
    --radius: 0.5rem;
  }
}

@layer base {
  * {
    @apply border-border;
  }
  body {
    @apply bg-background text-foreground;
  }
}
```

### TypeScript Configuration

**tsconfig.json** (already configured by Vite, but ensure these options):
```json
{
  "compilerOptions": {
    "target": "ES2020",
    "useDefineForClassFields": true,
    "lib": ["ES2020", "DOM", "DOM.Iterable"],
    "module": "ESNext",
    "skipLibCheck": true,
    "moduleResolution": "bundler",
    "allowImportingTsExtensions": true,
    "resolveJsonModule": true,
    "isolatedModules": true,
    "noEmit": true,
    "jsx": "react-jsx",
    "strict": true,
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "noFallthroughCasesInSwitch": true,
    "baseUrl": ".",
    "paths": {
      "@/*": ["./src/*"]
    }
  },
  "include": ["src"],
  "references": [{ "path": "./tsconfig.node.json" }]
}
```

### Vite Configuration

**vite.config.ts** - Configure asset paths for flexible deployments:
```typescript
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  base: './',  // Use relative paths for all assets
})
```

**Why `base: './'`?**

- **Absolute paths (default)**: Assets load from domain root (`/assets/...`). Breaks if deployed to subdirectories
- **Relative paths (`base: './'`)**: Assets use relative URLs from current file location. Works in any deployment location

This is especially useful when deploying to:
- Subdirectories (e.g., `example.com/myapp/`)
- Static file servers
- GitHub Pages or similar hosting
- Development/staging/production with different URL structures

---

## Project Structure

### Recommended Folder Structure

**Folder-per-Component Pattern** (Recommended):
```
src/
├── components/
│   ├── icons.tsx        # ⭐ All SVG icons in one file
│   ├── ui/              # Reusable UI components
│   │   ├── Button/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   ├── Card/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   ├── Input/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   ├── Modal/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   └── Badge/
│   │       ├── index.tsx
│   │       └── styles.css
│   ├── layout/          # Layout components
│   │   ├── Header/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   ├── Sidebar/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   ├── Footer/
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   └── MainLayout/
│   │       ├── index.tsx
│   │       └── styles.css
│   └── features/        # Feature-specific components
│       ├── auth/
│       │   ├── LoginForm/
│       │   │   ├── index.tsx
│       │   │   └── styles.css
│       │   └── RegisterForm/
│       │       ├── index.tsx
│       │       └── styles.css
│       ├── dashboard/
│       │   ├── Dashboard/
│       │   │   ├── index.tsx
│       │   │   └── styles.css
│       │   └── StatsCard/
│       │       ├── index.tsx
│       │       └── styles.css
│       └── users/
│           ├── UserTable/
│           │   ├── index.tsx
│           │   └── styles.css
│           └── UserProfile/
│               ├── index.tsx
│               └── styles.css
├── hooks/               # Custom React hooks
│   ├── useAuth.ts
│   ├── useModal.ts
│   ├── useForm.ts
│   ├── useFetch.ts
│   └── useLocalStorage.ts
├── contexts/            # React contexts
│   ├── AuthContext.tsx
│   └── ThemeContext.tsx
├── store/               # State management (Zustand/Redux)
│   └── useStore.ts
├── services/            # API services
│   ├── api.ts
│   └── auth.ts
├── types/               # TypeScript types/interfaces
│   ├── api.ts
│   ├── components.ts
│   └── models.ts
├── utils/               # Utility functions
│   ├── helpers.ts
│   └── validators.ts
├── styles/              # Global styles (not component-specific)
│   └── globals.css
├── pages/               # Page components (if using routing)
│   ├── Home/
│   │   ├── index.tsx
│   │   └── styles.css
│   ├── About/
│   │   ├── index.tsx
│   │   └── styles.css
│   ├── Dashboard/
│   │   ├── index.tsx
│   │   └── styles.css
│   ├── UserManagement/      # Page with page-specific components
│   │   ├── index.tsx
│   │   ├── styles.css
│   │   ├── EditUserModal/   # Component used only by UserManagement page
│   │   │   ├── index.tsx
│   │   │   └── styles.css
│   │   └── UserListTable/   # Component used only by UserManagement page
│   │       ├── index.tsx
│   │       └── styles.css
│   └── NotFound/
│       ├── index.tsx
│       └── styles.css
├── App.tsx
└── main.tsx
```

### File Naming Conventions

**Components and Pages:**
- Folder name: **PascalCase** - `Button/`, `UserProfile/`, `LoginForm/`
- Component file: **index.tsx** (always named `index.tsx`)
- Styles file: **styles.css** (always named `styles.css`)

**Hooks, Services, Utils:**
- **camelCase** for hooks: `useAuth.ts`, `useModal.ts`
- **camelCase** for utilities: `helpers.ts`, `validators.ts`

**Why this structure?**
- ✅ **Consistent naming** - All components use `index.tsx` and `styles.css`
- ✅ **Easy imports** - `import Button from '@/components/ui/Button'` (no need to specify file)
- ✅ **Colocation** - Component logic and styles stay together
- ✅ **Scalable** - Easy to add tests, types, or subcomponents to each folder
- ✅ **Clean** - Folder structure shows component hierarchy clearly

**Import Examples:**
```typescript
// Import components (index.tsx is implied)
import Button from '@/components/ui/Button';
import Header from '@/components/layout/Header';
import Dashboard from '@/pages/Dashboard';
import LoginForm from '@/components/features/auth/LoginForm';

// Import hooks, services, utils (explicit file names)
import { useAuth } from '@/hooks/useAuth';
import { api } from '@/services/api';
import { formatDate } from '@/utils/helpers';
```

### Folder Organization Guidelines

**components/ui/**
- Reusable, generic UI components
- No business logic
- Can be used across different features
- Examples: Button, Input, Card, Modal

**components/layout/**
- Layout and structural components
- Header, Footer, Sidebar, Navigation
- Page layout wrappers

**components/features/**
- Feature-specific components
- Organized by feature domain
- Can contain business logic
- Examples: auth/, dashboard/, users/

**hooks/**
- Custom React hooks
- Reusable stateful logic
- Prefix with "use"

**contexts/**
- React Context providers and consumers
- Global state management
- Theme, Auth, etc.

**store/**
- Global state management (Zustand, Redux, etc.)
- Actions, reducers, selectors

**services/**
- API calls and external services
- Business logic that doesn't belong in components
- Data fetching utilities

**types/**
- TypeScript type definitions
- Interfaces and types shared across the app
- API response types, model definitions

**utils/**
- Pure utility functions
- Helpers, formatters, validators
- No React-specific code

**pages/**
- Top-level route components (if using React Router)
- One file per route
- Compose smaller components
- **Page-Specific Components**: If a page has components that are ONLY used by that page, create a folder for each component within the page folder
  - Example: `pages/UserManagement/EditUserModal/` contains components used only by UserManagement
  - Each page-specific component follows the same folder-per-component pattern: `index.tsx` and `styles.css`
  - This keeps page-specific logic colocated with the page
  - If a component is used by multiple pages, move it to `components/features/` or `components/ui/`

**Example Page with Page-Specific Components:**
```
pages/UserManagement/
├── index.tsx              # Main page component
├── styles.css             # Page styles
├── EditUserModal/         # Component used ONLY by UserManagement
│   ├── index.tsx
│   └── styles.css
└── UserListTable/         # Component used ONLY by UserManagement
    ├── index.tsx
    └── styles.css
```

**Import Pattern:**
```typescript
// In pages/UserManagement/index.tsx
import EditUserModal from './EditUserModal';
import UserListTable from './UserListTable';
```

### Alternative Structures

**Feature-Based Structure** (for large apps):
```
src/
├── features/
│   ├── auth/
│   │   ├── components/
│   │   ├── hooks/
│   │   ├── services/
│   │   └── types/
│   ├── dashboard/
│   │   ├── components/
│   │   ├── hooks/
│   │   └── types/
│   └── users/
│       ├── components/
│       ├── hooks/
│       └── types/
├── shared/
│   ├── components/ui/
│   ├── hooks/
│   └── utils/
├── App.tsx
└── main.tsx
```

**Monorepo/Package Structure** (for very large apps):
```
packages/
├── ui/              # Shared UI component library
├── utils/           # Shared utilities
├── types/           # Shared TypeScript types
└── app/             # Main application
    └── src/
        ├── features/
        └── pages/
```

### Best Practices

1. **Keep it flat** - Don't nest folders too deeply (max 3-4 levels)
2. **Colocate related files** - Keep tests, styles near components
3. **One component per file** - Easier to find and maintain
4. **Index files** - Use `index.ts` to simplify imports
5. **Consistent naming** - Follow conventions throughout

### Component File Structure Examples

**Basic Component (UI components):**
```
Button/
├── index.tsx    # Component logic
└── styles.css   # Component styles
```

**src/components/ui/Button/index.tsx:**
```typescript
import React from 'react';
import './styles.css';

interface ButtonProps {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary';
  onClick?: () => void;
}

const Button: React.FC<ButtonProps> = ({
  children,
  variant = 'primary',
  onClick
}) => {
  return (
    <button className={`btn btn-${variant}`} onClick={onClick}>
      {children}
    </button>
  );
};

export default Button;
```

**src/components/ui/Button/styles.css:**
```css
.btn {
  padding: 0.5rem 1rem;
  border-radius: 0.375rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-primary {
  background-color: #2563eb;
  color: white;
}

.btn-primary:hover {
  background-color: #1d4ed8;
}

.btn-secondary {
  background-color: #64748b;
  color: white;
}

.btn-secondary:hover {
  background-color: #475569;
}
```

**Page Component:**
```
Home/
├── index.tsx
└── styles.css
```

**src/pages/Home/index.tsx:**
```typescript
import React from 'react';
import Button from '@/components/ui/Button';
import './styles.css';

const Home: React.FC = () => {
  return (
    <div className="home-container">
      <h1>Welcome Home</h1>
      <Button variant="primary" onClick={() => console.log('Clicked')}>
        Get Started
      </Button>
    </div>
  );
};

export default Home;
```

**Complex Component with Additional Files:**

When components need tests, TypeScript types, or subcomponents, extend the folder:

```
Dashboard/
├── index.tsx           # Main component
├── styles.css          # Component styles
├── index.test.tsx      # Component tests
├── types.ts           # Component-specific types
└── components/        # Subcomponents used only by Dashboard
    └── StatItem/
        ├── index.tsx
        └── styles.css
```

**Page with Page-Specific Components:**

When a page has components that are ONLY used by that page, organize them as subfolders:

```
pages/UserManagement/
├── index.tsx              # Main page component
├── styles.css             # Page styles
├── EditUserModal/         # Component used ONLY by this page
│   ├── index.tsx
│   └── styles.css
└── UserListTable/         # Component used ONLY by this page
    ├── index.tsx
    └── styles.css
```

**src/pages/UserManagement/index.tsx:**
```typescript
import React, { useState } from 'react';
import EditUserModal from './EditUserModal';
import UserListTable from './UserListTable';
import './styles.css';

const UserManagement: React.FC = () => {
  const [showModal, setShowModal] = useState(false);
  const [users, setUsers] = useState([]);

  return (
    <div className="user-management-container">
      <h1>User Management</h1>
      <UserListTable users={users} onEdit={() => setShowModal(true)} />
      {showModal && (
        <EditUserModal onClose={() => setShowModal(false)} />
      )}
    </div>
  );
};

export default UserManagement;
```

**IMPORTANT:**
- If a component is used by ONLY ONE page, keep it in the page folder
- If a component is used by MULTIPLE pages, move it to `components/features/` or `components/ui/`
- This keeps the codebase organized and makes it clear which components are shared vs. page-specific

---

## Icons Pattern

### Centralized Icons File

All SVG icons should be stored in a single **`src/components/icons.tsx`** file for consistency and reusability.

**src/components/icons.tsx:**
```typescript
import React from 'react';

export interface IconProps {
  size?: number;
  color?: string;
  className?: string;
}

export const CloseIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <line x1="18" y1="6" x2="6" y2="18" />
    <line x1="6" y1="6" x2="18" y2="18" />
  </svg>
);

export const MenuIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <line x1="3" y1="12" x2="21" y2="12" />
    <line x1="3" y1="6" x2="21" y2="6" />
    <line x1="3" y1="18" x2="21" y2="18" />
  </svg>
);

export const SearchIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <circle cx="11" cy="11" r="8" />
    <path d="m21 21-4.35-4.35" />
  </svg>
);

export const UserIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" />
    <circle cx="12" cy="7" r="4" />
  </svg>
);

export const HomeIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" />
    <polyline points="9 22 9 12 15 12 15 22" />
  </svg>
);

export const SettingsIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="M12.22 2h-.44a2 2 0 0 0-2 2v.18a2 2 0 0 1-1 1.73l-.43.25a2 2 0 0 1-2 0l-.15-.08a2 2 0 0 0-2.73.73l-.22.38a2 2 0 0 0 .73 2.73l.15.1a2 2 0 0 1 1 1.72v.51a2 2 0 0 1-1 1.74l-.15.09a2 2 0 0 0-.73 2.73l.22.38a2 2 0 0 0 2.73.73l.15-.08a2 2 0 0 1 2 0l.43.25a2 2 0 0 1 1 1.73V20a2 2 0 0 0 2 2h.44a2 2 0 0 0 2-2v-.18a2 2 0 0 1 1-1.73l.43-.25a2 2 0 0 1 2 0l.15.08a2 2 0 0 0 2.73-.73l.22-.39a2 2 0 0 0-.73-2.73l-.15-.08a2 2 0 0 1-1-1.74v-.5a2 2 0 0 1 1-1.74l.15-.09a2 2 0 0 0 .73-2.73l-.22-.38a2 2 0 0 0-2.73-.73l-.15.08a2 2 0 0 1-2 0l-.43-.25a2 2 0 0 1-1-1.73V4a2 2 0 0 0-2-2z" />
    <circle cx="12" cy="12" r="3" />
  </svg>
);

export const ChevronRightIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <polyline points="9 18 15 12 9 6" />
  </svg>
);

export const ChevronDownIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <polyline points="6 9 12 15 18 9" />
  </svg>
);

export const CheckIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <polyline points="20 6 9 17 4 12" />
  </svg>
);

export const AlertCircleIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <circle cx="12" cy="12" r="10" />
    <line x1="12" y1="8" x2="12" y2="12" />
    <line x1="12" y1="16" x2="12.01" y2="16" />
  </svg>
);

export const InfoIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <circle cx="12" cy="12" r="10" />
    <line x1="12" y1="16" x2="12" y2="12" />
    <line x1="12" y1="8" x2="12.01" y2="8" />
  </svg>
);

export const TrashIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <polyline points="3 6 5 6 21 6" />
    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" />
  </svg>
);

export const EditIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z" />
  </svg>
);

export const PlusIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <line x1="12" y1="5" x2="12" y2="19" />
    <line x1="5" y1="12" x2="19" y2="12" />
  </svg>
);

export const MinusIcon: React.FC<IconProps> = ({
  size = 24,
  color = 'currentColor',
  className = ''
}) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={size}
    height={size}
    viewBox="0 0 24 24"
    fill="none"
    stroke={color}
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <line x1="5" y1="12" x2="19" y2="12" />
  </svg>
);
```

### Using Icons in Components

**Example in Button component:**
```typescript
import React from 'react';
import { PlusIcon } from '@/components/icons';
import './styles.css';

interface ButtonProps {
  children: React.ReactNode;
  icon?: React.ReactNode;
  variant?: 'primary' | 'secondary';
}

const Button: React.FC<ButtonProps> = ({ children, icon, variant = 'primary' }) => {
  return (
    <button className={`btn btn-${variant}`}>
      {icon && <span className="btn-icon">{icon}</span>}
      {children}
    </button>
  );
};

export default Button;

// Usage:
// <Button icon={<PlusIcon size={20} />}>Add Item</Button>
```

**Example in Modal component:**
```typescript
import React from 'react';
import { CloseIcon } from '@/components/icons';
import './styles.css';

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

const Modal: React.FC<ModalProps> = ({ isOpen, onClose, title, children }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h2>{title}</h2>
          <button onClick={onClose} className="modal-close" aria-label="Close">
            <CloseIcon size={20} />
          </button>
        </div>
        <div className="modal-body">{children}</div>
      </div>
    </div>
  );
};

export default Modal;
```

**Example in Navigation:**
```typescript
import React from 'react';
import { HomeIcon, UserIcon, SettingsIcon } from '@/components/icons';
import './styles.css';

const Navigation: React.FC = () => {
  return (
    <nav className="nav">
      <a href="/" className="nav-link">
        <HomeIcon size={20} />
        <span>Home</span>
      </a>
      <a href="/profile" className="nav-link">
        <UserIcon size={20} />
        <span>Profile</span>
      </a>
      <a href="/settings" className="nav-link">
        <SettingsIcon size={20} />
        <span>Settings</span>
      </a>
    </nav>
  );
};

export default Navigation;
```

### Icon Best Practices

1. **Consistent Props**: All icons accept `size`, `color`, and `className`
2. **Default Values**: Set sensible defaults (size=24, color='currentColor')
3. **Accessibility**: Use `aria-label` on icon-only buttons
4. **Naming**: Use descriptive names ending with "Icon" (e.g., `CloseIcon`, `MenuIcon`)
5. **Single Source**: All icons in one file makes them easy to find and maintain
6. **Tree-shaking**: Named exports allow bundlers to tree-shake unused icons
7. **Flexibility**: `color='currentColor'` inherits text color from parent

### Adding New Icons

When adding new icons to `icons.tsx`:

1. Export as a named component (e.g., `export const NewIcon`)
2. Use the same `IconProps` interface
3. Include default values for size and color
4. Keep SVG viewBox at "0 0 24 24" for consistency
5. Use stroke-based icons when possible for consistency

---

## Component Templates

### Button Component

**src/components/ui/Button.tsx:**
```typescript
import React from 'react';

export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost' | 'destructive';
  size?: 'sm' | 'md' | 'lg';
  children: React.ReactNode;
}

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  size = 'md',
  className = '',
  children,
  ...props
}) => {
  const baseStyles = 'inline-flex items-center justify-center rounded-md font-medium transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50';

  const variants = {
    primary: 'bg-primary text-primary-foreground hover:bg-primary/90',
    secondary: 'bg-secondary text-secondary-foreground hover:bg-secondary/80',
    outline: 'border border-input bg-background hover:bg-accent hover:text-accent-foreground',
    ghost: 'hover:bg-accent hover:text-accent-foreground',
    destructive: 'bg-destructive text-destructive-foreground hover:bg-destructive/90',
  };

  const sizes = {
    sm: 'h-9 px-3 text-sm',
    md: 'h-10 px-4 py-2',
    lg: 'h-11 px-8',
  };

  return (
    <button
      className={`${baseStyles} ${variants[variant]} ${sizes[size]} ${className}`}
      {...props}
    >
      {children}
    </button>
  );
};
```

### Card Component

**src/components/ui/Card.tsx:**
```typescript
import React from 'react';

export interface CardProps {
  children: React.ReactNode;
  className?: string;
}

export const Card: React.FC<CardProps> = ({ children, className = '' }) => {
  return (
    <div className={`rounded-lg border bg-card text-card-foreground shadow-sm ${className}`}>
      {children}
    </div>
  );
};

export interface CardHeaderProps {
  children: React.ReactNode;
  className?: string;
}

export const CardHeader: React.FC<CardHeaderProps> = ({ children, className = '' }) => {
  return (
    <div className={`flex flex-col space-y-1.5 p-6 ${className}`}>
      {children}
    </div>
  );
};

export interface CardTitleProps {
  children: React.ReactNode;
  className?: string;
}

export const CardTitle: React.FC<CardTitleProps> = ({ children, className = '' }) => {
  return (
    <h3 className={`text-2xl font-semibold leading-none tracking-tight ${className}`}>
      {children}
    </h3>
  );
};

export interface CardDescriptionProps {
  children: React.ReactNode;
  className?: string;
}

export const CardDescription: React.FC<CardDescriptionProps> = ({ children, className = '' }) => {
  return (
    <p className={`text-sm text-muted-foreground ${className}`}>
      {children}
    </p>
  );
};

export interface CardContentProps {
  children: React.ReactNode;
  className?: string;
}

export const CardContent: React.FC<CardContentProps> = ({ children, className = '' }) => {
  return (
    <div className={`p-6 pt-0 ${className}`}>
      {children}
    </div>
  );
};

export interface CardFooterProps {
  children: React.ReactNode;
  className?: string;
}

export const CardFooter: React.FC<CardFooterProps> = ({ children, className = '' }) => {
  return (
    <div className={`flex items-center p-6 pt-0 ${className}`}>
      {children}
    </div>
  );
};
```

### Input Component

**src/components/ui/Input.tsx:**
```typescript
import React from 'react';

export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  helperText?: string;
}

export const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className = '', label, error, helperText, id, ...props }, ref) => {
    const inputId = id || label?.toLowerCase().replace(/\s+/g, '-');

    return (
      <div className="w-full">
        {label && (
          <label
            htmlFor={inputId}
            className="mb-2 block text-sm font-medium text-foreground"
          >
            {label}
          </label>
        )}
        <input
          id={inputId}
          ref={ref}
          className={`flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 ${
            error ? 'border-destructive' : ''
          } ${className}`}
          {...props}
        />
        {error && (
          <p className="mt-1 text-sm text-destructive">{error}</p>
        )}
        {helperText && !error && (
          <p className="mt-1 text-sm text-muted-foreground">{helperText}</p>
        )}
      </div>
    );
  }
);

Input.displayName = 'Input';
```

### Modal Component

**src/components/ui/Modal.tsx:**
```typescript
import React, { useEffect } from 'react';

export interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  description?: string;
  children: React.ReactNode;
  footer?: React.ReactNode;
}

export const Modal: React.FC<ModalProps> = ({
  isOpen,
  onClose,
  title,
  description,
  children,
  footer,
}) => {
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
      {/* Backdrop */}
      <div
        className="fixed inset-0 bg-black/50 transition-opacity"
        onClick={onClose}
        aria-hidden="true"
      />

      {/* Modal */}
      <div className="relative z-50 w-full max-w-lg rounded-lg bg-background p-6 shadow-lg">
        {/* Header */}
        {(title || description) && (
          <div className="mb-4">
            {title && (
              <h2 className="text-lg font-semibold text-foreground">{title}</h2>
            )}
            {description && (
              <p className="mt-1 text-sm text-muted-foreground">{description}</p>
            )}
          </div>
        )}

        {/* Close button */}
        <button
          onClick={onClose}
          className="absolute right-4 top-4 rounded-sm opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
          aria-label="Close"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          >
            <line x1="18" y1="6" x2="6" y2="18"></line>
            <line x1="6" y1="6" x2="18" y2="18"></line>
          </svg>
        </button>

        {/* Content */}
        <div className="mb-4">{children}</div>

        {/* Footer */}
        {footer && (
          <div className="flex justify-end space-x-2">{footer}</div>
        )}
      </div>
    </div>
  );
};
```

### Badge Component

**src/components/ui/Badge.tsx:**
```typescript
import React from 'react';

export interface BadgeProps {
  variant?: 'default' | 'secondary' | 'destructive' | 'outline' | 'success';
  children: React.ReactNode;
  className?: string;
}

export const Badge: React.FC<BadgeProps> = ({
  variant = 'default',
  children,
  className = '',
}) => {
  const variants = {
    default: 'bg-primary text-primary-foreground hover:bg-primary/80',
    secondary: 'bg-secondary text-secondary-foreground hover:bg-secondary/80',
    destructive: 'bg-destructive text-destructive-foreground hover:bg-destructive/80',
    outline: 'border border-input bg-background hover:bg-accent hover:text-accent-foreground',
    success: 'bg-green-100 text-green-800 hover:bg-green-100/80',
  };

  return (
    <span
      className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 ${variants[variant]} ${className}`}
    >
      {children}
    </span>
  );
};
```

---

## Hook Patterns

### useModal Hook

**src/hooks/useModal.ts:**
```typescript
import { useState, useCallback } from 'react';

export interface UseModalReturn {
  isOpen: boolean;
  open: () => void;
  close: () => void;
  toggle: () => void;
}

export const useModal = (initialState = false): UseModalReturn => {
  const [isOpen, setIsOpen] = useState(initialState);

  const open = useCallback(() => setIsOpen(true), []);
  const close = useCallback(() => setIsOpen(false), []);
  const toggle = useCallback(() => setIsOpen(prev => !prev), []);

  return { isOpen, open, close, toggle };
};
```

### useForm Hook

**src/hooks/useForm.ts:**
```typescript
import { useState, useCallback, ChangeEvent } from 'react';

export interface UseFormReturn<T> {
  values: T;
  errors: Partial<Record<keyof T, string>>;
  handleChange: (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => void;
  setFieldValue: (field: keyof T, value: any) => void;
  setErrors: (errors: Partial<Record<keyof T, string>>) => void;
  setValues: (values: T) => void;
  reset: () => void;
  resetField: (field: keyof T) => void;
}

export const useForm = <T extends Record<string, any>>(
  initialValues: T
): UseFormReturn<T> => {
  const [values, setValues] = useState<T>(initialValues);
  const [errors, setErrors] = useState<Partial<Record<keyof T, string>>>({});

  const handleChange = useCallback((
    e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.target;
    const fieldValue = type === 'checkbox' ? (e.target as HTMLInputElement).checked : value;

    setValues(prev => ({
      ...prev,
      [name]: fieldValue,
    }));

    // Clear error for this field when user starts typing
    if (errors[name as keyof T]) {
      setErrors(prev => {
        const newErrors = { ...prev };
        delete newErrors[name as keyof T];
        return newErrors;
      });
    }
  }, [errors]);

  const setFieldValue = useCallback((field: keyof T, value: any) => {
    setValues(prev => ({
      ...prev,
      [field]: value,
    }));
  }, []);

  const reset = useCallback(() => {
    setValues(initialValues);
    setErrors({});
  }, [initialValues]);

  const resetField = useCallback((field: keyof T) => {
    setValues(prev => ({
      ...prev,
      [field]: initialValues[field],
    }));
    setErrors(prev => {
      const newErrors = { ...prev };
      delete newErrors[field];
      return newErrors;
    });
  }, [initialValues]);

  return {
    values,
    errors,
    handleChange,
    setFieldValue,
    setErrors,
    setValues,
    reset,
    resetField,
  };
};
```

### useLocalStorage Hook

**src/hooks/useLocalStorage.ts:**
```typescript
import { useState, useEffect } from 'react';

export function useLocalStorage<T>(key: string, initialValue: T) {
  const [storedValue, setStoredValue] = useState<T>(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      console.error(error);
      return initialValue;
    }
  });

  const setValue = (value: T | ((val: T) => T)) => {
    try {
      const valueToStore = value instanceof Function ? value(storedValue) : value;
      setStoredValue(valueToStore);
      window.localStorage.setItem(key, JSON.stringify(valueToStore));
    } catch (error) {
      console.error(error);
    }
  };

  return [storedValue, setValue] as const;
}
```

### useDebounce Hook

**src/hooks/useDebounce.ts:**
```typescript
import { useState, useEffect } from 'react';

export function useDebounce<T>(value: T, delay: number = 500): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(handler);
    };
  }, [value, delay]);

  return debouncedValue;
}
```

### useFetch Hook

**src/hooks/useFetch.ts:**
```typescript
import { useState, useEffect } from 'react';

export interface UseFetchResult<T> {
  data: T | null;
  loading: boolean;
  error: Error | null;
  refetch: () => void;
}

export function useFetch<T>(url: string): UseFetchResult<T> {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const fetchData = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await fetch(url);

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

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
}
```

---

## User Notifications

### React-Toastify (Default)

**IMPORTANT:** This project uses **react-toastify** as the default library for displaying toast notifications to users. Always use react-toastify for success messages, error alerts, warnings, and informational messages.

### Installation

```bash
npm install react-toastify
```

### Setup

**src/main.tsx or src/App.tsx:**
```typescript
import React from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function App() {
  return (
    <>
      {/* Your app components */}
      <ToastContainer
        position="top-right"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
      />
    </>
  );
}

export default App;
```

### Basic Usage

**Success Message:**
```typescript
import { toast } from 'react-toastify';

const handleSuccess = () => {
  toast.success('Operation completed successfully!');
};
```

**Error Message:**
```typescript
import { toast } from 'react-toastify';

const handleError = () => {
  toast.error('Something went wrong. Please try again.');
};
```

**Warning Message:**
```typescript
import { toast } from 'react-toastify';

const handleWarning = () => {
  toast.warning('Please review your input before submitting.');
};
```

**Info Message:**
```typescript
import { toast } from 'react-toastify';

const handleInfo = () => {
  toast.info('New updates are available.');
};
```

### Advanced Usage

**Custom Duration:**
```typescript
toast.success('Saved!', {
  autoClose: 5000, // 5 seconds
});
```

**Custom Position:**
```typescript
toast.success('Message sent!', {
  position: 'bottom-center',
});
```

**With Loading State:**
```typescript
import { toast } from 'react-toastify';

const handleAsyncOperation = async () => {
  const toastId = toast.loading('Processing...');

  try {
    await someAsyncOperation();
    toast.update(toastId, {
      render: 'Success!',
      type: 'success',
      isLoading: false,
      autoClose: 3000,
    });
  } catch (error) {
    toast.update(toastId, {
      render: 'Failed to process',
      type: 'error',
      isLoading: false,
      autoClose: 3000,
    });
  }
};
```

**Promise-based Toast:**
```typescript
import { toast } from 'react-toastify';

const handlePromise = () => {
  const myPromise = fetch('/api/data').then(res => res.json());

  toast.promise(
    myPromise,
    {
      pending: 'Loading data...',
      success: 'Data loaded successfully!',
      error: 'Failed to load data',
    }
  );
};
```

### Custom Styling

**Custom Class Names:**
```typescript
toast.success('Custom styled toast!', {
  className: 'custom-toast',
  bodyClassName: 'custom-toast-body',
  progressClassName: 'custom-progress-bar',
});
```

**Dark Theme:**
```typescript
toast.success('Dark mode toast!', {
  theme: 'dark',
});
```

### Practical Examples

**Form Submission:**
```typescript
import React, { useState } from 'react';
import { toast } from 'react-toastify';
import { Button } from './ui/Button';
import { Input } from './ui/Input';

export const CreateUserForm: React.FC = () => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!name || !email) {
      toast.warning('Please fill in all fields');
      return;
    }

    setLoading(true);
    const toastId = toast.loading('Creating user...');

    try {
      const response = await fetch('/api/users', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, email }),
      });

      if (!response.ok) throw new Error('Failed to create user');

      toast.update(toastId, {
        render: 'User created successfully!',
        type: 'success',
        isLoading: false,
        autoClose: 3000,
      });

      setName('');
      setEmail('');
    } catch (error) {
      toast.update(toastId, {
        render: 'Failed to create user. Please try again.',
        type: 'error',
        isLoading: false,
        autoClose: 3000,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <Input
        label="Name"
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Enter name"
      />
      <Input
        label="Email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Enter email"
      />
      <Button type="submit" disabled={loading}>
        {loading ? 'Creating...' : 'Create User'}
      </Button>
    </form>
  );
};
```

**Delete Confirmation:**
```typescript
import React from 'react';
import { toast } from 'react-toastify';
import { Button } from './ui/Button';

export const DeleteButton: React.FC<{ userId: number }> = ({ userId }) => {
  const handleDelete = async () => {
    const toastId = toast.loading('Deleting user...');

    try {
      const response = await fetch(`/api/users/${userId}`, {
        method: 'DELETE',
      });

      if (!response.ok) throw new Error('Delete failed');

      toast.update(toastId, {
        render: 'User deleted successfully',
        type: 'success',
        isLoading: false,
        autoClose: 3000,
      });
    } catch (error) {
      toast.update(toastId, {
        render: 'Failed to delete user',
        type: 'error',
        isLoading: false,
        autoClose: 3000,
      });
    }
  };

  return (
    <Button variant="destructive" onClick={handleDelete}>
      Delete
    </Button>
  );
};
```

**Copy to Clipboard:**
```typescript
import React from 'react';
import { toast } from 'react-toastify';
import { Button } from './ui/Button';

export const CopyButton: React.FC<{ text: string }> = ({ text }) => {
  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(text);
      toast.success('Copied to clipboard!', {
        autoClose: 2000,
      });
    } catch (error) {
      toast.error('Failed to copy');
    }
  };

  return (
    <Button variant="outline" onClick={handleCopy}>
      Copy
    </Button>
  );
};
```

### useToast Custom Hook

**src/hooks/useToast.ts:**
```typescript
import { toast, ToastOptions } from 'react-toastify';

export const useToast = () => {
  const showSuccess = (message: string, options?: ToastOptions) => {
    toast.success(message, options);
  };

  const showError = (message: string, options?: ToastOptions) => {
    toast.error(message, options);
  };

  const showWarning = (message: string, options?: ToastOptions) => {
    toast.warning(message, options);
  };

  const showInfo = (message: string, options?: ToastOptions) => {
    toast.info(message, options);
  };

  const showLoading = (message: string, options?: ToastOptions) => {
    return toast.loading(message, options);
  };

  const updateToast = (
    toastId: string | number,
    message: string,
    type: 'success' | 'error' | 'warning' | 'info',
    options?: ToastOptions
  ) => {
    toast.update(toastId, {
      render: message,
      type,
      isLoading: false,
      autoClose: 3000,
      ...options,
    });
  };

  return {
    showSuccess,
    showError,
    showWarning,
    showInfo,
    showLoading,
    updateToast,
  };
};
```

**Usage:**
```typescript
import React from 'react';
import { useToast } from '@/hooks/useToast';
import { Button } from './ui/Button';

export const ExampleComponent: React.FC = () => {
  const { showSuccess, showError } = useToast();

  const handleClick = async () => {
    try {
      await someOperation();
      showSuccess('Operation completed!');
    } catch (error) {
      showError('Operation failed!');
    }
  };

  return <Button onClick={handleClick}>Click Me</Button>;
};
```

### ToastContainer Configuration Options

**Common configurations:**

```typescript
<ToastContainer
  position="top-right"           // top-left, top-right, top-center, bottom-left, bottom-right, bottom-center
  autoClose={3000}               // Duration in milliseconds (false to disable)
  hideProgressBar={false}        // Show/hide progress bar
  newestOnTop={false}           // Stack newest toasts on top
  closeOnClick                   // Close toast on click
  rtl={false}                    // Right-to-left support
  pauseOnFocusLoss              // Pause on window blur
  draggable                      // Enable drag to dismiss
  pauseOnHover                   // Pause autoClose on hover
  theme="light"                  // light, dark, colored
  limit={3}                      // Max number of toasts to show
/>
```

### Best Practices

1. **Always use react-toastify** for user notifications (don't use alert(), console.log(), or custom notification systems)
2. **Be specific with messages** - Users should understand what happened
3. **Use appropriate toast types** - success for confirmations, error for failures, warning for caution, info for general messages
4. **Keep messages concise** - One or two short sentences maximum
5. **Use loading states** for async operations that take more than 1 second
6. **Don't spam toasts** - Consider using the `limit` option to prevent overwhelming users
7. **Match toast theme** to your application theme (light/dark)

---

## Styling Solutions

### Tailwind CSS (Recommended)

Best for: Fast development, consistency, prototype conversions

**Usage:**
```typescript
export const Example: React.FC = () => {
  return (
    <div className="rounded-lg border border-slate-200 bg-white p-6 shadow-sm">
      <h2 className="text-xl font-semibold text-slate-900">Title</h2>
      <p className="mt-2 text-sm text-slate-600">Description</p>
      <button className="mt-4 rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700">
        Click me
      </button>
    </div>
  );
};
```

### CSS Modules

Best for: Component isolation, traditional CSS approach

**Component.module.css:**
```css
.card {
  border-radius: 0.5rem;
  border: 1px solid #e2e8f0;
  background-color: white;
  padding: 1.5rem;
  box-shadow: 0 1px 2px 0 rgb(0 0 0 / 0.05);
}

.title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #0f172a;
}

.description {
  margin-top: 0.5rem;
  font-size: 0.875rem;
  color: #64748b;
}

.button {
  margin-top: 1rem;
  border-radius: 0.375rem;
  background-color: #2563eb;
  padding: 0.5rem 1rem;
  color: white;
}

.button:hover {
  background-color: #1d4ed8;
}
```

**Component.tsx:**
```typescript
import styles from './Component.module.css';

export const Example: React.FC = () => {
  return (
    <div className={styles.card}>
      <h2 className={styles.title}>Title</h2>
      <p className={styles.description}>Description</p>
      <button className={styles.button}>Click me</button>
    </div>
  );
};
```

### styled-components

Best for: Dynamic styling, theming, CSS-in-JS preference

**Installation:**
```bash
npm install styled-components
npm install -D @types/styled-components
```

**Component.tsx:**
```typescript
import styled from 'styled-components';

const Card = styled.div`
  border-radius: 0.5rem;
  border: 1px solid #e2e8f0;
  background-color: white;
  padding: 1.5rem;
  box-shadow: 0 1px 2px 0 rgb(0 0 0 / 0.05);
`;

const Title = styled.h2`
  font-size: 1.25rem;
  font-weight: 600;
  color: #0f172a;
`;

const Description = styled.p`
  margin-top: 0.5rem;
  font-size: 0.875rem;
  color: #64748b;
`;

const Button = styled.button`
  margin-top: 1rem;
  border-radius: 0.375rem;
  background-color: #2563eb;
  padding: 0.5rem 1rem;
  color: white;
  border: none;
  cursor: pointer;

  &:hover {
    background-color: #1d4ed8;
  }
`;

export const Example: React.FC = () => {
  return (
    <Card>
      <Title>Title</Title>
      <Description>Description</Description>
      <Button>Click me</Button>
    </Card>
  );
};
```

---

## Common Conversions

### Dashboard Layout

**HTML Prototype:**
```html
<div class="min-h-screen bg-gray-50">
  <aside class="fixed left-0 top-0 h-screen w-64 bg-white border-r">
    <!-- Sidebar content -->
  </aside>
  <div class="ml-64">
    <header class="border-b bg-white px-6 py-4">
      <!-- Header content -->
    </header>
    <main class="p-6">
      <!-- Main content -->
    </main>
  </div>
</div>
```

**React Conversion:**

**src/components/layout/DashboardLayout.tsx:**
```typescript
import React from 'react';
import { Sidebar } from './Sidebar';
import { Header } from './Header';

export interface DashboardLayoutProps {
  children: React.ReactNode;
}

export const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen bg-gray-50">
      <Sidebar />
      <div className="ml-64">
        <Header />
        <main className="p-6">{children}</main>
      </div>
    </div>
  );
};
```

**src/components/layout/Sidebar.tsx:**
```typescript
import React from 'react';
import { Link, useLocation } from 'react-router-dom';

interface NavItem {
  name: string;
  path: string;
  icon: React.ReactNode;
}

const navItems: NavItem[] = [
  { name: 'Dashboard', path: '/', icon: <span>📊</span> },
  { name: 'Users', path: '/users', icon: <span>👥</span> },
  { name: 'Settings', path: '/settings', icon: <span>⚙️</span> },
];

export const Sidebar: React.FC = () => {
  const location = useLocation();

  return (
    <aside className="fixed left-0 top-0 h-screen w-64 border-r bg-white">
      <div className="p-6">
        <h1 className="text-xl font-bold">My App</h1>
      </div>
      <nav className="space-y-1 px-3">
        {navItems.map((item) => {
          const isActive = location.pathname === item.path;
          return (
            <Link
              key={item.path}
              to={item.path}
              className={`flex items-center space-x-3 rounded-md px-3 py-2 text-sm font-medium ${
                isActive
                  ? 'bg-gray-100 text-gray-900'
                  : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900'
              }`}
            >
              {item.icon}
              <span>{item.name}</span>
            </Link>
          );
        })}
      </nav>
    </aside>
  );
};
```

### Form Conversion

**HTML Prototype:**
```html
<form id="loginForm">
  <div>
    <label for="email">Email</label>
    <input type="email" id="email" required>
    <span id="email-error" class="hidden"></span>
  </div>
  <div>
    <label for="password">Password</label>
    <input type="password" id="password" required>
    <span id="password-error" class="hidden"></span>
  </div>
  <button type="submit">Login</button>
</form>
```

**React Conversion:**

**src/components/LoginForm.tsx:**
```typescript
import React, { useState } from 'react';
import { Input } from './ui/Input';
import { Button } from './ui/Button';
import { useForm } from '../hooks/useForm';

interface LoginFormData {
  email: string;
  password: string;
}

export const LoginForm: React.FC = () => {
  const { values, errors, handleChange, setErrors } = useForm<LoginFormData>({
    email: '',
    password: '',
  });
  const [loading, setLoading] = useState(false);

  const validate = (): boolean => {
    const newErrors: Partial<Record<keyof LoginFormData, string>> = {};

    if (!values.email) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(values.email)) {
      newErrors.email = 'Email is invalid';
    }

    if (!values.password) {
      newErrors.password = 'Password is required';
    } else if (values.password.length < 8) {
      newErrors.password = 'Password must be at least 8 characters';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validate()) return;

    setLoading(true);
    try {
      // API call here
      await new Promise(resolve => setTimeout(resolve, 1000));
      console.log('Login successful', values);
    } catch (error) {
      console.error('Login failed', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <Input
        label="Email"
        type="email"
        name="email"
        value={values.email}
        onChange={handleChange}
        error={errors.email}
        placeholder="Enter your email"
        required
      />

      <Input
        label="Password"
        type="password"
        name="password"
        value={values.password}
        onChange={handleChange}
        error={errors.password}
        placeholder="Enter your password"
        required
      />

      <Button type="submit" disabled={loading} className="w-full">
        {loading ? 'Loading...' : 'Login'}
      </Button>
    </form>
  );
};
```

### Table Conversion

**HTML Prototype:**
```html
<table>
  <thead>
    <tr>
      <th onclick="sort('name')">Name</th>
      <th onclick="sort('email')">Email</th>
      <th>Actions</th>
    </tr>
  </thead>
  <tbody id="tableBody">
    <!-- Rows populated by JavaScript -->
  </tbody>
</table>
```

**React Conversion:**

**src/components/UserTable.tsx:**
```typescript
import React, { useState, useMemo } from 'react';
import { Button } from './ui/Button';

export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

export interface UserTableProps {
  users: User[];
  onEdit: (user: User) => void;
  onDelete: (userId: number) => void;
}

type SortField = keyof User;
type SortDirection = 'asc' | 'desc';

export const UserTable: React.FC<UserTableProps> = ({
  users,
  onEdit,
  onDelete,
}) => {
  const [sortField, setSortField] = useState<SortField>('name');
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');

  const sortedUsers = useMemo(() => {
    return [...users].sort((a, b) => {
      const aValue = a[sortField];
      const bValue = b[sortField];
      const modifier = sortDirection === 'asc' ? 1 : -1;

      if (aValue < bValue) return -1 * modifier;
      if (aValue > bValue) return 1 * modifier;
      return 0;
    });
  }, [users, sortField, sortDirection]);

  const handleSort = (field: SortField) => {
    if (field === sortField) {
      setSortDirection(prev => (prev === 'asc' ? 'desc' : 'asc'));
    } else {
      setSortField(field);
      setSortDirection('asc');
    }
  };

  const getSortIcon = (field: SortField) => {
    if (field !== sortField) return '↕️';
    return sortDirection === 'asc' ? '↑' : '↓';
  };

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th
              onClick={() => handleSort('name')}
              className="cursor-pointer px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500 hover:bg-gray-100"
            >
              Name {getSortIcon('name')}
            </th>
            <th
              onClick={() => handleSort('email')}
              className="cursor-pointer px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500 hover:bg-gray-100"
            >
              Email {getSortIcon('email')}
            </th>
            <th
              onClick={() => handleSort('role')}
              className="cursor-pointer px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500 hover:bg-gray-100"
            >
              Role {getSortIcon('role')}
            </th>
            <th className="px-6 py-3 text-right text-xs font-medium uppercase tracking-wider text-gray-500">
              Actions
            </th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200 bg-white">
          {sortedUsers.map((user) => (
            <tr key={user.id} className="hover:bg-gray-50">
              <td className="whitespace-nowrap px-6 py-4 text-sm font-medium text-gray-900">
                {user.name}
              </td>
              <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500">
                {user.email}
              </td>
              <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500">
                {user.role}
              </td>
              <td className="whitespace-nowrap px-6 py-4 text-right text-sm font-medium">
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() => onEdit(user)}
                  className="mr-2"
                >
                  Edit
                </Button>
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() => onDelete(user.id)}
                  className="text-red-600 hover:text-red-800"
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
```

---

## Advanced Patterns

### Context for Theme

**src/contexts/ThemeContext.tsx:**
```typescript
import React, { createContext, useContext, useState, useEffect } from 'react';

type Theme = 'light' | 'dark';

interface ThemeContextType {
  theme: Theme;
  toggleTheme: () => void;
  setTheme: (theme: Theme) => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [theme, setTheme] = useState<Theme>(() => {
    const stored = localStorage.getItem('theme');
    return (stored as Theme) || 'light';
  });

  useEffect(() => {
    localStorage.setItem('theme', theme);
    document.documentElement.classList.toggle('dark', theme === 'dark');
  }, [theme]);

  const toggleTheme = () => {
    setTheme(prev => (prev === 'light' ? 'dark' : 'light'));
  };

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error('useTheme must be used within ThemeProvider');
  }
  return context;
};
```

### Error Boundary

**src/components/ErrorBoundary.tsx:**
```typescript
import React, { Component, ReactNode } from 'react';

interface Props {
  children: ReactNode;
  fallback?: ReactNode;
}

interface State {
  hasError: boolean;
  error: Error | null;
}

export class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error('Error caught by boundary:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        this.props.fallback || (
          <div className="flex min-h-screen items-center justify-center">
            <div className="text-center">
              <h1 className="text-2xl font-bold text-red-600">Something went wrong</h1>
              <p className="mt-2 text-gray-600">{this.state.error?.message}</p>
              <button
                onClick={() => this.setState({ hasError: false, error: null })}
                className="mt-4 rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700"
              >
                Try again
              </button>
            </div>
          </div>
        )
      );
    }

    return this.props.children;
  }
}
```

### Protected Route

**src/components/ProtectedRoute.tsx:**
```typescript
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

interface ProtectedRouteProps {
  isAuthenticated: boolean;
  redirectPath?: string;
  children?: React.ReactNode;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  isAuthenticated,
  redirectPath = '/login',
  children,
}) => {
  if (!isAuthenticated) {
    return <Navigate to={redirectPath} replace />;
  }

  return children ? <>{children}</> : <Outlet />;
};
```

### Infinite Scroll

**src/hooks/useInfiniteScroll.ts:**
```typescript
import { useEffect, useRef, useCallback } from 'react';

export function useInfiniteScroll(
  onLoadMore: () => void,
  hasMore: boolean,
  loading: boolean
) {
  const observer = useRef<IntersectionObserver | null>(null);

  const lastElementRef = useCallback(
    (node: HTMLElement | null) => {
      if (loading) return;
      if (observer.current) observer.current.disconnect();

      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && hasMore) {
          onLoadMore();
        }
      });

      if (node) observer.current.observe(node);
    },
    [loading, hasMore, onLoadMore]
  );

  return lastElementRef;
}
```

---

## Complete Example: Dashboard App

**src/App.tsx:**
```typescript
import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from './contexts/ThemeContext';
import { ErrorBoundary } from './components/ErrorBoundary';
import { DashboardLayout } from './components/layout/DashboardLayout';
import { Dashboard } from './pages/Dashboard';
import { Users } from './pages/Users';
import { Settings } from './pages/Settings';
import { Login } from './pages/Login';
import { ProtectedRoute } from './components/ProtectedRoute';

function App() {
  const [isAuthenticated, setIsAuthenticated] = React.useState(false);

  return (
    <ErrorBoundary>
      <ThemeProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<Login onLogin={() => setIsAuthenticated(true)} />} />

            <Route element={<ProtectedRoute isAuthenticated={isAuthenticated} />}>
              <Route element={<DashboardLayout />}>
                <Route path="/" element={<Dashboard />} />
                <Route path="/users" element={<Users />} />
                <Route path="/settings" element={<Settings />} />
              </Route>
            </Route>
          </Routes>
        </BrowserRouter>
      </ThemeProvider>
    </ErrorBoundary>
  );
}

export default App;
```

This reference guide provides all the essential patterns for converting HTML prototypes to production-ready React applications.
