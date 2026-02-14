---
name: Feature Implementation Workflow
description: This skill guides developers through a structured 5-phase process for implementing features, from task definition through knowledge base integration. Ensures consistent documentation, code analysis, and business rule capture.
---

# Feature Implementation Workflow

## Purpose

To streamline feature implementation by providing a guided, step-by-step workflow that ensures:
- Clear task definition with acceptance criteria
- Comprehensive technical analysis before implementation
- All affected code areas are identified
- Implementation is properly routed to specialized skills
- Business rules are documented and integrated into the knowledge base
- No steps are skipped or overlooked

## When to Use This Skill

Use this skill when implementing a new feature or significant change that:
- Spans multiple files or components
- Has business logic implications
- Requires new documentation
- Affects API behavior or user experience
- Benefits from structured planning before execution

## How to Use This Skill

Work through each phase sequentially, following the guidance and prompts. The workflow has built-in checkpoints to ensure thoroughness.

### Phase 1: Task Definition

**Purpose:** Capture clear requirements and create persistent task documentation.

**Steps:**
1. Define the feature name and brief description
2. Collect detailed requirements and acceptance criteria
3. Identify all files/endpoints that will be affected
4. Create task in task management system (optional, but recommended)
5. Create `docs/tasks/{feature-name}.md` with:
   - Task ID/name
   - Current status
   - Overview
   - Requirements
   - Acceptance criteria
   - Skills involved
   - Implementation notes placeholder

**Checkpoint:** Before proceeding, confirm:
- [ ] Feature name is clear and specific
- [ ] Requirements are documented
- [ ] Acceptance criteria are testable
- [ ] All affected areas are identified (ask: "Are there other endpoints/files that need changes?")

### Phase 2: Technical Analysis

**Purpose:** Understand existing implementation before making changes.

**Steps:**
1. Explore the affected files using Explore agent or Glob/Grep tools
2. Document current implementation with code snippets
3. Collect examples of what needs to change
4. Identify all endpoints/methods that need modification
5. Update `docs/tasks/{feature-name}.md` with "Code Analysis" section including:
   - Affected files list
   - Current state (code snippets - before)
   - Proposed changes (code snippets - after)
   - Why changes are needed
   - Benefits of implementation

**Checkpoint:** Before proceeding, confirm:
- [ ] Current implementation is understood
- [ ] All affected endpoints/methods are documented
- [ ] Before/after code snippets are clear
- [ ] Related code context is captured (models, DTOs, services)
- [ ] Edge cases identified (ask: "Did you miss any endpoints like login vs register?")

### Phase 3: Implementation

**Purpose:** Execute the implementation using appropriate specialized skills.

**Steps:**
1. Review the checkpoint from Phase 2 - confirm all areas identified
2. Route to appropriate skill based on affected code:
   - Backend .NET: Use `/backend-engineer-dotnet`
   - C# code: Use `/csharp-specialist`
   - Database/EF Core: Use `/efcore-specialist`
   - React/TypeScript: Use `/react-specialist`
   - UI/Frontend: Use `/frontend-design`
3. Work through implementation with the routed skill
4. Mark task status as "in_progress"
5. After implementation complete, mark task status as "completed"

**Checkpoint:** Before proceeding, confirm:
- [ ] Implementation is complete
- [ ] All identified affected areas have been modified
- [ ] Code follows project conventions
- [ ] Tests pass (if applicable)
- [ ] Task is marked as completed

**IMPORTANT:** Do NOT report the feature as "done" after this phase. Phases 4 and 5 (documentation) are mandatory. Immediately proceed to Phase 4 after implementation is verified.

### Phase 4: Business Rules Documentation

**Purpose:** Document business logic and technical reference for stakeholders, AI context, and future developers.

**Steps:**
1. Create `docs/documentation/{feature-name}-business-rules.md`
2. Document business rules:
   - What is the rule?
   - When does it apply?
   - What are the business outcomes?
   - Why does this rule exist?
3. Include technical reference for developer context:
   - Data model overview (entities, enums, relationships)
   - API endpoints summary (method, route, description)
   - Key implementation files table
   - Access control rules
4. Include concrete examples of user/business scenarios
5. Update the task document status to "Completed" and check off all acceptance criteria

**Sections to include:**
- Feature overview
- Data model (entities and enums)
- Business rules (grouped by domain area)
- Access control rules
- Frontend behavior rules (if applicable)
- API endpoints reference table
- Key implementation files table

**Checkpoint:** Before proceeding, confirm:
- [ ] Business rules are clearly stated
- [ ] Technical reference (data model, API, files) is included
- [ ] Examples are concrete and relatable
- [ ] Task document is marked as Completed with all criteria checked

### Phase 5: Knowledge Base Integration

**Purpose:** Make business rules available to AI and future developers through documentation index.

**Steps:**
1. Update `docs/documentation/index.md` to include new business rules document
2. Add entry under appropriate category (Business Rules, Features, etc.)
3. Include brief description of what the document covers
4. Optionally update `CLAUDE.md` if this introduces new project patterns

**Checkpoint:** Before completing workflow, confirm:
- [ ] Documentation Index updated
- [ ] New document is properly referenced
- [ ] All related documentation is linked appropriately

## Post-Workflow

A feature is only considered **complete** after ALL 5 phases are done:
- Phase 1-2: Task is defined and technically analyzed
- Phase 3: Code is implemented and tested
- Phase 4: Business rules are documented, task marked as Completed
- Phase 5: Documentation index is updated

**Never report a feature as "done" after Phase 3.** The documentation phases (4 and 5) must be completed before the workflow ends. This ensures business rules are captured while context is fresh.

## Common Patterns

### Catching Missed Endpoints
During Phase 2, explicitly ask: "Are there other endpoints (login vs register, create vs update) that need the same changes?" This prevents discovering missing changes during implementation.

### Separating Business and Technical Docs
- **Business Rules Doc:** What and Why from business perspective
- **Task Doc:** How and What from technical perspective
- **Code Comments:** Why specific code patterns were chosen (in code)

### Routing to Skills
- If changes span multiple layers, start with data layer (use `/efcore-specialist`), then application layer (`/csharp-specialist`)
- Include context about all affected areas so routed skill has full picture
