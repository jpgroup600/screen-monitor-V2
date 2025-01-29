using ScreenshotMonitor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ScreenshotMonitor.Data.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<string?> RegisterAdmin(User admin);
        public Task<string?> RegisterEmployee(User employee);
        public Task<string?> LoginAdmin(string email, string password);
        public Task<string?> LoginEmployee(string email, string password);
        Task<bool> ForgotPassword(string dtoEmail, string dtoNewPassword, string dtoPhoneNumber);


    }
}
