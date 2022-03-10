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
        public void CanCreateEntry()
        {
            Assert.True(false);
        }

        [Fact]
        public void CreateEntryReturns404OnEntryNotFound()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task CanGetListOfEntriesByTask()
        {
            var task = Data.Task();
            task.Entries.Add(Data.Entry(task));

            var taskRepo = Repository.Task((r => r.GetById(task.TaskId), task));
            var ctrl = Controllers.Entry(taskRepo: taskRepo);

            var retVal = await ctrl.GetEntries(task.TaskId);
            var entryResult = Assert.IsType<ActionResult<List<Entry>>>(retVal);
            var entryList = Assert.IsType<List<Entry>>(entryResult.Value);
            Assert.Equal(task.Entries, entryList);
        }

        [Fact]
        public async Task EntryListReturns404OnNoTask()
        {
            var taskRepo = Repository.Task((r => r.GetById(It.IsAny<int>()), (ProjectTask?)null));
            var ctrl = Controllers.Entry(taskRepo: taskRepo);

            var retVal = await ctrl.GetEntries(999);
            var entryResult = Assert.IsType<ActionResult<List<Entry>>>(retVal);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(entryResult.Result);
            Assert.Equal("Task not found: 999", notFoundResult.Value);
        }

        [Fact]
        public async Task CanGetEntryById()
        {
            var entry = Data.Entry(Data.Task());
            var repo = Repository.Entry((r => r.GetById(entry.EntryId), entry));
            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.GetEntry(0, 1);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var entryOut = Assert.IsType<Entry>(entryResult.Value);
            Assert.Equal(entry, entryOut);
        }

        [Fact]
        public async Task CanRemoveEntry()
        {
            var repo = Repository.Entry(r => r.Remove(0, 1), true);
            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.RemoveEntry(0, 1);
            Assert.IsType<NoContentResult>(retVal);
            repo.Verify(r => r.Remove(0, 1), Times.Once);
        }

        [Fact]
        public async Task Returns404IfEntryNotFoundForRemove()
        {
            var repo = Repository.Entry(r => r.Remove(0, 1), false);
            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.RemoveEntry(0, 1);
            var notFoundRes = Assert.IsType<NotFoundObjectResult>(retVal);
            Assert.Equal("Entry 1 not found on task 0", notFoundRes.Value);
            repo.Verify(r => r.Remove(0, 1), Times.Once);
        }

        [Fact]
        public async Task Returns404IfNoEntryById()
        {
            var repo = Repository.Entry((r => r.GetById(It.IsAny<int>()), (Entry?)null));
            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.GetEntry(0, 1);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(entryResult.Result);
            Assert.Equal("Entry 1 not found on task 0", notFoundResult.Value);
        }

        [Fact]
        public async Task Returns404IfEntryDoesNotMatchTask()
        {
            var entry = Data.Entry(Data.Task(1));
            var repo = Repository.Entry((r => r.GetById(entry.EntryId), entry));
            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.GetEntry(0, 1);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(entryResult.Result);
            Assert.Equal("Entry 1 not found on task 0", notFoundResult.Value);
        }

        [Fact]
        public async Task CanGetActiveEntry()
        {
            var entry = Data.Entry(Data.Task());
            var repo = Repository.Entry(r => r.GetActive(1), entry);

            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.GetActiveEntry(1);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var found = Assert.IsType<Entry>(entryResult.Value);
            Assert.Equal(entry, found);
        }

        [Fact]
        public async Task ActiveListReturns204OnNoActive()
        {
            var task = Data.Task();
            var repo = Repository.Entry(r => r.GetActive(task.TaskId), null);

            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.GetActiveEntry(task.TaskId);
            var entryResult = Assert.IsType<ActionResult<Entry?>>(retVal);
            Assert.IsType<NoContentResult>(entryResult.Result);
            repo.Verify(r => r.GetActive(task.TaskId));
        }

        [Fact]
        public async Task ActiveListReturns404OnNoSuchTask()
        {
            var repo = Repository.Entry(r => r.GetActive(It.IsAny<int>()), new Entry());
            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.GetActiveEntry(999);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(entryResult.Result);
            Assert.Equal("Task not found: 999", notFoundResult.Value);
            repo.Verify(r => r.GetActive(999));
        }

        [Fact]
        public async Task CanStartEntry()
        {
            var task = Data.Task();
            var entry = Data.Entry(task);
            var repo = Repository.Entry((r => r.Add(It.IsAny<Entry>()), entry));
            repo.Setup(r => r.GetActive(It.IsAny<int>()))
                .ReturnsAsync((Entry?)null);

            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.StartTask(task.TaskId, null);
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var entryOut = Assert.IsType<Entry>(entryResult.Value);
            Assert.Equal(entry, entryOut);
            repo.Verify(r => r.Add(It.Is<Entry>(e => VerifyEntry(e, task.TaskId, null, null, null))), Times.Once);
        }

        [Fact]
        public async Task CanStartEntryWithNote()
        {
            var task = Data.Task();
            var entry = Data.Entry(task);
            var repo = Repository.Entry((r => r.Add(It.IsAny<Entry>()), entry));
            repo.Setup(r => r.GetActive(It.IsAny<int>()))
                .ReturnsAsync((Entry?)null);

            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.StartTask(task.TaskId, "This is a note");
            var entryResult = Assert.IsType<ActionResult<Entry>>(retVal);
            var entryOut = Assert.IsType<Entry>(entryResult.Value);
            Assert.Equal(entry, entryOut);
            repo.Verify(r => r.Add(It.Is<Entry>(e => VerifyEntry(e, task.TaskId, "This is a note", null, null))), Times.Once);
        }

        [Fact]
        public async Task ReturnsErrorIfAlreadyStarted()
        {
            var task = Data.Task();
            var entry = Data.Entry(task);
            var repo = Repository.Entry((r => r.GetActive(It.IsAny<int>()), entry));

            var ctrl = Controllers.Entry(repo);

            var retVal = await ctrl.StartTask(task.TaskId, null);
            Assert.IsType<ActionResult<Entry>>(retVal);

            var errorRet = await ctrl.StartTask(task.TaskId, null);
            Assert.IsType<ActionResult<Entry>>(errorRet);
            var errorResult = Assert.IsType<ConflictObjectResult>(errorRet.Result);
            Assert.Equal("Task already started: " + task.TaskId, errorResult.Value);
            repo.Verify(r => r.Add(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task Returns404OnStartIfNoTask()
        {
            var repo = Repository.Entry((r => r.GetActive(It.IsAny<int>()), new Entry()));

            var ctrl = Controllers.Entry(repo);

            var errorRet = await ctrl.StartTask(999, null);
            Assert.IsType<ActionResult<Entry>>(errorRet);
            var errorResult = Assert.IsType<NotFoundObjectResult>(errorRet.Result);
            Assert.Equal("Task not found: 999", errorResult.Value);
            repo.Verify(r => r.Add(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task CanStopEntry()
        {
            var entryIn = Data.Entry(Data.Task(1), 2);
            var repo = Repository.Entry(r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), true);
            repo.Setup(r => r.GetActive(It.IsAny<int>())).ReturnsAsync(entryIn);
            var ctrl = Controllers.Entry(repo);

            var entryRet = await ctrl.StopTask(1, "This is a note");

            Assert.IsType<ActionResult<Entry>>(entryRet);
            var entryOut = Assert.IsType<Entry>(entryRet.Value);
            var endTime = Assert.IsType<DateTime>(entryOut.EndTime);

            Assert.Equal(1, entryOut.TaskId);
            Assert.Equal(2, entryOut.EntryId);
            Assert.Equal(entryIn.StartTime, entryOut.StartTime);
            Assert.Equal(DateTime.UtcNow, endTime, TimeSpan.FromMinutes(1));
            Assert.Equal("This is a note", entryOut.Note);

            repo.Verify(r => r.Update(entryIn.EntryId, It.Is<Entry>(e => VerifyEntry(e, 1, "This is a note", entryIn.StartTime, endTime))));
        }

        [Fact]
        public async Task ReturnsError500OnStopIfUpdateError()
        {
            var entryIn = Data.Entry(Data.Task(1));
            var repo = Repository.Entry(r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), false);
            repo.Setup(r => r.GetActive(It.IsAny<int>())).ReturnsAsync(entryIn);
            var ctrl = Controllers.Entry(repo);

            var stopRes = await ctrl.StopTask(1, "This is a note");
            var stopRet = Assert.IsType<ActionResult<Entry>>(stopRes);
            var conflictRet = Assert.IsType<StatusCodeResult>(stopRet.Result);
            Assert.Equal(500, conflictRet.StatusCode);
            repo.Verify(r => r.Add(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task ReturnsError409OnStopIfNotStarted()
        {
            var repo = Repository.Entry(r => r.GetActive(It.IsAny<int>()), new Entry());
            var ctrl = Controllers.Entry(repo);

            var stopRes = await ctrl.StopTask(999, "This is a note");
            var stopRet = Assert.IsType<ActionResult<Entry>>(stopRes);
            var conflictRet = Assert.IsType<ConflictObjectResult>(stopRet.Result);
            Assert.Equal("Task is not started: 999", conflictRet.Value);
            repo.Verify(r => r.Add(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task Returns404OnStopIfNoSuchTask()
        {
            var repo = Repository.Entry(r => r.GetActive(It.IsAny<int>()), null);
            var ctrl = Controllers.Entry(repo);

            var stopRes = await ctrl.StopTask(999, "This is a note");
            var stopRet = Assert.IsType<ActionResult<Entry>>(stopRes);
            var conflictRet = Assert.IsType<NotFoundObjectResult>(stopRet.Result);
            Assert.Equal("Task not found: 999", conflictRet.Value);
            repo.Verify(r => r.Add(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task CanUpdateEntry()
        {
            var repo = Repository.Entry((r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), true));

            var ctrl = Controllers.Entry(repo);

            var entryIn = Data.Entry(Data.Task(1), 2);
            var returnResult = await ctrl.UpdateEntry(1, 2, entryIn);
            Assert.IsType<OkResult>(returnResult);
            repo.Verify(r => r.Update(2, entryIn), Times.Once);
        }

        [Fact]
        public async Task UpdateReturns500OnUpdateError()
        {
            var repo = Repository.Entry();
            repo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()))
                .ThrowsAsync(new Exception());
            var ctrl = Controllers.Entry(repo);

            var entryIn = Data.Entry(Data.Task(1), 2);
            var returnResult = await ctrl.UpdateEntry(1, 2, entryIn);
            var statusResult = Assert.IsType<ObjectResult>(returnResult);
            var message = Assert.IsType<string>(statusResult.Value);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("An error occurred updating Entry 2 on Task 1", message);
            repo.Verify(r => r.Update(2, entryIn), Times.Once);
        }

        [Fact]
        public async Task UpdateReturns404OnNoSuchTask()
        {
            var repo = Repository.Entry();
            var ctrl = Controllers.Entry(repo);

            var entryIn = Data.Entry(Data.Task(1), 2);
            var returnResult = await ctrl.UpdateEntry(1, 2, entryIn);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
            Assert.Equal("Task not found: 1", notFoundObjectResult.Value);
            repo.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task UpdateReturns404OnNoSuchEntryOnTask()
        {
            var repo = Repository.Entry();
            var ctrl = Controllers.Entry(repo);

            var entryIn = Data.Entry(Data.Task(1), 2);
            var returnResult = await ctrl.UpdateEntry(1, 2, entryIn);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(returnResult);
            Assert.Equal("Entry 2 not found on task 1", notFoundObjectResult.Value);
            repo.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<Entry>()), Times.Never);
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
