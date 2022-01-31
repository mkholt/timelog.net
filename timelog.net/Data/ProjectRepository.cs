using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;
using Task = System.Threading.Tasks.Task;

namespace timelog.net.Data;

public class ProjectRepository : IRepository<Project>
{
    private readonly ProjectContext _context;
    
    public ProjectRepository(ProjectContext context) => _context = context;

    public async Task<Project?> GetById(int id) =>
        await _context.Projects
            .Where(p => p.ProjectId == id)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Project>> GetAll() => await _context.Projects.ToListAsync();

    public async Task Add(Project entry) => await _context.Projects.AddAsync(entry);

    public Task Update(Project newEntry)
    {
        throw new System.NotImplementedException();
    }

    public Task Remove(Project entry) => Task.FromResult(_context.Projects.Remove(entry));

    public async Task Remove(int id)
    {
        var entry = await _context.Projects.FindAsync(id);
        if (entry is null) return;
        await Remove(entry);
    }

    public async Task SaveChanges() => await _context.SaveChangesAsync();
}