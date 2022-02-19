using System.Collections.Generic;
using System.Threading.Tasks;

namespace timelog.net.Data;

public interface IRepository<TType>
{
    Task<TType?> GetById(int id);

    Task<IEnumerable<TType>> GetAll();

    Task<TType> Add(TType entry);
    
    Task<bool> Update(int id, TType newEntry);
    
    Task<bool> Remove(int id);
}