using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenshotMonitor.Data.Entities
{
    public class ProjectEmployee
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public string EmployeeId { get; set; }
        public User Employee { get; set; }

        public TimeSpan TotalActiveTime { get; set; } // Total active time in the project
    }
}
