using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;
using timelog.net.Models;

namespace timelog.net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly IRepository<Project> _repository;

        public ProjectController(IRepository<Project> repository) => _repository = repository;

        [HttpGet]
        public async Task<IEnumerable<Project>> GetProjects() => await _repository.GetAll();

        [HttpGet]
        [Route("{projectId:int}")]
        public async Task<Project?> GetProject(int projectId) => await _repository.GetById(projectId);
    }
}