using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MyGamingList.Api.HealthChecks;
using MyGamingList.Application;
using MyGamingList.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

try {
    Log.Information("Starting API");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped(typeof(CancellationToken),
    implementationFactory: serviceProvider => {
        IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        return httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
    });

    builder.Services.AddCors(options => {
        options.AddPolicy("AllowAll",
            configurePolicy: policyBuilder => {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
            });
    });
    
    builder.Services.AddHealthChecks()
        .AddCheck<RandomHeathCheck>("Random", tags: ["random"])
        .AddCheck<HealthyHeathCheck>("Healthy", tags: ["healthy"])
        .AddCheck<DegradedHeathCheck>("Degraded", tags: ["degraded"])
        .AddCheck<UnhealthyHeathCheck>("Unhealthy", tags: ["unhealthy"]);

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment()) {
        app.MapOpenApi();
    }
    
    app.MapScalarApiReference(options => {
        options.Title = "My Gaming List Api";
        options.Theme = ScalarTheme.Saturn;
        options.Layout = ScalarLayout.Modern;
        options.HideClientButton = true;
    });
    app.UseCors("AllowAll");
    
    app.MapHealthChecks("/health");
    app.MapHealthChecks("/health/ui", new HealthCheckOptions {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/ui/healthy", new HealthCheckOptions {
        Predicate = hcr => hcr.Tags.Contains("healthy"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/ui/degraded", new HealthCheckOptions {
        Predicate = hcr => hcr.Tags.Contains("degraded"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/ui/unhealthy", new HealthCheckOptions {
        Predicate = hcr => hcr.Tags.Contains("unhealthy"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/ui/random", new HealthCheckOptions {
        Predicate = hcr => hcr.Tags.Contains("random"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
} catch (Exception ex) {
    Log.Fatal(ex, "API end point failed");
} finally {
    Log.CloseAndFlush();
}
