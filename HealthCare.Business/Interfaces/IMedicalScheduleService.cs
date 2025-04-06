using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IMedicalScheduleService : IDisposable
    {
        Task<MedicalSchedule> GetMedicalScheduleByIdAsync(Guid id);
        Task<bool> CreateAsync(MedicalSchedule medicalSchedule);
        Task<bool> UpdateAsync(MedicalSchedule medicalSchedule);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkAsUnavailable(MedicalSchedule schedule);
        Task<bool> MarkAsAvailable(MedicalSchedule schedule);

        Task<MedicalSchedule> FindAsync(Guid id);

        Task<MedicalSchedule?> GetLastValidScheduleForAppointment(Appointment appointment);
    }
}