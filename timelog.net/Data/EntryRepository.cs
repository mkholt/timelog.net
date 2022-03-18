using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;

namespace timelog.net.Data;

public class EntryRepository : BaseRepository<Entry>
{
    public EntryRepository(ProjectContext context) : base(context.Entries, () => context.SaveChangesAsync())
    {
    }

    public override async Task<Entry?> GetById(int entityId) =>
        await DbSet.Where(t => t.EntryId == entityId).FirstOrDefaultAsync();
}