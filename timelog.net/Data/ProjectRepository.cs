using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;

namespace timelog.net.Data;

public class ProjectRepository : IRepository<Project>
{
    private readonly ProjectContext _context;
    
    public ProjectRepository(ProjectContext context) => _context = context;

    public async Task<Project?> GetById(int entityId) =>
        await _context.Projects
            .Where(p => p.ProjectId == entityId)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Project>> GetAll() => await _context.Projects.ToListAsync();

    public async Task<Project> Add(Project entry)
    {
        var project = await _context.Projects.AddAsync(entry);
        await _context.SaveChangesAsync();
        return project.Entity;
    }

    public Task<bool> Update(int projectId, Project newEntry)
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> Remove(int entityId)
    {
        var entry = await _context.Projects.FindAsync(entityId);
        if (entry is null) return false;
        _context.Projects.Remove(entry);
        return await _context.SaveChangesAsync() > 0;
    }
}