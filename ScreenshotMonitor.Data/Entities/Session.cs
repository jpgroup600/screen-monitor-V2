using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenshotMonitor.Data.Entities
{
    public class Session
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }

        public string ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable for active sessions

        public TimeSpan ActiveDuration { get; set; } // Total active time in this session

        public List<string> ForegroundApps { get; set; } = new();
        public List<string> BackgroundApps { get; set; } = new();

        // Relations
        public ICollection<Screenshot> Screenshots { get; set; } = new List<Screenshot>();
    }
}
