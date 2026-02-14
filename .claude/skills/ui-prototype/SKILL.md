---
name: ui-prototype
description: Create rapid UI prototypes using HTML, CSS, and Tailwind CSS via CDN. Specializes in generating modern, accessible, and interactive user interfaces without build tools or frameworks. Just open HTML files in a browser.
allowed-tools: Read, Grep, Glob, Bash, Write, Edit
---

# UI Prototype Skill

## Purpose

Generate quick, beautiful UI prototypes using simple web technologies:
- Pure HTML5 with semantic markup
- Tailwind CSS via CDN (no build process)
- Vanilla JavaScript for interactivity
- shadcn-inspired components and styling
- Responsive layouts with mobile-first approach
- Accessible components following WCAG guidelines
- Zero setup - just open HTML files in browser

## CRITICAL CONSTRAINTS

**FORBIDDEN ACTIONS:**
1. **NEVER modify production front-end projects** (Web/, client/, src/, app/, etc.)
2. **NEVER check or explore production web projects** unless explicitly requested by the user
3. **ONLY work on standalone prototype HTML files** (typically in prototype/ or similar folders)
4. **This skill creates NEW standalone prototypes** - it does not integrate with existing applications

**REQUIRED BEST PRACTICES:**
1. **ALWAYS centralize shared styles in styles.css** when multiple HTML pages exist
2. **NEVER duplicate theme variables or common styles** across multiple HTML files
3. **Link to styles.css** from all HTML files that need shared styles
4. **Keep only page-specific styles inline** in each HTML file

**Scope:**
- Work exclusively with standalone HTML files for rapid prototyping
- Create prototypes in a dedicated prototype folder (e.g., prototype/, prototypes/, html-prototypes/)
- If unsure where to place files, ask the user first
- Do not attempt to integrate prototypes into existing build systems or frameworks

## When to Use

Invoke this skill when you need to:
- "Create a UI prototype for [feature]"
- "Build a dashboard mockup"
- "Design a form for [purpose]"
- "Prototype a landing page"
- "Create a settings page UI"
- "Build a data table interface"
- "Generate a modal/dialog design"
- "Create a navigation layout"
- "Mock up an admin panel"

## Analysis Steps

1. **UNDERSTAND REQUIREMENTS**
   - Clarify the UI purpose and target users
   - Identify key features and interactions needed
   - Determine if prototype is static or needs interactivity
   - Ask about design preferences (layout, color scheme, components)

2. **CHECK PROJECT CONTEXT**
   - Look for existing HTML prototypes or templates
   - **Check for existing styles.css file** - use it if present
   - **Identify if multiple pages will share styles** - create styles.css if needed
   - Check if there's a preferred color scheme or branding
   - Identify if this is standalone or part of a larger project
   - Review any existing style patterns to maintain consistency

3. **PLAN LAYOUT STRUCTURE**
   - Break down UI into logical sections (header, sidebar, main, footer)
   - Identify component types needed (cards, forms, tables, modals)
   - Determine responsive breakpoints
   - Plan navigation and user flow
   - Decide on color palette (default to neutral slate/zinc)

4. **GENERATE HTML PROTOTYPE**
   - Create styles.css first if multiple pages will share styles
   - Create standalone HTML file(s) that link to styles.css
   - Include Tailwind CSS via CDN
   - Use semantic HTML5 elements
   - Add shadcn-inspired component classes
   - Implement responsive grid layouts
   - Include sample/placeholder data
   - Keep page-specific styles inline, shared styles in styles.css

5. **ADD INTERACTIVITY (if needed)**
   - Add vanilla JavaScript for interactions
   - Implement form validation
   - Create modal/dialog functionality
   - Add dropdown menus and tabs
   - Handle mobile menu toggles
   - Keep JavaScript simple and inline or in same file

6. **ENSURE QUALITY**
   - Use semantic HTML for accessibility
   - Add ARIA labels where needed
   - Ensure keyboard navigation works
   - Test mobile responsiveness
   - Verify color contrast
   - Add helpful HTML comments

## HTML Template Structure

Every prototype should start with this base:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Prototype - [Feature Name]</title>

    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>

    <!-- Optional: Custom Tailwind Config -->
    <script>
        tailwind.config = {
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
                    }
                }
            }
        }
    </script>

    <style>
        /* Custom shadcn-inspired component styles */
        .btn {
            @apply inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50;
        }
        .btn-primary {
            @apply bg-primary text-primary-foreground hover:bg-primary/90 px-4 py-2;
        }
        .btn-outline {
            @apply border border-input bg-background hover:bg-secondary hover:text-secondary-foreground px-4 py-2;
        }
        .card {
            @apply rounded-lg border bg-white shadow-sm;
        }
        .input {
            @apply flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50;
        }
    </style>
</head>
<body class="bg-gray-50">
    <!-- Your UI prototype goes here -->

    <script>
        // Vanilla JavaScript for interactivity
    </script>
</body>
</html>
```

## Shared Stylesheet Template (styles.css)

When multiple HTML pages share styles, create a `styles.css` file with this structure:

```css
/* ============================================
   SHARED STYLES FOR PROTOTYPE
   ============================================ */

/* Theme Variables - Light Mode */
:root[data-theme="light"] {
    --bg-primary: #ffffff;
    --bg-secondary: #f8f9fa;
    --bg-tertiary: #e9ecef;
    --bg-card: #ffffff;
    --bg-hover: #f1f3f5;
    --border-primary: #dee2e6;
    --border-secondary: #ced4da;
    --text-primary: #212529;
    --text-secondary: #495057;
    --text-muted: #6c757d;
    --accent-primary: #646cff;
    --accent-hover: #535bf2;
    --success: #10b981;
    --danger: #dc3545;
}

/* Theme Variables - Dark Mode */
:root[data-theme="dark"] {
    --bg-primary: #1a1a1a;
    --bg-secondary: #2a2a2a;
    --bg-tertiary: #3a3a3a;
    --bg-card: #2a2a2a;
    --bg-hover: #404040;
    --border-primary: #404040;
    --border-secondary: #555555;
    --text-primary: rgba(255, 255, 255, 0.87);
    --text-secondary: rgba(255, 255, 255, 0.70);
    --text-muted: rgba(255, 255, 255, 0.50);
    --accent-primary: #646cff;
    --accent-hover: #535bf2;
    --success: #10b981;
    --danger: #dc3545;
}

/* Base Styles */
body {
    background-color: var(--bg-primary);
    color: var(--text-primary);
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    transition: background-color 0.3s ease, color 0.3s ease;
}

/* Custom Scrollbar */
.custom-scrollbar::-webkit-scrollbar {
    width: 8px;
}

.custom-scrollbar::-webkit-scrollbar-track {
    background: var(--bg-secondary);
    border-radius: 4px;
}

.custom-scrollbar::-webkit-scrollbar-thumb {
    background: var(--border-secondary);
    border-radius: 4px;
}

.custom-scrollbar::-webkit-scrollbar-thumb:hover {
    background: var(--text-muted);
}

/* Common Animations */
@keyframes pulse {
    0%, 100% { opacity: 1; }
    50% { opacity: 0.5; }
}

.pulse {
    animation: pulse 2s infinite;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

.spinner {
    animation: spin 1s linear infinite;
}

/* Sidebar Styles */
.sidebar {
    transition: transform 0.3s ease;
}

@media (max-width: 768px) {
    .sidebar {
        transform: translateX(-100%);
    }
    .sidebar.open {
        transform: translateX(0);
    }
}

/* Add more shared component styles as needed */
```

Then in each HTML file, link to it:

```html
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Prototype - Page Name</title>

    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>

    <!-- Shared Styles -->
    <link rel="stylesheet" href="styles.css">

    <style>
        /* Page-specific styles only */
    </style>
</head>
```

## Tailwind CSS Utility Patterns

### shadcn-Inspired Component Classes

**Button:**
```html
<!-- Primary -->
<button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-900">
    Button
</button>

<!-- Outline -->
<button class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100 focus:outline-none focus:ring-2 focus:ring-slate-900">
    Button
</button>

<!-- Ghost -->
<button class="inline-flex items-center justify-center rounded-md px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100 focus:outline-none focus:ring-2 focus:ring-slate-900">
    Button
</button>
```

**Card:**
```html
<div class="rounded-lg border border-slate-200 bg-white shadow-sm">
    <div class="p-6">
        <h3 class="text-lg font-semibold">Card Title</h3>
        <p class="mt-2 text-sm text-slate-600">Card description goes here.</p>
    </div>
</div>
```

**Input:**
```html
<input type="text"
       class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-900"
       placeholder="Enter text...">
```

**Badge:**
```html
<span class="inline-flex items-center rounded-full bg-slate-100 px-2.5 py-0.5 text-xs font-semibold text-slate-800">
    Badge
</span>
```

## Common UI Patterns

### Dashboard Layout
- Fixed sidebar navigation (desktop) / mobile menu
- Top navigation bar with user menu
- Main content area with grid of stat cards
- Tables and charts
- Responsive: sidebar collapses on mobile

### Form Pages
- Form sections with labels and inputs
- Validation messages (show with JavaScript)
- Submit/cancel buttons
- Multi-step forms with progress indicator
- Inline help text

### Data Tables
- Responsive table (scrollable on mobile)
- Sortable column headers
- Row actions (edit, delete buttons)
- Pagination controls
- Search/filter inputs

### Modal/Dialog
- Overlay background (backdrop)
- Centered dialog with animation
- Close button (×) and cancel/confirm actions
- Focus trap for accessibility
- ESC key to close

### Settings Pages
- Tab navigation for categories
- Form sections with separators
- Toggle switches (custom styled checkboxes)
- Danger zone with red styling
- Save buttons at section or page level

## Responsive Design Approach

Mobile-first with Tailwind breakpoints:
- `sm:` - 640px and up
- `md:` - 768px and up
- `lg:` - 1024px and up
- `xl:` - 1280px and up

**Example:**
```html
<div class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
    <!-- Stacks on mobile, 2 cols tablet, 4 cols desktop -->
</div>
```

## Accessibility Requirements

1. **CRITICAL:** Use semantic HTML (header, nav, main, section, article, footer)
2. Add ARIA labels for icon-only buttons: `<button aria-label="Close">×</button>`
3. Ensure keyboard navigation works (Tab, Enter, Space, Escape)
4. Provide visible focus indicators (Tailwind's `focus:ring-2` classes)
5. Use proper heading hierarchy (h1 → h2 → h3)
6. Add alt text for images: `<img src="..." alt="Description">`
7. Ensure sufficient color contrast (use Tailwind's default colors)
8. Label form inputs properly: `<label for="email">Email</label>`

## Vanilla JavaScript Patterns

### Modal Toggle
```javascript
function openModal(modalId) {
    document.getElementById(modalId).classList.remove('hidden');
}

function closeModal(modalId) {
    document.getElementById(modalId).classList.add('hidden');
}
```

### Tab Switching
```javascript
function switchTab(tabName) {
    // Hide all tab contents
    document.querySelectorAll('.tab-content').forEach(el => {
        el.classList.add('hidden');
    });

    // Show selected tab
    document.getElementById(tabName).classList.remove('hidden');

    // Update active tab styling
    document.querySelectorAll('.tab-button').forEach(el => {
        el.classList.remove('border-b-2', 'border-slate-900');
    });
    event.target.classList.add('border-b-2', 'border-slate-900');
}
```

### Form Validation
```javascript
document.getElementById('myForm').addEventListener('submit', function(e) {
    e.preventDefault();

    const email = document.getElementById('email').value;
    const errorEl = document.getElementById('email-error');

    if (!email.includes('@')) {
        errorEl.textContent = 'Please enter a valid email';
        errorEl.classList.remove('hidden');
        return;
    }

    errorEl.classList.add('hidden');
    // Submit form
});
```

### Mobile Menu Toggle
```javascript
function toggleMobileMenu() {
    const menu = document.getElementById('mobile-menu');
    menu.classList.toggle('hidden');
}
```

## Shared Styles Management

**CRITICAL RULE:** When multiple HTML files share the same styles (especially theme systems, CSS variables, or common component styles), **ALWAYS** centralize them in a `styles.css` file instead of duplicating them in each HTML file.

### When to Create styles.css:

1. **Multiple pages share the same styles** (theme variables, colors, fonts)
2. **Common component styles** used across pages (buttons, cards, forms)
3. **Theme system with CSS variables** for light/dark modes
4. **Complex custom styles** that would clutter inline `<style>` tags

### What to Keep in styles.css:

- CSS custom properties (variables) for themes
- Component base styles that are reused
- Animation keyframes used in multiple places
- Typography settings
- Common utility classes

### What to Keep Inline in HTML:

- Page-specific styles that only apply to one file
- Very simple, one-off customizations
- Styles that are truly unique to a single page

### Example structure:

```
prototype/
├── styles.css           # Shared styles, theme variables, common components
├── index.html           # Links to styles.css, contains page-specific inline styles
├── settings.html        # Links to styles.css, contains page-specific inline styles
└── dashboard.html       # Links to styles.css, contains page-specific inline styles
```

In each HTML file:
```html
<head>
    <link rel="stylesheet" href="styles.css">
    <style>
        /* Only page-specific styles here */
    </style>
</head>
```

## Workflow Example

```bash
# STEP 1: Check for existing prototypes ONLY in prototype folders
glob **/prototype*/*.html
glob prototype/**/*.html
glob prototype/**/*.css

# STEP 2: DO NOT check production web projects unless explicitly requested
# Skip checking Web/, src/, app/, client/ folders

# STEP 3: Identify if shared styles exist or are needed
# If multiple pages will share styles, create styles.css FIRST

# STEP 4: Create shared styles.css in prototype folder
write prototype/styles.css         # For theme variables, common components

# STEP 5: Create prototype HTML files that link to styles.css
write prototype/index.html         # Links to styles.css
write prototype/settings.html      # Links to styles.css
write prototype/users.html         # Links to styles.css

# STEP 6: Verify in browser
bash start prototype/index.html    # Opens in default browser
```

## Best Practices Checklist

1. **File Structure:**
   - [ ] Use descriptive filenames (prototype-[feature].html)
   - [ ] **CRITICAL:** Extract shared styles to styles.css when multiple pages exist
   - [ ] Link to styles.css from all HTML files that need shared styles
   - [ ] Keep page-specific styles inline in each HTML file
   - [ ] Add comments to explain complex sections

2. **HTML:**
   - [ ] Use semantic HTML5 elements
   - [ ] Proper heading hierarchy
   - [ ] All forms have labels
   - [ ] All images have alt text
   - [ ] Use data attributes for JS hooks

3. **Styling:**
   - [ ] Use Tailwind utility classes
   - [ ] Follow mobile-first responsive design
   - [ ] Maintain consistent spacing
   - [ ] Use shadcn-inspired color palette
   - [ ] Add hover/focus states to interactive elements

4. **Accessibility:**
   - [ ] Semantic HTML elements
   - [ ] ARIA labels where needed
   - [ ] Keyboard navigation works
   - [ ] Focus indicators visible
   - [ ] Sufficient color contrast

5. **Interactivity:**
   - [ ] Use vanilla JavaScript (no jQuery/frameworks)
   - [ ] Keep JS simple and readable
   - [ ] Add comments explaining functionality
   - [ ] Handle edge cases (empty states, errors)
   - [ ] Test on mobile devices

6. **Code Quality:**
   - [ ] Properly indented HTML
   - [ ] Consistent naming conventions
   - [ ] Descriptive IDs and classes
   - [ ] Comments for complex logic
   - [ ] Remove unused code

7. **Testing:**
   - [ ] Open in browser and verify layout
   - [ ] Test responsive breakpoints
   - [ ] Test all interactive elements
   - [ ] Check keyboard navigation
   - [ ] Verify on mobile device or browser dev tools

## Common Prototype Templates

See SHADCN-REFERENCE.md for complete HTML templates including:
- Dashboard layouts with sidebar
- Form pages with validation
- Data tables with sorting
- Modal dialogs and confirmations
- Settings pages with tabs
- Login/signup pages
- Landing pages
- Admin panels
- E-commerce product pages
- Blog/article layouts

## Color Schemes

**Default (Slate):**
- Primary: slate-900
- Secondary: slate-100
- Muted: slate-500
- Border: slate-300
- Background: white/gray-50

**Alternative (Zinc):**
- Primary: zinc-900
- Secondary: zinc-100
- Muted: zinc-500
- Border: zinc-300

**Custom Branding:**
Ask user for brand colors and configure in Tailwind config.

## Error Prevention

1. **CRITICAL:** Always include viewport meta tag for mobile responsiveness
2. **CRITICAL:** Use Tailwind CDN script tag (don't forget it!)
3. **CRITICAL:** Test prototype by opening HTML file in browser
4. Use valid HTML5 (check with browser dev tools)
5. Ensure all IDs are unique
6. Test JavaScript in browser console for errors
7. Verify all links/buttons have proper click handlers
