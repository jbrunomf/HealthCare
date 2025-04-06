using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IAppointmentService : IDisposable
    {
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(Guid id);
        Task<Appointment> FindAsync(Guid id);
        Task MarkAsCancelled(Appointment appointment);
    }
}