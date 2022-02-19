using System;
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
using Task = System.Threading.Tasks.Task;

namespace UnitTest;

public class ProjectControllerTests
{
    [Fact]
    public async Task CanGetEmptyProjectList()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetAll())
            .ReturnsAsync(Enumerable.Empty<Project>());

        var ctrl = new ProjectController(repo.Object);

        var projects = await ctrl.GetAll();
        
        repo.Verify(r => r.GetAll());
        var actionResult = Assert.IsType<ActionResult<List<Project>>>(projects);
        var projectList = Assert.IsType<List<Project>>(actionResult.Value);
        Assert.Empty(projectList);
    }

    [Fact]
    public async Task CanGetProject()
    {
        var projectIn = CreateProject();
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(projectIn);
        
        var ctrl = new ProjectController(repo.Object);

        var result = await ctrl.Get(0);
        var actionResult = Assert.IsType<ActionResult<Project>>(result);
        var projectOut = Assert.IsType<Project>(actionResult.Value);
        Assert.Equal(projectIn, projectOut);
    }

    [Fact]
    public async Task GetProjectReturns404OnMissingProject()
    {
        var projectIn = CreateProject();
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(projectIn);
        
        var ctrl = new ProjectController(repo.Object);

        const int nonExistentId = 999;
        var result = await ctrl.Get(nonExistentId);
        var actionResult = Assert.IsType<ActionResult<Project>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal(nonExistentId, notFoundResult.Value);
    }

    [Fact]
    public async Task GetProjectReturns404OnNoProjects()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync((Project?) null);
        
        var ctrl = new ProjectController(repo.Object);

        const int nonExistentId = 999;
        var returnResult = await ctrl.Get(nonExistentId);
        var actionResult = Assert.IsType<ActionResult<Project>>(returnResult);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal(nonExistentId, notFoundResult.Value);
    }

    [Fact]
    public async Task CanCreateProject()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.Add(It.IsAny<Project>()))
            .ReturnsAsync((Project p) => new Project
            {
                ProjectId = 1,
                Title = p.Title,
                Url = p.Url,
                ExternalId = p.ExternalId,
                Tasks = new List<ProjectTask>(p.Tasks)
            });

        var ctrl = new ProjectController(repo.Object);

        var projectIn = CreateProject(false);
        var returnResult = await ctrl.Create(projectIn);

        var actionResult = Assert.IsType<ActionResult<Project>>(returnResult);
        var projectOut = Assert.IsType<Project>(actionResult.Value);

        var expOut = CreateProject();
        expOut.ProjectId = 1;
        var expOutStr = JsonConvert.SerializeObject(expOut);
        var actOutStr = JsonConvert.SerializeObject(projectOut);
        Assert.Equal(expOutStr, actOutStr);
    }

    [Fact]
    public async Task CanUpdateProject()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<Project>()))
            .ReturnsAsync(true);
        
        var ctrl = new ProjectController(repo.Object);

        var projectIn = CreateProject();
        var returnResult = await ctrl.Update(projectIn.ProjectId, projectIn);
        Assert.IsType<OkResult>(returnResult);
        repo.Verify(r => r.Update(projectIn.ProjectId, projectIn), Times.Once);
    }
    
    [Fact]
    public async Task UpdateReturns404OnFail()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<Project>()))
            .ReturnsAsync(false);
        
        var ctrl = new ProjectController(repo.Object);

        var projectIn = CreateProject();
        var returnResult = await ctrl.Update(projectIn.ProjectId, projectIn);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
        Assert.Equal(projectIn.ProjectId, notFoundObjectResult.Value);
        repo.Verify(r => r.Update(projectIn.ProjectId, projectIn), Times.Once);
    }
    
    [Fact]
    public async Task CanDeleteProject()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.Remove(It.IsAny<int>()))
            .ReturnsAsync(true);
        
        var ctrl = new ProjectController(repo.Object);

        var projectIn = CreateProject();
        var returnResult = await ctrl.Delete(projectIn.ProjectId);
        Assert.IsType<OkResult>(returnResult);
        repo.Verify(r => r.Remove(projectIn.ProjectId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteReturns404OnFail()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.Remove(It.IsAny<int>()))
            .ReturnsAsync(false);
        
        var ctrl = new ProjectController(repo.Object);

        var projectIn = CreateProject();
        var returnResult = await ctrl.Delete(projectIn.ProjectId);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
        Assert.Equal(projectIn.ProjectId, notFoundObjectResult.Value);
        repo.Verify(r => r.Remove(projectIn.ProjectId), Times.Once);
    }
    
    private static Project CreateProject(bool setId = true)
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
}