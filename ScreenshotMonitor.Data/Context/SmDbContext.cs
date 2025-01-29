using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScreenshotMonitor.Data.Entities;
using System;
using System.IO;

namespace ScreenshotMonitor.Data.Context
{
    public class SmDbContext : DbContext
    {
        public SmDbContext(DbContextOptions<SmDbContext> options) : base(options) { }
        SmDbContext(IConfiguration conf)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environment = "Development";

                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Loads appsettings.Development.json in Development mode
                    .Build();

                var connectionString = config.GetConnectionString("SmDb");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is missing. Check your appsettings.Development.json file.");
                }

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
    }
}
