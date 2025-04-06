using System.Linq.Expressions;
using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetPatientByDocument(string document);

        Task<Patient?> FindAsync(string userId);
    }

}
