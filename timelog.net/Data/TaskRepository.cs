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

    public Task<ProjectTask?> GetById(int id) =>
        _context.Tasks.Where(t => t.TaskId == id).Include(t => t.Entries).FirstOrDefaultAsync();

    public async Task<IEnumerable<ProjectTask>> GetAll() => await _context.Tasks.ToListAsync();

    public async Task Add(ProjectTask entry) => await _context.Tasks.AddAsync(entry);

    public Task Update(ProjectTask newEntry)
    {
        throw new System.NotImplementedException();
    }

    public Task Remove(ProjectTask entry) => Task.FromResult(_context.Tasks.Remove(entry));

    public async Task Remove(int id)
    {
        var entry = await _context.Tasks.FindAsync(id);
        if (entry is null) return;
        await Remove(entry);
    }

    public async Task SaveChanges() => await _context.SaveChangesAsync();
}