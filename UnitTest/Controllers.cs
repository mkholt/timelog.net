using Moq;
using timelog.net.Controllers;
using timelog.net.Data;
using timelog.net.Models;

namespace UnitTest
{
    internal static class Controllers
    {
        public static TaskController Task(Mock<IRepository<ProjectTask>>? taskRepo = null)
        {
            taskRepo ??= new();

            return new TaskController(taskRepo.Object);
        }

        public static EntryController Entry(Mock<IRepository<Entry>>? repo = null, Mock<IRepository<ProjectTask>>? taskRepo = null)
        {
            repo ??= new();
            taskRepo ??= new();

            return new EntryController(repo.Object, taskRepo.Object);
        }
    }
}
