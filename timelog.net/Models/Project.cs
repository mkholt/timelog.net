using System.Collections.Generic;
using System.Linq;

namespace timelog.net.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        public string? Title { get; set; }
        
        public string? Url { get; set; }
        
        public string? ExternalId { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}