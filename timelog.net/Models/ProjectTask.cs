using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace timelog.net.Models
{
    public class ProjectTask
    {
        public int TaskId { get; set; }
        
        public string? Title { get; set; }
        
        public int? ProjectId { get; set; }
        
        public string? ExternalId { get; set; }
        
        public string? Url { get; set; }
        
        public string? Icon { get; set; }

        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
        
        [JsonIgnore]
        public Project? Project { get; set; }
    }
}