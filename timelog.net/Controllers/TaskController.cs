using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace timelog.net.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private ProjectContext DbContext { get; }

        public TaskController(ProjectContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet]
        [Route("{taskId:int}")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            var task = await DbContext.Tasks.FindAsync(taskId);
            return Json(task);
        }

        [HttpPatch]
        [Route("{taskId:int}")]
        public async Task<IActionResult> PatchTask(int taskId, [FromBody] Models.Task task)
        {
            task.TaskId = taskId;
            
            DbContext.Tasks.Attach(task);
            DbContext.Entry(task).Property(p => p.Title).IsModified = true;
            await DbContext.SaveChangesAsync();
            return Ok();
        }
    }
}