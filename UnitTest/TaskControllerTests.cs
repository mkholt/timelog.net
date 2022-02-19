using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using timelog.net.Controllers;
using timelog.net.Data;
using timelog.net.Models;
using Xunit;

namespace UnitTest;

public class TaskControllerTests
{
    [Fact]
    public async Task CanGetEmptyProjectTaskList()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetAll())
            .ReturnsAsync(Enumerable.Empty<ProjectTask>());

        var ctrl = new TaskController(repo.Object);

        var projects = await ctrl.GetAll();
        
        repo.Verify(r => r.GetAll());
        var actionResult = Assert.IsType<ActionResult<List<ProjectTask>>>(projects);
        var projectList = Assert.IsType<List<ProjectTask>>(actionResult.Value);
        Assert.Empty(projectList);
    }

    [Fact]
    public async Task CanGetProjectTask()
    {
        var projectIn = CreateProjectTask();
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(projectIn);
        
        var ctrl = new TaskController(repo.Object);

        var result = await ctrl.Get(0);
        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(result);
        var projectOut = Assert.IsType<ProjectTask>(actionResult.Value);
        Assert.Equal(projectIn, projectOut);
    }

    [Fact]
    public async Task GetProjectTaskReturns404OnMissingProjectTask()
    {
        var taskIn = CreateProjectTask();
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetById(taskIn.TaskId))
            .ReturnsAsync(taskIn);
        
        var ctrl = new TaskController(repo.Object);

        const int nonExistentId = 999;
        var result = await ctrl.Get(nonExistentId);
        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal(nonExistentId, notFoundResult.Value);
    }

    [Fact]
    public async Task GetProjectTaskReturns404OnNoProjectTasks()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync((ProjectTask?) null);
        
        var ctrl = new TaskController(repo.Object);

        const int nonExistentId = 999;
        var returnResult = await ctrl.Get(nonExistentId);
        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(returnResult);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal(nonExistentId, notFoundResult.Value);
    }

    [Fact]
    public async Task CanCreateProjectTask()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.Add(It.IsAny<ProjectTask>()))
            .ReturnsAsync((ProjectTask p) => new ProjectTask
            {
                TaskId = 1,
                Title = p.Title,
                Entries = new List<Entry>(p.Entries),
                Icon = p.Icon,
                ProjectId = p.ProjectId,
                ExternalId = p.ExternalId,
                Url = p.Url
            });

        var ctrl = new TaskController(repo.Object);

        var taskIn = CreateProjectTask(false);
        taskIn.Project = null;
        var returnResult = await ctrl.Create(taskIn);

        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(returnResult);
        var projectOut = Assert.IsType<ProjectTask>(actionResult.Value);

        var expOut = CreateProjectTask();
        expOut.TaskId = 1;
        expOut.Project = null;
        var expOutStr = JsonConvert.SerializeObject(expOut);
        var actOutStr = JsonConvert.SerializeObject(projectOut);
        Assert.Equal(expOutStr, actOutStr);
    }

    [Fact]
    public async Task CanUpdateProjectTask()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<ProjectTask>()))
            .ReturnsAsync(true);
        
        var ctrl = new TaskController(repo.Object);

        var projectIn = CreateProjectTask();
        var returnResult = await ctrl.Update(projectIn.TaskId, projectIn);
        Assert.IsType<OkResult>(returnResult);
        repo.Verify(r => r.Update(projectIn.TaskId, projectIn), Times.Once);
    }
    
    [Fact]
    public async Task UpdateReturns404OnFail()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<ProjectTask>()))
            .ReturnsAsync(false);
        
        var ctrl = new TaskController(repo.Object);

        var projectIn = CreateProjectTask();
        var returnResult = await ctrl.Update(projectIn.TaskId, projectIn);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
        Assert.Equal(projectIn.TaskId, notFoundObjectResult.Value);
        repo.Verify(r => r.Update(projectIn.TaskId, projectIn), Times.Once);
    }
    
    [Fact]
    public async Task CanDeleteProjectTask()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.Remove(It.IsAny<int>()))
            .ReturnsAsync(true);
        
        var ctrl = new TaskController(repo.Object);

        var projectIn = CreateProjectTask();
        var returnResult = await ctrl.Delete(projectIn.TaskId);
        Assert.IsType<OkResult>(returnResult);
        repo.Verify(r => r.Remove(projectIn.TaskId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteReturns404OnFail()
    {
        var repo = new Mock<IRepository<ProjectTask>>();
        repo.Setup(r => r.Remove(It.IsAny<int>()))
            .ReturnsAsync(false);
        
        var ctrl = new TaskController(repo.Object);

        var projectIn = CreateProjectTask();
        var returnResult = await ctrl.Delete(projectIn.TaskId);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
        Assert.Equal(projectIn.TaskId, notFoundObjectResult.Value);
        repo.Verify(r => r.Remove(projectIn.TaskId), Times.Once);
    }
    
    private static ProjectTask CreateProjectTask(bool setId = true)
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

        if (setId) p.TaskId = 0;
        
        return p;
    }
}