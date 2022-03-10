using System.Collections.Generic;
using System.Threading.Tasks;
using timelog.net.Models;

namespace timelog.net.Data;

public interface IEntryRepository : IRepository<Entry>
{
    /**
     * Get the current active entry (no stop time) for the given task
     * Returns null if no active for the given task
     * Returns an empty Entry (no task) if task does not exist
     */
    Task<Entry?> GetActive(int taskId);

    /**
     * Remove the entry by the given Task / Entry ID combination
     * Returns true if removed, false if entry does not exist on task
     */
    Task<bool> Remove(int taskId, int entryId);
}