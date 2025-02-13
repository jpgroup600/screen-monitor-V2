using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Context;
using ScreenshotMonitor.Data.Entities;
using ScreenshotMonitor.Data.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreenshotMonitor.Data.Repositories
{

    public class AdminRepository(
        IConfiguration conf,
        SmDbContext dbContext,
        ILogger<AdminRepository> logger
    ) : IAdminRepository
    {
        // Fetch all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching all users");
                throw;
            }
        }

        // Fetch only Admins
        public async Task<List<User>> GetAdminsOnlyAsync()
        {
            try
            {
                return await dbContext.Users
                    .Where(u => u.Role == "Admin")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching admins");
                throw;
            }
        }

        // Fetch only Employees
        public async Task<List<User>> GetEmployeesOnlyAsync()
        {
            try
            {
                return await dbContext.Users
                    .Where(u => u.Role == "Employee")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching employees");
                throw;
            }
        }
        public async Task<List<User>> UpdateTotalOnlineTimeAndGetAllEmployeesAsync()
        {
            try
            {
                // Get all employees with their sessions
                var employees = await dbContext.Users
                    .Include(u => u.Sessions)
                    .Where(u => u.Role == "Employee")
                    .ToListAsync();

                // Loop through each employee and calculate TotalOnlineTime
                foreach (var employee in employees)
                {
                    // Sum ActiveDuration of all sessions for this employee
                    var totalActiveDuration = employee.Sessions
                        .Where(s => s.ActiveDuration != null)
                        .Select(s => s.ActiveDuration)
                        .Aggregate(TimeSpan.Zero, (acc, duration) => acc.Add(duration));

                    // Update TotalOnlineTime
                    employee.TotalOnlineTime = totalActiveDuration;
                }

                // Save changes to DB
                await dbContext.SaveChangesAsync();

                // Return updated list of employee profiles
                return employees;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating TotalOnlineTime for all employees");
                throw;
            }
        }

        // Fetch a single user by ID
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            try
            {
                return await dbContext.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching user by ID");
                throw;
            }
        }

        // Delete a user (Admin or Employee)
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null)
                    return false;

                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting user");
                throw;
            }
        }
    }

}
