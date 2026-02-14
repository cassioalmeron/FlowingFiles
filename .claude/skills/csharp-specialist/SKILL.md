---
name: csharp-specialist
description: Expert in C# development with strict adherence to naming conventions, coding standards, and best practices. Use this skill when writing new C# code, implementing features, or ensuring code follows modern C# idioms and conventions.
license: MIT
---

# C# Specialist

This skill provides specialized guidance for writing high-quality C# code that follows established conventions and best practices.

## When to Use This Skill

Use this skill when:
- Writing new C# classes, methods, or features
- Implementing backend API endpoints or services
- Working with Entity Framework Core models and database contexts
- Creating C# code that needs to follow strict naming and coding conventions

## Core Principles

### Naming Conventions

Follow these strict C# naming conventions:

**PascalCase:**
- Class names: `CustomerService`, `RewardStartDbContext`
- Method names: `GetCustomerById`, `SaveChangesAsync`
- Property names: `FirstName`, `IsActive`
- Public fields (avoid when possible): `MaxRetryCount`
- Namespace names: `RewardStart.Core`, `RewardStart.Api`
- Interface names (with 'I' prefix): `ICustomerRepository`, `IUserService`

**camelCase:**
- Local variables: `customerId`, `userName`
- Method parameters: `userId`, `activityName`
- Private fields (with underscore prefix): `_dbContext`, `_logger`

**ALL_CAPS:**
- Constants: `MAX_RETRY_COUNT`, `DEFAULT_PAGE_SIZE`

### Code Structure

**Namespace Declaration:**
- Use file-scoped namespaces (C# 11+) with `namespace NamespaceName;` syntax
- This eliminates unnecessary braces when the file contains only a single namespace
- Example:
```csharp
namespace RewardStart.Core.Extensions;

public static class DtoBaseExtension
{
    // class content
}
```

**Class Organization:**
1. Private fields
2. Constructors
3. Public properties
4. Public methods
5. Private methods

**Method Guidelines:**
- Keep methods focused on a single responsibility
- Use async/await for I/O operations with `Async` suffix
- Prefer explicit return types over `var` for public methods
- Use meaningful parameter names that indicate purpose

**Property Guidelines:**
- Use auto-properties when possible: `public string Name { get; set; }`
- Use init accessors for immutable properties: `public int Id { get; init; }`
- Avoid public fields; use properties instead

**Single-Line Statement Pattern:**
- When a scope block contains only one instruction, omit the braces
- This applies to `if`, `else`, `for`, `foreach`, `while`, and `using` statements
- When an `if` block has a `return`, the `else` is unnecessary (use early return pattern)

Example:
```csharp
// Incorrect - unnecessary braces for single statement
if (user == null)
{
    return NotFound();
}

// Correct - omit braces for single statement
if (user == null)
    return NotFound();

// Single-line if/else without returns
if (isActive)
    status = "Active";
else
    status = "Inactive";

// Incorrect - unnecessary else when if block returns
if (user == null)
    return NotFound();
else
    return Ok(user);

// Correct - omit else, use early return pattern
if (user == null)
    return NotFound();

return Ok(user);

// Multiple statements - braces required
if (user == null)
{
    _logger.LogWarning("User not found: {UserId}", userId);
    return NotFound();
}
```

**Expression-Bodied Members:**
- When a method, property, or member contains only a single expression, use expression-bodied syntax with `=>`
- This applies to methods, properties, indexers, constructors, and finalizers
- Makes code more concise and readable for simple operations

Example:
```csharp
// Incorrect - full method body for single expression
public static bool IsAdmin(this User user)
{
    return user.Id == UserConstants.ADMIN_USER_ID;
}

// Correct - expression-bodied member
public static bool IsAdmin(this User user) =>
    user.Id == UserConstants.ADMIN_USER_ID;

// Other examples
public string FullName => $"{FirstName} {LastName}";
public int GetAge() => DateTime.Now.Year - BirthYear;
public decimal CalculateTotal(decimal price, decimal tax) => price + tax;

// Void methods
public void LogError(string message) => _logger.LogError(message);

// Property with get/set
private string _name;
public string Name
{
    get => _name;
    set => _name = value?.Trim() ?? string.Empty;
}

// Still use full body when multiple statements needed
public bool ValidateUser(User user)
{
    _logger.LogInformation("Validating user {UserId}", user.Id);
    return user.IsActive && user.EmailVerified;
}
```

### Modern C# Features

Leverage modern C# features when appropriate:
- Nullable reference types: Enable and use `?` for nullable types
- Pattern matching: Use `is`, `switch` expressions
- Collection initializers: `new List<int> { 1, 2, 3 }`
- LINQ for data operations
- Record types for DTOs and immutable data

**String Interpolation (MANDATORY):**
- Always use string interpolation with `$""` syntax
- Never use string concatenation with `+` operator
- Never use `string.Format()` unless absolutely necessary

Example:
```csharp
// Incorrect - string concatenation
var message = "Hello " + name + ", you have " + count + " items";
var log = "User " + userId + " logged in";

// Incorrect - string.Format
var message = string.Format("Hello {0}, you have {1} items", name, count);

// Correct - string interpolation
var message = $"Hello {name}, you have {count} items";
var log = $"User {userId} logged in";
var path = $"/api/users/{userId}/activities/{activityId}";

// For logging with structured logging
_logger.LogInformation("User {UserId} logged in at {LoginTime}", userId, loginTime);
```

### Dependency Injection

Follow ASP.NET Core DI patterns:
- Constructor injection for required dependencies
- Register services appropriately (Scoped, Transient, Singleton)
- Use interfaces for abstraction: `ICustomerService`, `IRepository<T>`

### Error Handling

- Use specific exception types
- Avoid catching general `Exception` unless necessary
- Log exceptions with appropriate context
- Return appropriate HTTP status codes in API controllers

### Async/Await

- Always use `async`/`await` for I/O operations
- Suffix async methods with `Async`: `GetUserAsync`, `SaveChangesAsync`
- Avoid `async void` except for event handlers
- Use `ConfigureAwait(false)` in library code when appropriate

## Entity Framework Core

When working with EF Core:
- DbContext classes should end with `DbContext`: `RewardStartDbContext`
- Use DbSet properties for entity collections: `public DbSet<Customer> Customers { get; set; }`
- Configure entities using Fluent API or data annotations
- Use migrations for schema changes
- Implement IEntityTypeConfiguration for complex entity configs

## API Controllers

For ASP.NET Core controllers:
- Use `[ApiController]` attribute
- Use route attributes: `[Route("api/[controller]")]`
- Return `ActionResult<T>` or `IActionResult`
- Use HTTP verb attributes: `[HttpGet]`, `[HttpPost]`, etc.
- Validate input with data annotations or FluentValidation
- Return appropriate status codes: `Ok()`, `NotFound()`, `BadRequest()`, `Created()`

## Quality Standards

Before completing any C# code:
- Verify all naming follows conventions (PascalCase, camelCase, etc.)
- Ensure proper access modifiers (public, private, protected, internal)
- Check for proper use of async/await
- Confirm dependencies are injected via constructor
- Validate that code follows single responsibility principle
- Ensure proper null handling with nullable reference types
