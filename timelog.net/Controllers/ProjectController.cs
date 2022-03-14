using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;
using timelog.net.Models;

namespace timelog.net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : BaseController<Project>
    {
        public ProjectController(IRepository<Project> repository) : base(repository)
        {
        }
    }
}