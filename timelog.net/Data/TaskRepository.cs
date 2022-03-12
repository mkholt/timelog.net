using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;
using System.Threading.Tasks;

namespace timelog.net.Data;

public class TaskRepository : BaseRepository<ProjectTask>
{
    public TaskRepository(ProjectContext context) : base(context.Tasks, () => context.SaveChangesAsync())
    {
    }

    public override async Task<ProjectTask?> GetById(int entityId) =>
        await DbSet.Where(t => t.TaskId == entityId).Include(t => t.Entries).FirstOrDefaultAsync();
}