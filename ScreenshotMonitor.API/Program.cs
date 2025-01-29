using Microsoft.EntityFrameworkCore;
using ScreenshotMonitor.Data.Context;
var builder = WebApplication.CreateBuilder(args);

// Register DbContext
var config = builder.Configuration;
builder.Services.AddDbContext<SmDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("SmDb")!));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure the database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmDbContext>();

    // Ensure database is created if it doesn’t exist
    dbContext.Database.EnsureCreated();

    // Apply any pending migrations
    dbContext.Database.Migrate();
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
