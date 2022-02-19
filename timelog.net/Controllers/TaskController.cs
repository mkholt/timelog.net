using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;
using timelog.net.Models;

namespace timelog.net.Controllers
{
    [Route("[controller]")]
    public class TaskController : BaseController<ProjectTask>
    {
        public TaskController(IRepository<ProjectTask> repository) : base(repository)
        {
        }
    }
}