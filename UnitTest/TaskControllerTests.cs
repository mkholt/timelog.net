using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using timelog.net.Models;
using Xunit;

namespace UnitTest;

public class TaskControllerTests
{
    [Fact]
    public async Task CanGetEmptyProjectTaskList()
    {
        var repo = Repository.Repo<ProjectTask, IEnumerable<ProjectTask>>((r => r.GetAll(), Enumerable.Empty<ProjectTask>()));
        var ctrl = Controllers.Task(repo);

        var projects = await ctrl.GetAll();
        
        repo.Verify(r => r.GetAll());
        var actionResult = Assert.IsType<ActionResult<List<ProjectTask>>>(projects);
        var projectList = Assert.IsType<List<ProjectTask>>(actionResult.Value);
        Assert.Empty(projectList);
    }

    [Fact]
    public async Task CanGetProjectTask()
    {
        var projectIn = Data.Task();
        var repo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(0), projectIn));
        
        var ctrl = Controllers.Task(repo);

        var result = await ctrl.Get(0);
        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(result);
        var projectOut = Assert.IsType<ProjectTask>(actionResult.Value);
        Assert.Equal(projectIn, projectOut);
    }

    [Fact]
    public async Task GetProjectTaskReturns404OnMissingProjectTask()
    {
        var taskIn = Data.Task();
        var repo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(taskIn.TaskId), taskIn));
        
        var ctrl = Controllers.Task(repo);

        var result = await ctrl.Get(999);
        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal("Task not found: 999", notFoundResult.Value);
    }

    [Fact]
    public async Task GetProjectTaskReturns404OnNoProjectTasks()
    {
        var repo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(It.IsAny<int>()), (ProjectTask?) null));
        
        var ctrl = Controllers.Task(repo);

        var returnResult = await ctrl.Get(999);
        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(returnResult);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal("Task not found: 999", notFoundResult.Value);
    }

    [Fact]
    public async Task CanCreateProjectTask()
    {
        var repo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.Add(It.IsAny<ProjectTask>()), (ProjectTask p) => new ProjectTask
        {
            TaskId = 1,
            Title = p.Title,
            Entries = new List<Entry>(p.Entries),
            Icon = p.Icon,
            ProjectId = p.ProjectId,
            ExternalId = p.ExternalId,
            Url = p.Url
        }));

        var ctrl = Controllers.Task(repo);

        var taskIn = Data.Task(setId: false);
        taskIn.Project = null;
        var returnResult = await ctrl.Create(taskIn);

        var actionResult = Assert.IsType<ActionResult<ProjectTask>>(returnResult);
        var projectOut = Assert.IsType<ProjectTask>(actionResult.Value);

        var expOut = Data.Task();
        expOut.TaskId = 1;
        expOut.Project = null;
        var expOutStr = JsonConvert.SerializeObject(expOut);
        var actOutStr = JsonConvert.SerializeObject(projectOut);
        Assert.Equal(expOutStr, actOutStr);
    }

    [Fact]
    public async Task CanUpdateProjectTask()
    {
        var repo = Repository.Repo<ProjectTask, bool>((r => r.Update(It.IsAny<int>(), It.IsAny<ProjectTask>()), true));
        
        var ctrl = Controllers.Task(repo);

        var projectIn = Data.Task();
        var returnResult = await ctrl.Update(projectIn.TaskId, projectIn);
        Assert.IsType<OkResult>(returnResult);
        repo.Verify(r => r.Update(projectIn.TaskId, projectIn), Times.Once);
    }
    
    [Fact]
    public async Task UpdateReturns500OnFail()
    {
        var repo = Repository.Repo<ProjectTask>();
        repo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<ProjectTask>())).ThrowsAsync(new Exception());
        
        var ctrl = Controllers.Task(repo);

        var projectIn = Data.Task();
        var returnResult = await ctrl.Update(projectIn.TaskId, projectIn);
        var faultResult = Assert.IsType<ObjectResult>(returnResult);
        var faultMessage = Assert.IsType<string>(faultResult.Value);
        Assert.Equal(500, faultResult.StatusCode);
        Assert.Equal("An error occurred updating the Task: " + projectIn.ProjectId, faultMessage);
        repo.Verify(r => r.Update(projectIn.TaskId, projectIn), Times.Once);
    }

    [Fact]
    public async Task UpdateReturns404OnNoSuchTask()
    {
        var repo = Repository.Repo<ProjectTask, bool>((r => r.Update(It.IsAny<int>(), It.IsAny<ProjectTask>()), false));
        var ctrl = Controllers.Task(repo);

        var projectIn = Data.Task(1);
        var returnResult = await ctrl.Update(1, projectIn);

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
        Assert.Equal("Task not found: 1", notFoundObjectResult.Value);
        repo.Verify(r => r.Update(1, projectIn), Times.Once);
    }

    [Fact]
    public async Task CanDeleteProjectTask()
    {
        var repo = Repository.Repo<ProjectTask, bool>((r => r.Remove(It.IsAny<int>()), true));
        
        var ctrl = Controllers.Task(repo);

        var projectIn = Data.Task();
        var returnResult = await ctrl.Delete(projectIn.TaskId);
        Assert.IsType<NoContentResult>(returnResult);
        repo.Verify(r => r.Remove(projectIn.TaskId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteReturns404OnFail()
    {
        var repo = Repository.Repo<ProjectTask, bool>((r => r.Remove(It.IsAny<int>()), false));

        var ctrl = Controllers.Task(repo);

        var taskIn = Data.Task();
        var returnResult = await ctrl.Delete(taskIn.TaskId);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
        Assert.Equal("Task not found: " + taskIn.TaskId, notFoundObjectResult.Value);
        repo.Verify(r => r.Remove(taskIn.TaskId), Times.Once);
    }
}