using System.Collections.Generic;
using System.Linq;
using Moq;
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

        var projects = await ctrl.GetProjects();
        
        repo.Verify(r => r.GetAll());
        Assert.Empty(projects);
    }

    [Fact]
    public async Task CanGetProject()
    {
        var projectIn = new Project
        {
            ProjectId = 0,
            Title = "Project Title",
            Tasks = new List<timelog.net.Models.Task>()
        };
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(projectIn);
        
        var ctrl = new ProjectController(repo.Object);

        var projectOut = await ctrl.GetProject(0);
        Assert.Equal(projectIn, projectOut);
    }

    [Fact]
    public async Task GetProjectReturnsNullOnMissingProject()
    {
        var projectIn = new Project
        {
            ProjectId = 0,
            Title = "Project Title",
            Tasks = new List<timelog.net.Models.Task>()
        };
        
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetById(0))
            .ReturnsAsync(projectIn);
        
        var ctrl = new ProjectController(repo.Object);

        var projectOut = await ctrl.GetProject(1);
        Assert.Null(projectOut);
    }

    [Fact]
    public async Task GetProjectReturnsNullOnNoProjects()
    {
        var repo = new Mock<IRepository<Project>>();
        repo.Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync((Project?) null);
        
        var ctrl = new ProjectController(repo.Object);

        var projectOut = await ctrl.GetProject(0);
        Assert.Null(projectOut);
    }
}