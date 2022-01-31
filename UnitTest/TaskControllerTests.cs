using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using timelog.net.Controllers;
using timelog.net.Data;
using timelog.net.Models;
using Xunit;

namespace UnitTest;

public class TaskControllerTests
{
    [Fact]
    public async Task CanGetTask()
    {
        var taskIn = new ProjectTask
        {
            TaskId = 0,
            ProjectId = 0,
            ExternalId = "ExternalId",
            Url = "Url",
            Title = "Title",
            Icon = "Icon",
            Entries = new List<Entry>()
        };
        
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(taskIn);
        
        var ctrl = new TaskController(repo.Object);

        var taskOut = await ctrl.GetTask(0);
        Assert.Equal(taskIn, taskOut);
    }

    [Fact]
    public async Task GetTasksReturnsNullOnMissingTask()
    {
        var taskIn = new ProjectTask
        {
            TaskId = 0,
            ProjectId = 0,
            ExternalId = "ExternalId",
            Url = "Url",
            Title = "Title",
            Icon = "Icon",
            Entries = new List<Entry>()
        };
        
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(taskIn);
        
        var ctrl = new TaskController(repo.Object);

        var projectOut = await ctrl.GetTask(1);
        Assert.Null(projectOut);
    }

    [Fact]
    public async Task GetTaskReturnsNullOnNoTasks()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync((ProjectTask?) null);
        
        var ctrl = new TaskController(repo.Object);

        var taskOut = await ctrl.GetTask(0);
        Assert.Null(taskOut);
    }
}