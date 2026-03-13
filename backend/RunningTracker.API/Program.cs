using Microsoft.EntityFrameworkCore;
using RunningTracker.API.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// SQLite via EF Core — DB file lives next to the binary
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=running_tracker.db"));

// Allow any localhost origin in development
builder.Services.AddCors(opt =>
    opt.AddPolicy("DevFrontend", policy =>
        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

// Auto-create/migrate DB on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseCors("DevFrontend");
app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();
app.Run();
