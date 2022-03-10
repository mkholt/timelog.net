using Moq;
using timelog.net.Controllers;
using timelog.net.Data;
using timelog.net.Models;

namespace UnitTest
{
    internal static class Controllers
    {
        public static TaskController Task(IMock<IRepository<ProjectTask>>? taskRepo = null)
        {
            taskRepo ??= Repository.Task();

            return new TaskController(taskRepo.Object);
        }

        public static EntryController Entry(Mock<IEntryRepository>? repo = null, Mock<IRepository<ProjectTask>>? taskRepo = null)
        {
            repo ??= Repository.Entry();
            taskRepo ??= new();

            return new EntryController(repo.Object, taskRepo.Object);
        }
    }
}
