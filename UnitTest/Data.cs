using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timelog.net.Models;

namespace UnitTest
{
    internal static class Data
    {
        public static Project Project(bool setId = true)
        {
            var p = new Project
            {
                Title = "Project Title",
                Url = "https://github.com/mkholt/timelog.net",
                ExternalId = "1234",
                Tasks = new List<ProjectTask>()
            };

            if (setId) p.ProjectId = 0;

            return p;
        }

        public static ProjectTask Task(int? taskId = null, bool setId = true)
        {
            var p = new ProjectTask
            {
                ProjectId = 0,
                Project = new Project
                {
                    ProjectId = 0,
                    Title = "Project Title",
                },
                Title = "Task Title",
                Entries = new List<Entry>(),
                Icon = "Icon name",
                Url = "Url",
                ExternalId = "External ID",
            };

            if (taskId != null || setId) p.TaskId = taskId ?? 0;

            return p;
        }

        public static Entry Entry(ProjectTask task, int? entryId = null) =>
            new()
            {
                EntryId = entryId ?? 1,
                Task = task,
                TaskId = task.TaskId,
                Note = "Initial entry note",
                StartTime = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)),
                EndTime = DateTime.UtcNow,
            };
    }
}
