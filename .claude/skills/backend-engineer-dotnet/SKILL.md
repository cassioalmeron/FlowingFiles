---
name: backend-engineer-dotnet
description: Orchestrator for .NET backend development. Routes to specialized backend skills (csharp-specialist for C# code, efcore-specialist for database/EF Core work). Use this skill when working on .NET backend tasks.
license: MIT
---

# Backend Engineer - .NET

This skill routes .NET backend development tasks to specialized skills.

## Available Backend Skills

### /csharp-specialist
Use for C# code: API controllers, services, DTOs, business logic, naming conventions, async/await patterns.

### /efcore-specialist
Use for database work: Entity Framework Core migrations, entity configurations, DbContext setup, query optimization, cross-database compatibility (SQLite/Postgres).

## Quick Decision

- **Writing C# code?** → Use `/csharp-specialist`
- **Working with database/EF Core?** → Use `/efcore-specialist`
- **Both?** → Start with `/efcore-specialist` for data layer, then `/csharp-specialist` for application layer

## Common Tasks

| Task | Use |
|------|-----|
| New API endpoint | `/csharp-specialist` |
| Database migration | `/efcore-specialist` |
| Entity model + configuration | `/efcore-specialist` |
| Service/business logic | `/csharp-specialist` |
| Query optimization | `/efcore-specialist` |
| DTOs, validation, error handling | `/csharp-specialist` |

