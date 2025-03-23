using System.Linq.Expressions;
using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<int> CreateAsync(TEntity entity);
        Task<TEntity?> GetById(Guid id);
        Task<List<TEntity>> GetAll();
        Task Update(TEntity entity);
        Task Remove(Guid id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
    }
}