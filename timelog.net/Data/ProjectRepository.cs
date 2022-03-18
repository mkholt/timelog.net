using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;

namespace timelog.net.Data;

public class ProjectRepository : BaseRepository<Project>
{
    public ProjectRepository(ProjectContext context) : base(context.Projects, () => context.SaveChangesAsync())
    {
    }

    public override async Task<Project?> GetById(int entityId) =>
        await DbSet
            .Where(p => p.ProjectId == entityId)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync();
}
