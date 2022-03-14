using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using timelog.net.Models;
using Xunit;

namespace UnitTest
{
    public class EntryControllerTests
    {
        #region Entries

        [Fact]
        public async Task CanGetListOfEntriesByTask()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task, 2);

            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var ctrl = Controllers.Entry(null, taskRepo, 1);

            var retVal = await ctrl.GetAll();
            var entryResult = Assert.IsType<ActionResult<List<Entry>>>(retVal);
            var entryList = Assert.IsType<List<Entry>>(entryResult.Value);
            Assert.Equal(task.Entries, entryList);
        }

        [Fact]
        public async Task EntryListReturns404OnNoTask()
        {
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(It.IsAny<int>()), (ProjectTask?)null));
            var ctrl = Controllers.Entry(null, taskRepo, 999);

            var retVal = await ctrl.GetAll();
            var entryResult = Assert.IsType<ActionResult<List<Entry>>>(retVal);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(entryResult.Result);
            Assert.Equal("Task not found: 999", notFoundResult.Value);
        }

        [Fact]
        public async Task CanGetActiveEntry()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task, 2);
            entry.EndTime = null;
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var ctrl = Controllers.Entry(null, taskRepo, 1);

            var retVal = await ctrl.GetActiveEntry();
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var found = Assert.IsType<Entry>(entryResult.Value);
            Assert.Equal(entry, found);
        }

        [Fact]
        public async Task ActiveListReturns204OnNoActive()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task, 2);
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var ctrl = Controllers.Entry(null, taskRepo, 1);

            var retVal = await ctrl.GetActiveEntry();
            var entryResult = Assert.IsType<ActionResult<Entry?>>(retVal);
            Assert.IsType<NoContentResult>(entryResult.Result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("This is a note")]
        public async Task CanStartEntry(string? note)
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task);
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var repo = Repository.Repo<Entry, Entry?>((r => r.Add(It.IsAny<Entry>()), entry));

            var ctrl = Controllers.Entry(repo, taskRepo, 1);

            var retVal = await ctrl.StartTask(note);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var entryOut = Assert.IsType<Entry>(entryResult.Value);
            Assert.Equal(entry, entryOut);
            repo.Verify(r => r.Add(It.Is<Entry>(e => VerifyEntry(e, 1, note, null, null))), Times.Once);
        }

        [Fact]
        public async Task ReturnsErrorIfAlreadyStarted()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task);
            entry.EndTime = null;
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var repo = Repository.Repo<Entry, Entry?>((r => r.Add(It.IsAny<Entry>()), entry));

            var ctrl = Controllers.Entry(repo, taskRepo, 1);

            var errorRet = await ctrl.StartTask(null);
            Assert.IsType<ActionResult<Entry>>(errorRet);
            var errorResult = Assert.IsType<ConflictObjectResult>(errorRet.Result);
            Assert.Equal("Task already started: 1", errorResult.Value);
            repo.Verify(r => r.Add(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task ReturnsErrorIfStartFails()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task);
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var repo = Repository.Repo<Entry, Entry?>((r => r.Add(It.IsAny<Entry>()), (Entry?)null));

            var ctrl = Controllers.Entry(repo, taskRepo, 1);

            var retVal = await ctrl.StartTask(null);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var errorRet = Assert.IsType<ObjectResult>(entryResult.Result);
            Assert.Equal(500, errorRet.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("This is a note")]
        public async Task CanStopEntry(string note)
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task, 2);
            entry.EndTime = null;

            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var repo = Repository.Repo<Entry, bool>((r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), true));
            var ctrl = Controllers.Entry(repo, taskRepo, 1);

            var entryRet = await ctrl.StopTask(note);

            Assert.IsType<ActionResult<Entry>>(entryRet);
            var entryOut = Assert.IsType<Entry>(entryRet.Value);
            var endTime = Assert.IsType<DateTime>(entryOut.EndTime);

            Assert.Equal(1, entryOut.TaskId);
            Assert.Equal(2, entryOut.EntryId);
            Assert.Equal(entry.StartTime, entryOut.StartTime);
            Assert.Equal(DateTime.UtcNow, endTime, TimeSpan.FromMinutes(1));
            Assert.Equal(note, entryOut.Note);

            repo.Verify(r => r.Update(entry.EntryId, It.Is<Entry>(e => VerifyEntry(e, 1, note, entry.StartTime, endTime))));
        }

        [Fact]
        public async Task ReturnsErrorInternalServerOnStopIfUpdateError()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task, 2);
            entry.EndTime = null;

            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var repo = Repository.Repo<Entry, bool>((r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), false));
            var ctrl = Controllers.Entry(repo, taskRepo, 1);

            var stopRes = await ctrl.StopTask(null);
            var stopRet = Assert.IsType<ActionResult<Entry>>(stopRes);
            var conflictRet = Assert.IsType<ObjectResult>(stopRet.Result);
            Assert.Equal(500, conflictRet.StatusCode);
        }

        [Fact]
        public async Task ReturnsErrorConflicOnStopIfNotStarted()
        {
            var task = Data.Task(1);
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var repo = Repository.Repo<Entry>();
            var ctrl = Controllers.Entry(repo, taskRepo, 1);

            var stopRes = await ctrl.StopTask("This is a note");
            var stopRet = Assert.IsType<ActionResult<Entry>>(stopRes);
            var conflictRet = Assert.IsType<ConflictObjectResult>(stopRet.Result);
            Assert.Equal("Task is not started: 1", conflictRet.Value);
        }

        [Fact]
        public async Task ReturnsErrorNotFoundIfEntryDoesNotMatchTask()
        {
            var task = Data.Task(1);
            var entry = Data.Entry(task, 2);
            var taskRepo = Repository.Repo<ProjectTask, ProjectTask?>((r => r.GetById(1), task));
            var ctrl = Controllers.Entry(null, taskRepo, 1);

            var taskRes = await ctrl.Get(3);
            var taskRet = Assert.IsType<ActionResult<Entry>>(taskRes);
            var notFoundRet = Assert.IsType<NotFoundObjectResult>(taskRet.Result);
            Assert.Equal("Entry 3 not found on task 1", notFoundRet.Value);
        }

        #endregion

        #region Helper methods

        private static bool VerifyEntry(Entry e, int taskId, string? note, DateTime? startTime, DateTime? endTime)
        {
            if (e.TaskId != taskId) return false;
            if (e.Note != note) return false;
            if (startTime is null)
            {
                if (e.StartTime > DateTime.UtcNow || e.StartTime < DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1))) return false;
            } else
            {
                if (e.StartTime != startTime) return false;
            }
            return e.EndTime == endTime;
        }

        #endregion
    }
}
