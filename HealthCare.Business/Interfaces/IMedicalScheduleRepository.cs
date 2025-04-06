using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IMedicalScheduleRepository
    {
        Task<IEnumerable<MedicalSchedule>> GetAllAsync();
        Task<MedicalSchedule> GetByIdAsync(Guid id);
        Task AddAsync(MedicalSchedule medicalSchedule);
        Task UpdateAsync(MedicalSchedule medicalSchedule);
        Task DeleteAsync(Guid id);
        Task<List<MedicalSchedule>> GetAvailableSlots(Guid doctorId, DateTime date);
        Task<bool> BookAppointment(Guid scheduleId, Guid patientId);
        Task<bool> MarkAsUnavailable(Guid scheduleId);
        Task<bool> MarkAsAvailable(Guid scheduleId);
        Task<MedicalSchedule> FindAsync(Guid id);

        Task<MedicalSchedule?> GetLastValidScheduleForAppointment(Appointment appointment);
    }
}