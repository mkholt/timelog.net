using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;
using System.Threading.Tasks;

namespace timelog.net.Data;

public class TaskRepository : IRepository<ProjectTask>
{
    private readonly ProjectContext _context;

    public TaskRepository(ProjectContext context) => _context = context;

    public Task<ProjectTask?> GetById(int entityId) =>
        _context.Tasks.Where(t => t.TaskId == entityId).Include(t => t.Entries).FirstOrDefaultAsync();

    public async Task<IEnumerable<ProjectTask>> GetAll() => await _context.Tasks.ToListAsync();

    public async Task<ProjectTask?> Add(ProjectTask entry)
    {
        var task = await _context.Tasks.AddAsync(entry);
        await _context.SaveChangesAsync();
        return task.Entity;
    }

    public Task<bool> Update(int taskId, ProjectTask newEntry)
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> Remove(int entityId)
    {
        var entry = await _context.Tasks.FindAsync(entityId);
        if (entry is null) return false;
        _context.Tasks.Remove(entry);
        return await _context.SaveChangesAsync() > 0;
    }
}