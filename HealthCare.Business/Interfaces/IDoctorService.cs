using System.Linq.Expressions;
using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces;

public interface IDoctorService : IDisposable
{
    Task CreateAsync(Doctor doctor);
    Task UpdateAsync(Doctor doctor);
    Task DeleteAsync(Guid id);
    Task<Doctor> FindAsync(Guid id);

    Task<IEnumerable<Doctor>> Find(Expression<Func<Doctor, bool>> predicate);
}