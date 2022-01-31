using System.Collections.Generic;
using System.Threading.Tasks;

namespace timelog.net.Data;

public interface IRepository<TType>
{
    Task<TType?> GetById(int id);

    Task<IEnumerable<TType>> GetAll();

    Task Add(TType entry);
    Task Update(TType newEntry);
    Task Remove(TType entry);
    Task Remove(int id);

    Task SaveChanges();
}