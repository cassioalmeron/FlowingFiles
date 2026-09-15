using FlowingFiles.Api;
using FlowingFiles.Api.Middleware;
using FlowingFiles.Core;
using FlowingFiles.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Text.Json.Serialization;

// Walks up from the working directory until it finds the repository root file with the settings.
DotNetEnv.Env.TraversePath().Load();

var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
path = Path.Combine(path, "FlowingFiles", "Logs");

if (!Directory.Exists(path))
    Directory.CreateDirectory(path);

path = Path.Combine(path, "log-.log");

Log.Logger = new LoggerConfiguration()
    .Filter.ByExcluding("StartsWith(SourceContext, 'Microsoft.')")
    .WriteTo.File(
        path: path,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

Log.Information("Starting the application...");

var builder = WebApplication.CreateBuilder(args);

// The host logger forwards to the file logger above; writeToProviders keeps the ILogger output
// flowing to the other providers too - without it the OpenTelemetry log exporter never sees anything.
builder.Host.UseSerilog(
    (context, configuration) => configuration.WriteTo.Logger(Log.Logger),
    preserveStaticLogger: true,
    writeToProviders: true);

builder.AddApiTelemetry();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<FlowingFilesDbContext>();

// String-serialize enums (e.g. ClassificationMethod) instead of the numeric default, so the JSON
// contract stays "Rule"/"Similarity" — no frontend change needed for this DTO's Method field.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructuralServices();

// appsettings.json supplies the defaults, all four overridable from the environment — flat names
// read directly, matching how DatabaseSettings/GmailService read env vars in this project
// (Environment.GetEnvironmentVariable, not the Section__Key convention IConfiguration expects).
// appsettings.json itself is git-ignored (server/.gitignore), so Threshold/K must not depend on it
// alone: a server whose file predates these keys would silently bind K to 0, turning the
// similarity search's LIMIT into LIMIT 0 with no error.
builder.Services.Configure<OllamaSettings>(builder.Configuration.GetSection("Ollama"));
builder.Services.PostConfigure<OllamaSettings>(settings =>
{
    var baseUrl = Environment.GetEnvironmentVariable("OLLAMA_BASE_URL");
    if (!string.IsNullOrEmpty(baseUrl))
        settings.BaseUrl = baseUrl;

    var embeddingModel = Environment.GetEnvironmentVariable("OLLAMA_EMBEDDING_MODEL");
    if (!string.IsNullOrEmpty(embeddingModel))
        settings.EmbeddingModel = embeddingModel;

    var threshold = Environment.GetEnvironmentVariable("OLLAMA_THRESHOLD");
    if (double.TryParse(threshold, out var parsedThreshold))
        settings.Threshold = parsedThreshold;

    var k = Environment.GetEnvironmentVariable("OLLAMA_K");
    if (int.TryParse(k, out var parsedK))
        settings.K = parsedK;
});

// Registered after AddInfrastructuralServices() so this typed-client registration wins over that
// method's blanket AddScoped(EmbeddingService) — EF/DI resolves the last registration for a type.
builder.Services.AddHttpClient<EmbeddingService>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<OllamaSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRequestLogging();

app.UseCors("AllowAll");

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// Apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FlowingFilesDbContext>();
    dbContext.Database.Migrate();
    Log.Information("Database migration completed");
}

app.Run();
