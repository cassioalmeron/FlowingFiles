# CLAUDE.md

## Skill Compliance

When a plan or task references a project skill (from `.claude/skills/`), you MUST:

1. **Read the skill file completely** before writing any code
2. **Follow the skill's full workflow** — all steps, in order, no skipping
3. **Read all referenced documents** (e.g., `REACT-REFERENCE.md`) as mandatory prerequisites, not optional suggestions
4. **Apply every requirement** found in referenced docs (libraries, patterns, conventions) — do not cherry-pick
5. **Verify compliance** before considering the task done: re-check the skill's checklist/rules against what was implemented

## Docs Folder

All project documentation lives in `docs/`. Before starting any feature or task, check whether relevant docs exist.

| Folder / File | Purpose |
|---|---|
| `docs/documentation/` | Completed feature documentation. Start here to understand what's already built. `index.md` lists all features. |
| `docs/plans/` | Implementation plans for features or architectural work. Read the relevant plan before implementing. |
| `docs/tasks/` | Task trackers for in-progress or planned work. Check here for scope, acceptance criteria, and status. |
| `docs/react-conversion-plan.md` | Plan for converting the WPF desktop app to React. |
| `docs/backend-standards.md` | Coding standards and conventions for the .NET backend — mandatory reading before backend work. |

### Rules

- **Before implementing a feature**: read its task file (`docs/tasks/`) and any linked plan (`docs/plans/`)
- **After implementing a feature**: write or update its documentation in `docs/documentation/` and update `docs/documentation/index.md`
- **Do not create new docs** outside these folders unless there is a clear reason
