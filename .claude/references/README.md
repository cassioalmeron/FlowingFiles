# Shared References

This folder contains shared reference documentation used by multiple Claude Code skills.

## Purpose

Instead of duplicating documentation across skills, shared references are stored here and referenced by multiple skills. This ensures:
- **Single source of truth** for common patterns
- **Easier maintenance** - update once, applies everywhere
- **Consistency** across related skills
- **Reduced file duplication**

## Available References

### [REACT-REFERENCE.md](./REACT-REFERENCE.md)
Complete React reference with TypeScript examples, component templates, hooks, and patterns.

**Used by:**
- `react-specialist` - General React development
- `react-converter` - HTML to React conversion

**Contents:**
- Project setup (Vite + TypeScript)
- Component templates (Button, Card, Input, Modal, Badge)
- Custom hooks (useModal, useForm, useLocalStorage, useFetch, useDebounce)
- Styling solutions (Tailwind, CSS Modules, styled-components)
- Common conversions (Dashboard, Forms, Tables)
- Advanced patterns (Context, Error Boundaries, Protected Routes, Infinite Scroll)
- Complete example applications

## Adding New References

When creating new shared references:

1. **Create the reference file** in `.claude/references/`
2. **Name it descriptively** (e.g., `TYPESCRIPT-PATTERNS.md`, `API-DESIGN.md`)
3. **Update this README** with a description and which skills use it
4. **Reference it from skills** using relative path: `../../references/FILENAME.md`

## File Naming Convention

- Use UPPERCASE for reference file names
- Use descriptive, specific names
- Include file extension (`.md`)
- Examples:
  - ✅ `REACT-REFERENCE.md`
  - ✅ `TYPESCRIPT-PATTERNS.md`
  - ✅ `API-BEST-PRACTICES.md`
  - ❌ `reference.md`
  - ❌ `docs.md`

## Linking from Skills

In your skill's `skill.md`, reference shared documentation:

```markdown
See [REACT-REFERENCE.md](../../references/REACT-REFERENCE.md) for complete examples.
```

This creates a clickable link that works in both CLI and IDE environments.

## Directory Structure

```
.claude/
├── skills/
│   ├── react-specialist/
│   │   └── skill.md → references ../../references/REACT-REFERENCE.md
│   ├── react-converter/
│   │   └── skill.md → references ../../references/REACT-REFERENCE.md
│   └── ui-prototype/
│       ├── skill.md
│       └── SHADCN-REFERENCE.md (skill-specific, not shared)
└── references/
    ├── README.md (this file)
    └── REACT-REFERENCE.md (shared)
```

## When to Share vs Keep Skill-Specific

### Share when:
- Multiple skills need the same reference
- The content is technology/framework-specific (React, TypeScript, etc.)
- The patterns are universal best practices
- Updates should apply to all skills using it

### Keep skill-specific when:
- Only one skill uses the reference
- The content is specific to that skill's unique purpose
- Examples are tailored to that skill's workflow
- Independence from other skills is beneficial

## Examples of Future Shared References

Potential references that could be added:

- `TYPESCRIPT-PATTERNS.md` - TypeScript best practices, utility types, generics
- `TAILWIND-COMPONENTS.md` - Common Tailwind component patterns
- `API-DESIGN.md` - REST API design principles
- `TESTING-PATTERNS.md` - Testing strategies for different frameworks
- `DOCKER-BEST-PRACTICES.md` - Docker optimization and security
- `GIT-WORKFLOWS.md` - Git branching strategies and commit conventions

---

**Maintained by**: Claude Code Skills
**Last Updated**: 2025-12-20
