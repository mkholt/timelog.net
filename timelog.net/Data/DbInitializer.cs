using System;
using System.Linq;
using timelog.net.Models;

namespace timelog.net
{
    public static class DbInitializer
    {
        public static void Initialize(ProjectContext context)
        {
            if (context.Projects.Any())
            {
                return;
            }

            var projects = new Project[]
            {
                new() {Title = "Project 1"},
                new() {Title = "Project 2"}
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();

            var tasks = new ProjectTask[]
            {
                new() {Title = "Task 1", ProjectId = 1, Url = "https://dev.azure.com/bhgprod/BHIT/_workitems/edit/199672", ExternalId = "199672", Icon = "AzureLogo"},
                new() {Title = "Task 2", ProjectId = 1, Url = "https://dev.azure.com/bhgprod/BHIT/_workitems/edit/199672", ExternalId = "199672", Icon = "AzureLogo"},
                new() {Title = "Task 3", ProjectId = 1, Url = "https://dev.azure.com/bhgprod/BHIT/_workitems/edit/199672", ExternalId = "199672", Icon = "AzureLogo"},
                new() {Title = "Task 4", ProjectId = 2, Url = "https://dev.azure.com/bhgprod/BHIT/_workitems/edit/199672", ExternalId = "199672", Icon = "AzureLogo"},
                new() {Title = "Task 5", ProjectId = 2, Url = "https://dev.azure.com/bhgprod/BHIT/_workitems/edit/199672", ExternalId = "199672", Icon = "AzureLogo"}
            };
            
            context.Tasks.AddRange(tasks);
            context.SaveChanges();

            var entries = new Entry[]
            {
                new()
                {
                    TaskId = 1, StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.UtcNow.AddDays(-1).AddHours(2)
                },
                new()
                {
                    TaskId = 2, StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.UtcNow.AddDays(-1).AddHours(2)
                },
                new()
                {
                    TaskId = 3, StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.UtcNow.AddDays(-1).AddHours(2)
                },
                new()
                {
                    TaskId = 4, StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.UtcNow.AddDays(-1).AddHours(2)
                },
                new()
                {
                    TaskId = 5, StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.UtcNow.AddDays(-1).AddHours(2)
                },
            };
            context.Entries.AddRange(entries);
            context.SaveChanges();
        }
    }
}