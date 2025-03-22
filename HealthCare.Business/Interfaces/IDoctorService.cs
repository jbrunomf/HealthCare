using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IDoctorService : IDisposable
    {
        Task CreateDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(Guid id);
    }
}
