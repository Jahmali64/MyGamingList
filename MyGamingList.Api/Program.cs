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
            policyBuilder => {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
            });
    });

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment()) {
        app.MapOpenApi();
        app.MapScalarApiReference(options => {
            options.Title = "My Gaming List Api";
            options.Theme = ScalarTheme.Saturn;
            options.Layout = ScalarLayout.Modern;
            options.HideClientButton = true;
        });
        app.UseCors("AllowAll");
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
} catch (Exception ex) {
    Log.Fatal(ex, "API end point failed");
} finally {
    Log.CloseAndFlush();
}
