using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace timelog.net.Data
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected BaseRepository(DbSet<TEntity> dbSet, Func<Task<int>> saveChanges)
        {
            DbSet = dbSet;
            _saveChanges = saveChanges;
        }

        public DbSet<TEntity> DbSet { get; }
        private readonly Func<Task<int>> _saveChanges;

        public abstract Task<TEntity?> GetById(int entityId);

        public async Task<IEnumerable<TEntity>> GetAll() =>
            await DbSet.ToListAsync();

        public async Task<TEntity?> Add(TEntity entity)
        {
            var entry = await DbSet.AddAsync(entity);
            await _saveChanges();
            return entry.Entity;
        }

        public Task<bool> Update(int entityId, TEntity patch)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Remove(int entityId)
        {
            var entry = await DbSet.FindAsync(entityId);
            if (entry is null) return false;
            DbSet.Remove(entry);
            return await _saveChanges() > 0;
        }
    }
}
