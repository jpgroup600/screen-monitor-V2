using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScreenshotMonitor.Data.Context;
using ScreenshotMonitor.Data.Repositories;
using System.Text;
using ScreenshotMonitor.Data.Repositories.Interfaces;
using ScreenshotMonitor.Data.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//});
// Register DbContext
var config = builder.Configuration;
// Add Authentication service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // The Authority is the URL of your Identity Server or the authentication server
        options.Authority = "https://localhost:7037"; // Change this to your actual authority URL (e.g., IdentityServer URL)

        // The Audience is the identifier of your app or the API
        options.Audience = "ScreenshotMonitor.Client"; // Change to your API's audience value

        // Optionally, save the token in the authentication properties
        options.SaveToken = true;

        // Configure Token Validation Parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Set to true if you want to validate the issuer
            ValidateAudience = false, // Set to true if you want to validate the audience
            ValidateLifetime = true, // Ensure the token hasn't expired
            ValidateIssuerSigningKey = true, // Ensure the signing key is valid
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value!)), // Get the secret key from appsettings.json
            ClockSkew = TimeSpan.Zero // Adjust for token expiration time tolerance (e.g., 5 minutes)
        };
    });

builder.Services.AddDbContext<SmDbContext>(options => options.UseNpgsql(config.GetConnectionString("SmDb")!));
builder.Services.AddScoped<DbContext, SmDbContext>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline (authentication & authorization middleware).
app.UseAuthentication();  // Add Authentication middleware
app.UseAuthorization();   // Add Authorization middleware

// Ensure the database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmDbContext>();

    try
    {
        Console.WriteLine("Checking database connection...");
        dbContext.Database.CanConnect(); // Checks if the database is reachable
        Console.WriteLine("Database connection is successful.");

        // Ensure database is created if it doesn’t exist
        dbContext.Database.EnsureCreated();

        // Apply any pending migrations
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
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
