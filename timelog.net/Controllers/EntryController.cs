using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using timelog.net.Data;
using timelog.net.Models;
using System.Linq;
using NuGet.Protocol.Core.Types;

namespace timelog.net.Controllers
{
    [Route("task/{taskId:int}/entries")]
    public class EntryController : Controller
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IRepository<ProjectTask> _taskRepository;

        public EntryController(IEntryRepository entryRepository, IRepository<ProjectTask> taskRepository)
        {
            _entryRepository = entryRepository;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Entry>>> GetEntries(int taskId)
        {
            var task = await _taskRepository.GetById(taskId);
            return task is null
                ? NotFound("Task not found: " + taskId)
                : task.Entries.ToList();
        }

        [HttpPost]
        public Task<ActionResult<Entry>> CreateEntry([FromBody] Entry entry)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("active")]
        public async Task<ActionResult<Entry>> GetActiveEntry(int taskId)
        {
            var entry = await _entryRepository.GetActive(taskId);
            if (entry is null) return NoContent();
            if (entry.TaskId is null) return NotFound("Task not found: " + taskId);
            return entry;
        }

        [HttpPut]
        [Route("start")]
        public async Task<ActionResult<Entry>> StartTask(int taskId, [FromBody] string? note)
        {
            var active = await GetActiveEntry(taskId);
            if (active.Result is NotFoundObjectResult)
                return active.Result;

            if (active.Result is NoContentResult)
            {
                var entry = await _entryRepository.Add(new Entry
                {
                    TaskId = taskId,
                    StartTime = DateTime.UtcNow,
                    Note = note
                });

                return entry is null ? StatusCode(500) : entry;
            }

            return Conflict($"Task already started: {taskId}");
        }

        [HttpPut]
        [Route("stop")]
        public async Task<ActionResult<Entry>> StopTask(int taskId, [FromBody] string? note)
        {
            // Get the current active task
            var active = await GetActiveEntry(taskId);
            if (active.Result is NotFoundObjectResult) return Conflict($"Task is not started: {taskId}");
            if (active.Result is NoContentResult) return NotFound($"Task not found: {taskId}");
            var entry = active.Value;
            if (entry is null) return StatusCode(500);

            entry.EndTime = DateTime.UtcNow;

            var updated = await _entryRepository.Update(entry.EntryId, entry);
            return (!updated) ? StatusCode(500) : entry;
        }

        [HttpGet]
        [Route("{entryId:int}")]
        public async Task<ActionResult<Entry>> GetEntry(int taskId, int entryId)
        {
            var entry = await _entryRepository.GetById(entryId);
            return entry is null || entry.TaskId != taskId
                ? NotFound($"Entry {entryId} not found on task {taskId}")
                : entry;
        }

        [HttpPatch]
        [Route("{entryId:int}")]
        public async Task<ActionResult> UpdateEntry(int taskId, int entryId, [FromBody] Entry entryPatch)
        {
            try
            {
                return await _entryRepository.Update(entryId, entryPatch)
                    ? Ok()
                    : NotFound($"Entry not found: {entryId}");
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred updating Entry {entryId} on Task {taskId}");
            }
        }

        [HttpDelete]
        [Route("{entryId:int}")]
        public async Task<ActionResult> RemoveEntry(int taskId, int entryId)
        {
            return await _entryRepository.Remove(taskId, entryId)
                ? NoContent()
                : NotFound($"Entry {entryId} not found on task {taskId}");
        }
    }
}
