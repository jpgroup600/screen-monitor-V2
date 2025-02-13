using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreenshotMonitor.Data.Entities;

namespace ScreenshotMonitor.Data.Interfaces.Repositories
{
    
    public interface IAdminRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetAdminsOnlyAsync();
        Task<List<User>> GetEmployeesOnlyAsync();
        Task<User?> GetUserByIdAsync(string userId);
        Task<List<User>> UpdateTotalOnlineTimeAndGetAllEmployeesAsync();
        Task<bool> DeleteUserAsync(Guid userId);
    }

}
