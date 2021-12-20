using System;

namespace timelog.net.Models
{
    public class Entry
    {
        public Entry()
        {
            
        }

        public Entry(Entry fromEntry)
        {
            EntryId = fromEntry.EntryId;
            TaskId = fromEntry.TaskId;
            StartTime = fromEntry.StartTime;
            EndTime = fromEntry.EndTime;
        }
        
        public int EntryId { get; set; }
        public int? TaskId { get; set; }
        
        public Task? Task { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}