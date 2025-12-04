using System.Diagnostics;

using Demo.Agent.Fundraising.Agent;
using Demo.Agent.Fundraising.Data;
using Demo.Agent.Fundraising.Models;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

/* Load Configuration */

if (Debugger.IsAttached)
{
    builder.Configuration.AddJsonFile(@"appsettings.debug.json", optional: true, reloadOnChange: true);
}

builder.Configuration.AddJsonFile($@"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($@"appsettings.{Environment.UserName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configure OpenTelemetry
var serviceName = "Demo.Agent.Fundraising";
var serviceVersion = "1.0.0";

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: serviceName,
            serviceVersion: serviceVersion))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource(serviceName);
        
        // Only export to console in non-development environments
        if (!builder.Environment.IsDevelopment())
        {
            tracing.AddConsoleExporter();
        }
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddMeter(serviceName);
        
        // Only export to console in non-development environments
        if (!builder.Environment.IsDevelopment())
        {
            metrics.AddConsoleExporter();
        }
    });

builder.Logging.AddOpenTelemetry(logging =>
{
    // Only export to console in non-development environments
    if (!builder.Environment.IsDevelopment())
    {
        logging.AddConsoleExporter();
    }
});

// Register AgentState as singleton to maintain data in memory
builder.Services.AddSingleton(sp =>
{
    var state = new AgentState();
    SampleData.InitializeSampleData(state);
    return state;
});

// Register Tools for agent function calls
builder.Services.AddSingleton<Tools>();

// Register FundraisingAgent as singleton
builder.Services.AddSingleton<FundraisingAgent>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(opt =>
{
    opt.SingleLine = false;
    opt.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    opt.IncludeScopes = true;
});
builder.Logging.AddDebug();

var app = builder.Build();

// Initialize the agent at startup
var logger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
    var agent = app.Services.GetRequiredService<FundraisingAgent>();
    await agent.InitializeAsync();
    logger.LogInformation("✅ Fundraising Agent initialized successfully");
}
catch (Exception ex)
{
    logger.LogWarning(ex, "⚠️ Failed to initialize Fundraising Agent. Running in demo mode without AI capabilities.");
}

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    service = "Fundraising Agent"
}))
.WithName("HealthCheck")
.WithTags("Health");

// Agent chat endpoint - single message
app.MapPost("/agent/chat", async (ChatRequest request, FundraisingAgent agent) =>
{
    try
    {
        var response = await agent.ProcessMessageAsync(request.Message);
        return Results.Ok(new ChatResponse
        {
            Message = response,
            Timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing chat message");
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Error processing message"
        );
    }
})
.WithName("AgentChat")
.WithTags("Agent")
.AddOpenApiOperationTransformer((operation, _, _) =>
{
    operation.Summary = "Send a message to the Fundraising Agent and receive a response.";
    operation.Description = "This endpoint allows you to interact with the Fundraising Agent by sending a message and receiving a response.";
    return Task.FromResult(operation);
})
;

await app.RunAsync();
