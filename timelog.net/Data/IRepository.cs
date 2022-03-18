using System.Collections.Generic;
using System.Threading.Tasks;
using timelog.net.Models;

namespace timelog.net.Data;

public interface IRepository<TType>
{
    Task<TType?> GetById(int entityId);

    Task<IEnumerable<TType>> GetAll();

    Task<TType?> Add(TType entry);

    /// <summary>
    /// Update the entity by the given ID,
    /// Returns true on success
    /// If the entity does not exist, returns false
    /// Throws exception on fault
    /// </summary>
    /// <param name="entityId">The ID of the entity to update</param>
    /// <param name="newEntry">The fields to update on the entity</param>
    /// <returns>TRUE on success, FALSE if entity not found</returns>
    Task<bool> Update(int entityId, TType newEntry);
    
    Task<bool> Remove(int entityId);
}