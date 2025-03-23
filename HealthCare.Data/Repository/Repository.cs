using System.Linq.Expressions;
using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);

            return await _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity?> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remove(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });

            await SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}