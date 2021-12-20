using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace timelog.net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private ProjectContext DbContext { get; }

        public ProjectController(ProjectContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            return Json(await DbContext.Projects.ToListAsync());
        }

        [HttpGet]
        [Route("{projectId:int}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            var project = await DbContext.Projects
                .Where(p => p.ProjectId == projectId)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync();

            if (project is null) return NotFound();
            
            return Json(project);
        }
    }
}