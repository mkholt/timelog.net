using System.Collections.Generic;
using System.Linq;

namespace timelog.net.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        public string? Title { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}