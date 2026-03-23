using Microsoft.EntityFrameworkCore;
using RunningTracker.API.Data;
using RunningTracker.API.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Railway injects PORT — fall back to 8080 for local Docker, 5000 for dotnet run
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// SQLite — use /data volume on Railway, local file otherwise
var dbPath = Environment.GetEnvironmentVariable("DB_PATH") ?? "running_tracker.db";
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}"));

// Allow any localhost origin in development
builder.Services.AddCors(opt =>
    opt.AddPolicy("DevFrontend", policy =>
        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

// Auto-run migrations and seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Only enforce auth when credentials are configured (skips in local dev if vars aren't set)
if (Environment.GetEnvironmentVariable("AUTH_USER") is not null &&
    Environment.GetEnvironmentVariable("AUTH_PASSWORD") is not null)
    app.UseMiddleware<BasicAuthMiddleware>();

app.UseCors("DevFrontend");
app.MapGet("/health", () => Results.Ok("healthy"));
app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();

// Serve React SPA static files (wwwroot is populated by the Dockerfile)
app.UseDefaultFiles();
app.UseStaticFiles();
// Fallback: any non-API route returns index.html so React Router works
app.MapFallbackToFile("index.html");

app.Run();
