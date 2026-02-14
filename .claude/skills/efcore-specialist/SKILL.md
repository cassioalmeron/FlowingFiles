---
name: efcore-specialist
description: Expert in Entity Framework Core development with focus on migrations, entity configurations, query optimization, and best practices. Specializes in cross-database compatibility (SQLite for integration tests, Postgres for production). Use this skill when working with EF Core database operations, DbContext configuration, entity modeling, or setting up integration tests.
license: MIT
---

# EF Core Specialist

This skill provides expert guidance for Entity Framework Core development, covering migrations, entity configurations, query optimization, and best practices.

## When to Use This Skill

Use this skill when:
- Creating or modifying EF Core migrations
- Configuring DbContext or entity relationships
- Writing or optimizing database queries
- Setting up entity configurations with Fluent API
- Diagnosing EF Core performance issues
- Implementing database patterns with EF Core
- Setting up cross-database compatibility (SQLite for tests, Postgres for production)
- Creating integration tests with in-memory SQLite databases

## Core Principles

### 1. Migrations & DbContext Setup

**Creating Migrations:**
- Always use descriptive migration names that explain the change (e.g., `Add-UserEmailIndex`, `Update-ActivityRelationships`)
- Review generated migration code before applying to catch unintended changes
- Use `Add-Migration` (PMC) or `dotnet ef migrations add` (CLI)
- Apply migrations with `Update-Database` or `dotnet ef database update`

**DbContext Configuration:**
- Register DbContext in DI with `AddDbContext<T>()` in `Program.cs` or `Startup.cs`
- Use configuration options: connection strings, pooling, query behavior
- Override `OnModelCreating()` for Fluent API configurations
- Keep DbContext lightweight - avoid business logic

**Connection Strings:**
- Store connection strings in `appsettings.json`, never hardcode
- Use different connection strings per environment (Development, Production)
- Access via `IConfiguration` or `DbContextOptionsBuilder`

### 2. Entity Configuration

**Fluent API Over Data Annotations:**
- Prefer Fluent API in `OnModelCreating()` for complex configurations
- Use separate configuration classes implementing `IEntityTypeConfiguration<T>` for better organization
- Apply configurations with `modelBuilder.ApplyConfiguration(new EntityConfiguration())`

**Common Configurations:**
- Primary keys: `HasKey(e => e.Id)`
- Required fields: `IsRequired()`
- Max length: `HasMaxLength(100)`
- Column names: `HasColumnName("custom_name")`
- Table names: `ToTable("TableName")`
- Indexes: `HasIndex(e => e.Email).IsUnique()`
- Default values: `HasDefaultValue()` or `HasDefaultValueSql()`

**Relationships:**
- One-to-Many: `HasMany().WithOne().HasForeignKey()`
- Many-to-Many: Use join entity or `HasMany().WithMany()` (EF Core 5+)
- One-to-One: `HasOne().WithOne().HasForeignKey()`
- Always specify `OnDelete` behavior explicitly: `OnDelete(DeleteBehavior.Cascade/Restrict/SetNull)`

**Example Configuration Class:**
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Activities)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### 3. Cross-Database Compatibility (SQLite & Postgres)

**Use Case: SQLite for Integration Tests, Postgres for Production**

A common pattern is to use PostgreSQL in production/development and SQLite for integration tests. This provides:
- **Fast test execution**: In-memory SQLite databases are extremely fast
- **No infrastructure needed**: Tests run without Docker or database servers
- **Isolated tests**: Each test gets a fresh database instance
- **CI/CD friendly**: No external dependencies in build pipelines

To support this pattern, migrations and configurations must work across both providers. Follow these guidelines to ensure compatibility.

**Provider Configuration:**

```csharp
// Define database provider enum
public enum DatabaseProvider
{
    SQLite,
    Postgres
}

// In Program.cs - determine provider from configuration
var providerString = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
var databaseProvider = Enum.Parse<DatabaseProvider>(providerString, ignoreCase: true);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    switch (databaseProvider)
    {
        case DatabaseProvider.Postgres:
            options.UseNpgsql(connectionString);
            break;
        case DatabaseProvider.SQLite:
            options.UseSqlite(connectionString);
            break;
        default:
            throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}");
    }
});
```

**appsettings.json:**
```json
{
  "DatabaseProvider": "SQLite",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  }
}
```

**appsettings.Production.json:**
```json
{
  "DatabaseProvider": "Postgres",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=rewardstar;Username=postgres;Password=yourpassword"
  }
}
```

**Benefits of Using Enum:**
- ✅ Type-safe: Compile-time checking prevents typos ("Postgre" vs "Postgres")
- ✅ IntelliSense support: IDE autocomplete for available providers
- ✅ Refactoring-friendly: Rename providers across entire codebase
- ✅ Exhaustive switch checking: Compiler ensures all cases are handled
- ✅ Clear intent: `DatabaseProvider.Postgres` is more explicit than `"Postgres"`

**Compatible Data Types:**

| C# Type | SQLite | Postgres | Notes |
|---------|--------|----------|-------|
| `int`, `long` | INTEGER | integer, bigint | ✅ Compatible |
| `string` | TEXT | text, varchar | ✅ Use `HasMaxLength()` for both |
| `bool` | INTEGER (0/1) | boolean | ⚠️ **Requires `type:` in migrations** |
| `DateTime` | TEXT (ISO8601) | timestamp | ✅ **Works automatically - NO `type:` needed** |
| `decimal` | TEXT | numeric | ✅ Use `HasPrecision()` |
| `Guid` | TEXT (36 chars) | uuid | ✅ Compatible |
| `byte[]` | BLOB | bytea | ✅ Compatible |

**Configuration for Cross-Compatibility:**
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        // STRING: Always specify max length for compatibility
        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(255);  // Works on both

        // DECIMAL: Specify precision for consistency
        builder.Property(u => u.Balance)
               .HasPrecision(18, 2);  // Works on both

        // DATETIME: Use DateTime (not DateTimeOffset for SQLite compatibility)
        builder.Property(u => u.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");  // Both support this

        // GUID: Works on both (stored as TEXT in SQLite, uuid in Postgres)
        builder.Property(u => u.ExternalId)
               .HasColumnType("TEXT");  // Explicit for SQLite

        // INDEXES: Standard syntax works on both
        builder.HasIndex(u => u.Email).IsUnique();
    }
}
```

**Migration Patterns:**

**✅ Compatible Default Values:**
```csharp
// Both support CURRENT_TIMESTAMP
migrationBuilder.CreateTable(
    name: "Users",
    columns: table => new
    {
        CreatedAt = table.Column<DateTime>(nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP")
    });

// Both support literal defaults
builder.Property(u => u.IsActive)
       .HasDefaultValue(true);
```

**❌ Avoid Database-Specific SQL:**
```csharp
// BAD: Postgres-specific
.HasDefaultValueSql("NOW()")  // SQLite doesn't understand NOW()

// BAD: Postgres-specific
.HasColumnType("jsonb")  // SQLite doesn't support jsonb

// GOOD: Use compatible syntax
.HasDefaultValueSql("CURRENT_TIMESTAMP")
```

**⚠️ CRITICAL: Auto-Increment Identity Columns (Dual Annotation Required)**

When creating tables with auto-increment integer primary keys, you **MUST** include **BOTH** provider annotations. Missing one causes silent failures on that database provider.

```csharp
// ✅ CORRECT: Both annotations present
Id = table.Column<int>(nullable: false)
    .Annotation("Sqlite:Autoincrement", true)
    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

// ❌ WRONG: Missing Npgsql annotation - PostgreSQL cannot auto-generate IDs
Id = table.Column<int>(type: "INTEGER", nullable: false)
    .Annotation("Sqlite:Autoincrement", true),
// Error: "null value in column 'Id' of relation 'TableName' violates not-null constraint"

// ❌ WRONG: Missing Sqlite annotation - SQLite cannot auto-generate IDs
Id = table.Column<int>(nullable: false)
    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
```

**This also requires the Npgsql import at the top of the migration file:**
```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
```

**Why this happens:** EF Core migration scaffolding only generates annotations for the **active provider** at generation time. If you generate migrations with SQLite active, only `Sqlite:Autoincrement` is included. You must **manually add** the missing provider's annotation.

**Fluent API Configuration (entity configurations):**
```csharp
// Configure integer primary key - both handle auto-increment
builder.Property(u => u.Id)
       .ValueGeneratedOnAdd();  // Works on both

// For Guid primary keys, use client-side generation in code: Guid.NewGuid()
```

**Cross-Database Migration Example:**
```csharp
public partial class CreateUserTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Email = table.Column<string>(maxLength: 255, nullable: false),
                Balance = table.Column<decimal>(precision: 18, scale: 2, nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false,
                    defaultValueSql: "CURRENT_TIMESTAMP"),
                IsActive = table.Column<bool>(nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);
    }
}
```

**SQLite Limitations to Consider:**

1. **No ALTER TABLE support for many operations:**
   - Can't drop columns (must recreate table)
   - Can't modify column types (must recreate table)
   - EF Core handles this with table recreation, but be aware of performance impact

2. **Case sensitivity:**
   - SQLite is case-insensitive by default for ASCII
   - Postgres is case-sensitive
   - Use `.ToLower()` or `.ToUpper()` in queries for consistent behavior

3. **Foreign key constraints:**
   - SQLite requires `PRAGMA foreign_keys = ON;` (EF Core enables this automatically)
   - Always verify foreign keys work in SQLite testing

4. **DateTimeOffset:**
   - Limited support in SQLite - prefer `DateTime` for cross-compatibility

**⚠️ CRITICAL: Boolean Column Type Issue**

When generating migrations, EF Core includes provider-specific column types that cause failures when switching databases:

```csharp
// BAD: Migration generated with SQLite
IsActive = table.Column<bool>(type: "INTEGER", nullable: false)  // Fails on Postgres!

// BAD: Migration generated with Postgres
IsActive = table.Column<bool>(type: "boolean", nullable: false)  // Fails on SQLite!
```

**Why Boolean Columns Are Problematic:**

The database provider used DURING migration generation affects the entire migration structure, not just explicit `type:` parameters. When you generate a migration with SQLite active, EF Core scaffolds the migration with SQLite semantics (booleans as INTEGER). Simply removing the `type:` parameter does NOT fix this - the migration will still use INTEGER even when applied to PostgreSQL.

**Required Solution: Use Provider Detection for Boolean Columns**

For true cross-database compatibility, you MUST use runtime provider detection for all boolean columns:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Detect database provider at runtime for cross-database compatibility
    var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";
    var isPostgres = migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL";
    var boolType = isSqlite ? "INTEGER" : "boolean";

    migrationBuilder.CreateTable(
        name: "Users",
        columns: table => new
        {
            Id = table.Column<int>(nullable: false)
                .Annotation("Sqlite:Autoincrement", true)
                .Annotation("Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            IsActive = table.Column<bool>(
                type: boolType,  // Runtime provider detection
                nullable: false,
                defaultValue: true),
            EmailVerified = table.Column<bool>(
                type: boolType,  // Apply to ALL boolean columns
                nullable: false)
        });
}
```

**Key Points:**
- SQLite stores booleans as `INTEGER` (0 = false, 1 = true)
- PostgreSQL uses native `boolean` type
- The migration must specify the correct type based on the ACTIVE provider at runtime
- This applies to ALL boolean columns (flags, status fields, day-of-week booleans, etc.)

**Best Practice Workflow:**
1. Generate migration: `dotnet ef migrations add MigrationName`
2. Open migration file and add the `using` import if not present:
   ```csharp
   using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
   ```
3. Add provider detection at the start of `Up()`:
   ```csharp
   var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";
   var boolType = isSqlite ? "INTEGER" : "boolean";
   var dateTimeType = isSqlite ? "TEXT" : "timestamp without time zone";
   ```
4. **Check ALL `Id` columns** have BOTH auto-increment annotations:
   ```csharp
   Id = table.Column<int>(nullable: false)
       .Annotation("Sqlite:Autoincrement", true)
       .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
   ```
5. Update ALL boolean columns to use `type: boolType`
6. Update ALL DateTime columns to use `type: dateTimeType`
7. Test migration on BOTH SQLite and Postgres before committing
7. Verify the correct types are created:
   - SQLite: Check with `.schema TableName` - should show INTEGER for bools
   - Postgres: Check with `\d "TableName"` - should show boolean for bools
8. Always review generated migrations - don't trust them blindly

**Example: Complete Migration with Cross-Database Type Handling**

```csharp
public partial class CreateUserTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // CRITICAL: Provider detection for cross-database column types
        var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";
        var boolType = isSqlite ? "INTEGER" : "boolean";
        var dateTimeType = isSqlite ? "TEXT" : "timestamp without time zone";

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Email = table.Column<string>(maxLength: 255, nullable: false),
                IsActive = table.Column<bool>(type: boolType, nullable: false, defaultValue: true),
                EmailVerified = table.Column<bool>(type: boolType, nullable: false),
                Monday = table.Column<bool>(type: boolType, nullable: false),
                Tuesday = table.Column<bool>(type: boolType, nullable: false),
                CreatedAt = table.Column<DateTime>(type: dateTimeType, nullable: false),
                LastLoginAt = table.Column<DateTime>(type: dateTimeType, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Users");
    }
}
```

**✅ DateTime Columns: Usually Compatible Without Special Handling**

Unlike boolean columns, DateTime columns typically work seamlessly across SQLite and Postgres **IF you don't explicitly specify the column type**. However, understanding the behavior is critical to avoid issues.

**How DateTime Works Across Providers:**

| Database | Storage Type | Format | EF Core Behavior |
|----------|-------------|---------|------------------|
| SQLite | TEXT | ISO8601 string (e.g., "2026-01-15 10:30:00") | Converts automatically |
| Postgres | timestamp | Native timestamp type | Converts automatically |

**✅ CORRECT: Let EF Core Handle the Type**

```csharp
// GOOD: No explicit type - EF Core chooses the right type for each provider
CreatedAt = table.Column<DateTime>(nullable: false)
LastLoginAt = table.Column<DateTime>(nullable: true)
UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
```

When you **don't** specify `type:`, EF Core automatically uses:
- **SQLite**: TEXT with ISO8601 format
- **Postgres**: timestamp

This is the **recommended approach** - let EF Core handle it automatically.

**❌ PROBLEM: Explicitly Specifying DateTime Type**

```csharp
// BAD: Migration generated with SQLite active
CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
// This creates TEXT column in Postgres! Should be timestamp!

// BAD: Migration generated with Postgres active
CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
// This fails in SQLite! SQLite doesn't understand "timestamp"
```

**When Does This Become a Problem?**

If you generate migrations and EF Core scaffolds with explicit type specifications (this can happen with certain EF Core versions or configurations), you'll see errors like:

**SQLite Error:**
```
Microsoft.Data.Sqlite.SqliteException: SQLite Error 1: 'near "timestamp": syntax error'
```

**Postgres Error:**
```
Npgsql.PostgresException: 42804: column "CreatedAt" is of type timestamp but expression is of type text
```

**Solution: Remove Explicit Type or Use Provider Detection**

**Option 1: Remove the type parameter (Recommended)**
```csharp
// Simply don't specify type - EF Core handles it
CreatedAt = table.Column<DateTime>(nullable: false)
```

**Option 2: Use provider detection (if needed for other reasons)**
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";
    var dateTimeType = isSqlite ? "TEXT" : "timestamp";

    migrationBuilder.CreateTable(
        name: "Users",
        columns: table => new
        {
            CreatedAt = table.Column<DateTime>(
                type: dateTimeType,  // Only if you need explicit control
                nullable: false)
        });
}
```

**Real-World Example from RewardStar Project:**

```csharp
// ✅ This migration works correctly on both SQLite and Postgres
public partial class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";
        var boolType = isSqlite ? "INTEGER" : "boolean";

        migrationBuilder.CreateTable(
            name: "User",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(maxLength: 200, nullable: false),

                // ✅ Booleans: MUST use provider detection
                Active = table.Column<bool>(type: boolType, nullable: false),

                // ✅ DateTime: No type specified - works automatically!
                CreatedAt = table.Column<DateTime>(nullable: false),
                LastLoginAt = table.Column<DateTime>(nullable: true)
            });
    }
}
```

**What Happens in Each Database:**

**SQLite (when `DATABASE_URL` is NOT set):**
```sql
CREATE TABLE "User" (
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Active" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,           -- Stored as ISO8601 string
    "LastLoginAt" TEXT                   -- "2026-01-15 10:30:00"
);
```

**Postgres (when `DATABASE_URL` is set):**
```sql
CREATE TABLE "User" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Active" boolean NOT NULL,
    "CreatedAt" timestamp NOT NULL,      -- Native timestamp type
    "LastLoginAt" timestamp
);
```

**Key Takeaway:**
- **Booleans**: Always need explicit type detection (`type: boolType`)
- **DateTime**: Works automatically if you DON'T specify `type:`
- **Both**: EF Core handles the conversion seamlessly in your C# code

**Troubleshooting DateTime Issues:**

**Problem: "SQLite Error: near 'timestamp': syntax error"**
```csharp
// Your migration has:
CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)

// Fix: Remove the type parameter
CreatedAt = table.Column<DateTime>(nullable: false)
```

**Problem: "Postgres stores DateTime as TEXT instead of timestamp"**
```csharp
// Your migration has:
CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)

// Fix: Remove the type parameter or use provider detection
CreatedAt = table.Column<DateTime>(nullable: false)
```

**⚠️ CRITICAL: PostgreSQL DateTime UTC Requirement**

PostgreSQL's `timestamp with time zone` column type requires DateTime values to have `Kind=UTC`. When using NoTracking queries (common for performance), DateTime values can lose their UTC kind and become `Kind=Unspecified`, causing save failures.

**Error You'll See:**
```
Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes.
  ---> System.ArgumentException: Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported.
```

**Why This Happens:**
1. DbContext configured with `UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)` for performance
2. Entities retrieved from database have DateTime values with `Kind=Unspecified`
3. When updating these entities, PostgreSQL rejects non-UTC DateTimes

**Required Solution: Automatic UTC Conversion in DbContext**

To handle this automatically across your entire application, configure the DbContext to convert all DateTime values to UTC:

```csharp
public class RewardStartDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure DateTime handling for PostgreSQL
        if (Provider == DatabaseProvider.PostgreSQL)
        {
            // Convert all DateTime properties to UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(
                            new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                                v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                            )
                        );
                    }
                }
            }
        }
    }

    public override int SaveChanges()
    {
        ConvertDateTimesToUtc();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDateTimesToUtc();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDateTimesToUtc()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            foreach (var property in entry.Properties)
            {
                if (property.CurrentValue is DateTime dateTime)
                {
                    if (dateTime.Kind == DateTimeKind.Unspecified)
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    else if (dateTime.Kind == DateTimeKind.Local)
                        property.CurrentValue = dateTime.ToUniversalTime();
                }
            }
        }
    }
}
```

**What This Solution Does:**

1. **Value Converter (OnModelCreating)**: Automatically marks DateTimes as UTC when reading from database
   - Converts `Unspecified` to UTC by marking with `DateTime.SpecifyKind`
   - Converts `Local` to UTC with `ToUniversalTime()`
   - Only applies to PostgreSQL provider

2. **SaveChanges Override**: Ensures all DateTime values are UTC before saving
   - Catches any DateTime values that weren't handled by the converter
   - Works with both tracked and explicitly marked entities
   - Handles `Unspecified` and `Local` DateTimeKind

3. **Universal Application**: Works automatically for:
   - All entities across your application
   - Both nullable and non-nullable DateTime properties
   - Entities retrieved with NoTracking queries
   - New entities being inserted
   - Existing entities being updated

**When to Apply This Pattern:**

✅ **REQUIRED when:**
- Using PostgreSQL in production
- DbContext configured with `UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)`
- Updating entities retrieved from the database
- Using `FindAsync()` or any query that returns entities for modification

✅ **RECOMMENDED always for PostgreSQL projects** as defensive programming

**Alternative: Explicit Entity State Marking (Not Recommended)**

Without the automatic UTC conversion, you'd need to manually mark entities as modified and risk the UTC error:

```csharp
// Without UTC conversion - RISKY with NoTracking
var user = await _context.Users.FindAsync(id);
user.Active = true;
_context.Entry(user).State = EntityState.Modified;  // May fail with UTC error
await _context.SaveChangesAsync();

// With UTC conversion - SAFE
var user = await _context.Users.FindAsync(id);
user.Active = true;
_context.Entry(user).State = EntityState.Modified;  // Works reliably
await _context.SaveChangesAsync();  // DateTimes automatically converted to UTC
```

**Best Practices:**
- ✅ Always implement automatic UTC conversion when using PostgreSQL
- ✅ Store all dates as `DateTime` in C#, let the converter handle UTC
- ✅ Use `DateTime.UtcNow` instead of `DateTime.Now` in your application code
- ✅ Test updates with NoTracking queries to verify the solution works
- ❌ Don't manually convert DateTimes before saving (the DbContext handles it)
- ❌ Don't use `DateTimeOffset` unless you need timezone-specific data (adds complexity)

**Testing Both Providers:**

To test with different providers, update appsettings.json or use environment-specific configuration files:

```json
// appsettings.json (development with SQLite)
{
  "DatabaseProvider": "SQLite",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  }
}

// appsettings.Production.json (production with Postgres)
{
  "DatabaseProvider": "Postgres",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=rewardstar;Username=postgres;Password=yourpassword"
  }
}
```

The enum parser will handle the string value from configuration at runtime.

**SQLite for Integration Tests**

Configure integration tests to use in-memory SQLite for fast, isolated testing:

**1. Test DbContext Setup:**
```csharp
public class TestDbContextFactory
{
    public static AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")  // In-memory database
            .Options;

        var context = new AppDbContext(options);

        // IMPORTANT: Must open connection before using in-memory DB
        context.Database.OpenConnection();

        // Apply all migrations
        context.Database.EnsureCreated();

        return context;
    }
}
```

**2. xUnit Integration Test Example:**
```csharp
public class UserServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _context = TestDbContextFactory.CreateInMemoryContext();
        _userService = new UserService(_context);
    }

    [Fact]
    public async Task CreateUser_ShouldSaveToDatabase()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };

        // Act
        await _userService.CreateUserAsync(user);

        // Assert
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser);
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();  // Close connection to clear in-memory DB
        _context.Dispose();
    }
}
```

**3. Shared Test Context (for multiple tests):**
```csharp
public class DatabaseFixture : IDisposable
{
    public AppDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        Context = TestDbContextFactory.CreateInMemoryContext();
        SeedTestData();
    }

    private void SeedTestData()
    {
        // Add common test data
        Context.Users.AddRange(
            new User { Email = "user1@test.com" },
            new User { Email = "user2@test.com" }
        );
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Database.CloseConnection();
        Context.Dispose();
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

[Collection("Database collection")]
public class UserTests
{
    private readonly AppDbContext _context;

    public UserTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
    }

    [Fact]
    public async Task GetUsers_ShouldReturnSeededUsers()
    {
        var users = await _context.Users.ToListAsync();
        Assert.Equal(2, users.Count);
    }
}
```

**4. Best Practices for Integration Tests:**

✅ **DO:**
- Use in-memory SQLite for unit/integration tests (`:memory:` connection string)
- Call `Database.OpenConnection()` before using in-memory database
- Call `Database.EnsureCreated()` to apply schema
- Call `Database.CloseConnection()` in cleanup/dispose
- Test critical paths with both SQLite AND Postgres (run tests against both occasionally)
- Use fresh database instance per test for isolation

❌ **DON'T:**
- Forget to open connection for in-memory databases (tests will fail silently)
- Use file-based SQLite in tests (slower, leaves files behind)
- Skip testing on Postgres entirely (SQLite has limitations)
- Share database instances between unrelated tests without proper cleanup
- Rely on Postgres-specific features that SQLite doesn't support

**5. Testing on Both Databases:**

While SQLite is great for speed, periodically run tests against Postgres to catch:
- SQLite-specific behavior differences
- Case sensitivity issues
- Performance problems that only appear with real data volumes
- Features that work in SQLite but fail in Postgres

**Example: Using enum for tests against multiple providers:**

```csharp
// Test that runs against both providers
public class CrossDatabaseUserTests
{
    [Theory]
    [InlineData(DatabaseProvider.SQLite)]
    [InlineData(DatabaseProvider.Postgres)]
    public async Task CreateUser_ShouldWork_OnBothProviders(DatabaseProvider provider)
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContext(provider);
        var user = new User { Email = "test@example.com" };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser);

        // Cleanup for Postgres
        if (provider == DatabaseProvider.Postgres)
        {
            context.Database.EnsureDeleted();
        }
    }
}
```

**Configurable test database provider using enum:**

```csharp
public static class TestDbContextFactory
{
    public static AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }

    public static AppDbContext CreateContext(DatabaseProvider provider = DatabaseProvider.SQLite)
    {
        switch (provider)
        {
            case DatabaseProvider.Postgres:
                // Use testcontainers or Docker Postgres for integration tests
                var postgresOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql("Host=localhost;Database=test_db;Username=test;Password=test")
                    .Options;

                var postgresContext = new AppDbContext(postgresOptions);
                postgresContext.Database.EnsureDeleted();
                postgresContext.Database.EnsureCreated();
                return postgresContext;

            case DatabaseProvider.SQLite:
            default:
                return CreateInMemoryContext();
        }
    }

    // Alternative: Read provider from environment variable
    public static AppDbContext CreateContextFromEnvironment()
    {
        var providerString = Environment.GetEnvironmentVariable("TEST_DATABASE_PROVIDER") ?? "SQLite";
        var provider = Enum.Parse<DatabaseProvider>(providerString, ignoreCase: true);
        return CreateContext(provider);
    }
}
```

**6. CI/CD Configuration:**

In CI pipelines, SQLite tests run without any setup:
```yaml
# .github/workflows/test.yml
- name: Run Tests
  run: dotnet test
  # No database setup needed - SQLite in-memory works out of the box!
```

For occasional Postgres integration tests:
```yaml
- name: Setup Postgres
  uses: ikalnytskyi/action-setup-postgres@v4

- name: Run Postgres Integration Tests
  run: dotnet test --filter Category=PostgresIntegration
  env:
    TEST_DATABASE_PROVIDER: Postgres
```

**Best Practices for Multi-Database Support:**

1. ✅ **CRITICAL**: Use runtime provider detection for ALL boolean columns in migrations
   ```csharp
   var boolType = isSqlite ? "INTEGER" : "boolean";
   Active = table.Column<bool>(type: boolType, nullable: false)
   ```
2. ✅ **CRITICAL**: Implement automatic UTC conversion for PostgreSQL DateTime handling
   - Override `SaveChanges()` and `SaveChangesAsync()` to convert DateTimes to UTC
   - Add value converters in `OnModelCreating()` for PostgreSQL provider
   - Required when using `NoTracking` queries with PostgreSQL
   - See "PostgreSQL DateTime UTC Requirement" section for full implementation
3. ✅ **IMPORTANT**: Do NOT specify type for DateTime columns - let EF Core handle it automatically
   ```csharp
   // GOOD: No type specified - works on both SQLite (TEXT) and Postgres (timestamp)
   CreatedAt = table.Column<DateTime>(nullable: false)

   // BAD: Explicit type breaks cross-database compatibility
   CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)  // Fails on Postgres!
   ```
4. ✅ Add both `Sqlite:Autoincrement` and `Npgsql:ValueGenerationStrategy` annotations to identity columns
5. ✅ Use `DatabaseProvider` enum instead of strings for type safety
6. ✅ Always specify `HasMaxLength()` for strings
7. ✅ Use `HasPrecision()` for decimals
8. ✅ Test migrations on BOTH databases before committing
9. ✅ Use `CURRENT_TIMESTAMP` for date defaults (works on both)
10. ✅ Stick to common data types (int, string, DateTime, decimal, bool, Guid)
11. ✅ Review and edit generated migrations - don't trust them blindly
12. ✅ Use `DateTime.UtcNow` instead of `DateTime.Now` in application code
13. ❌ Avoid database-specific functions in default values
14. ❌ Avoid database-specific column types (jsonb, arrays, etc.)
15. ❌ Avoid computed columns in migrations (database-specific syntax)

### 4. Query Optimization

**Avoid N+1 Queries:**
- Use `.Include()` for eager loading related entities
- Use `.ThenInclude()` for nested relationships
- Consider split queries for multiple collections: `.AsSplitQuery()`

**Efficient Querying:**
- Use `.AsNoTracking()` for read-only queries (better performance)
- Project to DTOs with `.Select()` instead of loading full entities
- Filter with `.Where()` before loading data
- Use pagination with `.Skip()` and `.Take()`

**Examples:**
```csharp
// BAD: N+1 problem
var users = await context.Users.ToListAsync();
foreach (var user in users)
    var activities = user.Activities; // Separate query per user

// GOOD: Eager loading
var users = await context.Users
    .Include(u => u.Activities)
    .ToListAsync();

// BETTER: Project to DTO for read-only
var users = await context.Users
    .AsNoTracking()
    .Select(u => new UserDto
    {
        Id = u.Id,
        Name = u.Name,
        ActivityCount = u.Activities.Count
    })
    .ToListAsync();
```

**Tracking vs No-Tracking:**
- Use `.AsNoTracking()` for queries where entities won't be modified
- Use tracking (default) when updating entities
- Use `.AsNoTrackingWithIdentityResolution()` for read-only queries with relationships

### 5. Best Practices & Common Pitfalls

**Entity Design:**
- Use `int` or `Guid` for primary keys
- Include audit fields: `CreatedAt`, `UpdatedAt`, `IsActive/IsDeleted`
- Navigation properties should be virtual for lazy loading (if enabled)
- Avoid circular references in entity navigation properties

**SaveChanges:**
- Call `SaveChangesAsync()` to persist changes
- EF Core tracks changes automatically for loaded entities
- Batch multiple changes before calling `SaveChangesAsync()` for efficiency
- Wrap in transactions for atomic operations: `context.Database.BeginTransaction()`

**Common Mistakes:**
- ❌ Forgetting `.ToListAsync()` or `.FirstOrDefaultAsync()` (query won't execute)
- ❌ Using blocking calls (`.ToList()`, `.FirstOrDefault()`) instead of async
- ❌ Not disposing DbContext (use DI for automatic disposal)
- ❌ Loading too much data - always filter and project
- ❌ Modifying entities outside of tracking context

**Async/Await:**
- Always use async methods: `ToListAsync()`, `FirstOrDefaultAsync()`, `SaveChangesAsync()`
- Avoid mixing sync and async code
- Never use `.Result` or `.Wait()` - causes deadlocks

**Performance Tips:**
- Enable query logging in development to see generated SQL
- Use compiled queries for frequently executed queries
- Consider read replicas for read-heavy workloads
- Batch updates/inserts when processing large datasets

## Implementation Workflow

When working with EF Core:

1. **Planning**: Identify entities, relationships, and required configurations
2. **Entity Setup**: Create entity classes with proper properties and navigation properties
3. **Configuration**: Create `IEntityTypeConfiguration<T>` classes for each entity
4. **DbContext**: Register configurations in `OnModelCreating()`
5. **Migration**: Generate and review migration with `Add-Migration`
6. **Apply**: Update database with `Update-Database`
7. **Query**: Write efficient queries with appropriate loading strategies
8. **Test**: Verify relationships and query performance

## Debugging & Troubleshooting

**Enable Query Logging:**
```csharp
// In Program.cs or Startup.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging() // Development only
           .LogTo(Console.WriteLine, LogLevel.Information));
```

**Common Issues:**
- Migration conflicts: Remove migration with `Remove-Migration`, fix model, regenerate
- Foreign key violations: Check `OnDelete` behavior and relationship configuration
- Slow queries: Enable logging, examine generated SQL, add indexes or optimize query
- Tracking issues: Use `.AsNoTracking()` or detach entities manually
- **"null value in column 'Id' violates not-null constraint" on PostgreSQL**: Migration is missing `Npgsql:ValueGenerationStrategy` annotation on the Id column. Add `.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)` alongside `Sqlite:Autoincrement`. See "Auto-Increment Identity Columns" section above.
- **"Reading as 'System.DateTime' is not supported for fields having DataTypeName 'text'" on PostgreSQL**: DateTime column in migration has no explicit `type:` parameter, so it defaults to `text` on PostgreSQL (SQLite scaffolds DateTime as TEXT). Fix: add `type: dateTimeType` where `var dateTimeType = isSqlite ? "TEXT" : "timestamp without time zone";`. All DateTime columns must use the `dateTimeType` variable, same as boolean columns use `boolType`.

## File Organization

Recommended structure:
```
Project/
├── Models/           # Entity classes
├── Configurations/   # IEntityTypeConfiguration classes
├── Data/            # DbContext and migrations
│   ├── AppDbContext.cs
│   └── Migrations/
└── DTOs/            # Data transfer objects for queries
```
