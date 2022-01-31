using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;
using timelog.net.Models;

namespace timelog.net.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly IRepository<ProjectTask> _repository;

        public TaskController(IRepository<ProjectTask> repository) => _repository = repository;

        [HttpGet]
        [Route("{taskId:int}")]
        public async Task<ProjectTask?> GetTask(int taskId) => await _repository.GetById(taskId);

        [HttpPatch]
        [Route("{taskId:int}")]
        public Task<ProjectTask> PatchTask(int taskId, [FromBody] ProjectTask projectTask)
        {
            throw new NotImplementedException();
            /*task.TaskId = taskId;
            
            DbContext.Tasks.Attach(task);
            DbContext.Entry(task).Property(p => p.Title).IsModified = true;
            await DbContext.SaveChangesAsync();
            return await GetTask(taskId);*/
        }
    }
}