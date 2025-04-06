using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IAppointmentRepository : IDisposable
    {
        Task CreateAsync(Appointment appointment);
        Task Update(Appointment appointment);
        Task DeleteAsync(Guid id);
        Task<Appointment?> FindAsync(Guid id);
    }
}
