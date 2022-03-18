using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using timelog.net.Data;
using timelog.net.Models;
using System.Linq;
using System.Net;
using timelog.net.Exceptions;

namespace timelog.net.Controllers
{
    [Route("task/{taskId:int}/entries")]
    public class EntryController : BaseController<Entry>
    {
        private readonly IRepository<Entry> _entryRepository;
        private readonly IRepository<ProjectTask> _taskRepository;

        private ProjectTask CurrentTask { get; set; } = new ProjectTask();

        [FromRoute(Name = "taskId")]
        public int TaskId { get; set; }

        private Entry? CurrentActive => CurrentTask.Entries.FirstOrDefault(e => e.EndTime is null);

        public EntryController(IRepository<Entry> entryRepository, IRepository<ProjectTask> taskRepository) : base(entryRepository)
        {
            _entryRepository = entryRepository;
            _taskRepository = taskRepository;
        }

        protected override async Task Validate(int? entryId)
        {
            var task = await _taskRepository.GetById(TaskId);
            if (task is null) throw new ControllerException(NotFound("Task not found: " + TaskId));
            CurrentTask = task;

            if (entryId is null) return;

            if (!task.Entries.Any(e => e.EntryId == entryId)) throw new ControllerException(NotFound("Entry " + entryId + " not found on task " + TaskId));
        }

        public override async Task<ActionResult<List<Entry>>> GetAll() =>
            await HandleRequest(null, () => Task.FromResult(CurrentTask.Entries.ToList()));

        [HttpGet]
        [Route("active")]
        public async Task<ActionResult<Entry>> GetActiveEntry()
        {
            return await HandleRequest(null, () => Task.FromResult(CurrentActive ?? throw new ControllerException(base.NoContent())));
        }

        [HttpPut]
        [Route("start")]
        public async Task<ActionResult<Entry>> StartTask([FromBody] string? note)
        {
            return await HandleRequest(null, async () =>
            {
                var active = CurrentActive;
                if (active is not null) throw new ControllerException(Conflict("Task already started: " + TaskId));
                var entry = await _entryRepository.Add(new Entry
                {
                    TaskId = TaskId,
                    StartTime = DateTime.UtcNow,
                    Note = note
                });

                return entry ?? throw new ControllerException(HttpStatusCode.InternalServerError);
            });
        }

        [HttpPut]
        [Route("stop")]
        public async Task<ActionResult<Entry>> StopTask([FromBody] string? note)
        {
            return await HandleRequest(null, async () =>
            {
                var active = CurrentActive;
                if (active is null) throw new ControllerException(Conflict("Task is not started: " + TaskId));

                active.EndTime = DateTime.UtcNow;
                active.Note = note;
                var updated = await _entryRepository.Update(active.EntryId, active);
                return updated ? active : throw new ControllerException(HttpStatusCode.InternalServerError);
            });
        }
    }
}
