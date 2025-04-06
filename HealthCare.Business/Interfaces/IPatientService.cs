using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IPatientService : IDisposable
    {
        Task CreatePatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(Guid id);
        Task<Patient?> FindAsync(string userId);
    }
}
