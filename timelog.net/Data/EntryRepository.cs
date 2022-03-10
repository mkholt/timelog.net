using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;

namespace timelog.net.Data;

public class EntryRepository : IEntryRepository
{
    private ProjectContext _context;
    
    public EntryRepository(ProjectContext context) => _context = context;
    
    public Task<Entry?> GetById(int entityId)
    {
        throw new NotImplementedException();
    }

    public Task<Entry?> GetActive(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Entry>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Entry?> Add(Entry? entry)
    {
        try
        {
            var outEntry = await _context.Entries.AddAsync(entry);
            await _context.SaveChangesAsync();
            return outEntry.Entity;
        }
        catch (DbUpdateException)
        {
            // TODO: Should we log this?
            return null;
        }
    }

    public Task<bool> Update(int entityId, Entry newEntry)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Remove(int entityId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Remove(int taskId, int entryId)
    {
        throw new NotImplementedException();
    }
}