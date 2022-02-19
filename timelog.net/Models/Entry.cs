using System;

namespace timelog.net.Models
{
    public class Entry
    {
        public Entry()
        {
            
        }
        
        public int EntryId { get; set; }
        
        public int? TaskId { get; set; }
        
        public ProjectTask? Task { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public string? Note { get; set; }
    }
}