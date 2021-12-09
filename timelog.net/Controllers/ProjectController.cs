using Microsoft.AspNetCore.Mvc;
using timelog.net.Models;

namespace timelog.net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        [HttpGet]
        public IActionResult GetProjects()
        {
            var data = new[] {new Project() {Id = "0", Title = "Test Project "}};
            return Json(data);
        }
    }
}