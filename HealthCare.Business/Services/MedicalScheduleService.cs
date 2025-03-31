using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;

namespace HealthCare.Business.Services
{
    public class MedicalScheduleService : BaseService
    {
        private readonly IMedicalScheduleRepository _medicalScheduleRepository;

        public MedicalScheduleService(IMedicalScheduleRepository repository, INotifier notifier) : base(notifier)
        {
            _medicalScheduleRepository = repository;
        }

        public async Task<IEnumerable<MedicalSchedule>> GetAllAsync()
        {
            return await _medicalScheduleRepository.GetAllAsync();
        }

        public async Task<MedicalSchedule> GetByIdAsync(Guid id)
        {
            return await _medicalScheduleRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(MedicalSchedule medicalSchedule)
        {
            await _medicalScheduleRepository.AddAsync(medicalSchedule);
        }

        public async Task UpdateAsync(MedicalSchedule medicalSchedule)
        {
            await _medicalScheduleRepository.UpdateAsync(medicalSchedule);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _medicalScheduleRepository.DeleteAsync(id);
        }

        public async Task<List<MedicalSchedule>> GetAvailableSlotsAsync(Guid doctorId, DateTime date)
        {
            return await _medicalScheduleRepository.GetAvailableSlots(doctorId, date);
        }

        public async Task<bool> BookAppointmentAsync(Guid scheduleId, Guid patientId)
        {
            return await _medicalScheduleRepository.BookAppointment(scheduleId, patientId);
        }
    }
}