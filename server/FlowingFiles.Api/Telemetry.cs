using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FlowingFiles.Api;

internal static class Telemetry
{
    private const string SERVICE_NAME = "flowingfiles-api";

    public static WebApplicationBuilder AddApiTelemetry(this WebApplicationBuilder builder)
    {
        var otel = builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(SERVICE_NAME, serviceVersion: typeof(Telemetry).Assembly.GetName().Version?.ToString())
                .AddAttributes([new("deployment.environment", builder.Environment.EnvironmentName)]))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation(options => options.Filter = IsInteresting)
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            // ParseStateValues/IncludeFormattedMessage default to false: without them the OTLP log
            // exporter sends the raw message template ("Similarity classification for {FileName}...")
            // instead of the value substituted into it, which is all a viewer like the Aspire
            // Dashboard can render.
            .WithLogging(configureBuilder: null, configureOptions: (OpenTelemetryLoggerOptions options) =>
            {
                options.ParseStateValues = true;
                options.IncludeFormattedMessage = true;
            });

        // Without a configured endpoint there is no exporter: avoids the endless OTLP retry
        // when the dashboard is not running.
        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")))
            otel.UseOtlpExporter();

        return builder;
    }

    // /swagger* and favicon pollute the trace without telling anything.
    private static bool IsInteresting(HttpContext context) =>
        !context.Request.Path.StartsWithSegments("/swagger")
        && !context.Request.Path.StartsWithSegments("/favicon.ico");
}
