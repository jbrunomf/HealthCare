using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Business.Models.Validators;

namespace HealthCare.Business.Services
{
    public class MedicalScheduleService : BaseService, IMedicalScheduleService
    {
        private readonly IMedicalScheduleRepository _medicalScheduleRepository;

        public MedicalScheduleService(IMedicalScheduleRepository repository, INotifier notifier) : base(notifier)
        {
            _medicalScheduleRepository = repository;
        }


        public async Task<MedicalSchedule> GetMedicalScheduleByIdAsync(Guid id)
        {
            var medicalSchedule = await _medicalScheduleRepository.GetByIdAsync(id);

            if (medicalSchedule == null)
            {
                Notify("Medical schedule not found.");
                return null;
            }

            return medicalSchedule;
        }

        public async Task<bool> CreateAsync(MedicalSchedule medicalSchedule)
        {
            if (medicalSchedule == null)
            {
                Notify("Medical schedule cannot be null.");
                return false;
            }

            if (medicalSchedule.StartTime >= medicalSchedule.EndTime)
            {
                Notify("Start time must be earlier than end time.");
                return false;
            }

            var overlappingSchedules =
                await _medicalScheduleRepository.GetAvailableSlots(medicalSchedule.DoctorId,
                    medicalSchedule.StartTime.Date);
            if (overlappingSchedules.Any(schedule =>
                    medicalSchedule.StartTime < schedule.EndTime && medicalSchedule.EndTime > schedule.StartTime))
            {
                Notify("The specified time slot overlaps with an existing schedule.");
                return false;
            }

            medicalSchedule.CreatedAt = DateTime.UtcNow;
            medicalSchedule.UpdatedAt = DateTime.UtcNow;

            await _medicalScheduleRepository.AddAsync(medicalSchedule);
            return true;
        }

        public async Task<bool> UpdateAsync(MedicalSchedule medicalSchedule)
        {
            if (medicalSchedule == null)
            {
                Notify("Medical schedule cannot be null.");
                return false;
            }

            var existingSchedule = await _medicalScheduleRepository.GetByIdAsync(medicalSchedule.Id);
            if (existingSchedule == null)
            {
                Notify("Medical schedule not found.");
                return false;
            }

            if (medicalSchedule.StartTime >= medicalSchedule.EndTime)
            {
                Notify("Start time must be earlier than end time.");
                return false;
            }

            var overlappingSchedules =
                await _medicalScheduleRepository.GetAvailableSlots(medicalSchedule.DoctorId,
                    medicalSchedule.StartTime.Date);
            if (overlappingSchedules.Any(schedule =>
                    schedule.Id != medicalSchedule.Id && medicalSchedule.StartTime < schedule.EndTime &&
                    medicalSchedule.EndTime > schedule.StartTime))
            {
                Notify("The specified time slot overlaps with an existing schedule.");
                return false;
            }

            existingSchedule.StartTime = medicalSchedule.StartTime;
            existingSchedule.EndTime = medicalSchedule.EndTime;
            existingSchedule.IsAvailable = medicalSchedule.IsAvailable;
            existingSchedule.UpdatedAt = DateTime.UtcNow;

            await _medicalScheduleRepository.UpdateAsync(existingSchedule);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medicalSchedule = await _medicalScheduleRepository.GetByIdAsync(new Guid(id.ToString()));

            if (medicalSchedule == null)
            {
                Notify("Medical schedule not found.");
                return false;
            }

            await _medicalScheduleRepository.DeleteAsync(medicalSchedule.Id);
            return true;
        }

        public async Task<bool> MarkAsUnavailable(MedicalSchedule schedule)
        {
            if (schedule == null)
            {
                Notify("Invalid schedule.");
                return false;
            }

            var existingSchedule = await _medicalScheduleRepository.GetByIdAsync(schedule.Id);
            if (existingSchedule == null)
            {
                Notify("Schedule not found.");
                return false;
            }

            if (!Validate(new ScheduleValidator(), existingSchedule))
            {
                return false;
            }

            existingSchedule.IsAvailable = false;
            existingSchedule.UpdatedAt = DateTime.UtcNow;

            await _medicalScheduleRepository.UpdateAsync(existingSchedule);
            return true;
        }

        public async Task<bool> MarkAsAvailable(MedicalSchedule schedule)
        {
            if (schedule == null)
            {
                Notify("The schedule cannot be null.");
                return false;
            }
            
            schedule.IsAvailable = true;
            schedule.UpdatedAt = DateTime.UtcNow;

            await _medicalScheduleRepository.UpdateAsync(schedule);
            return true;
        }

        public async Task<MedicalSchedule> FindAsync(Guid id)
        {
            return await _medicalScheduleRepository.GetByIdAsync(id);
        }

        public async Task<MedicalSchedule?> GetLastValidScheduleForAppointment(Appointment appointment)
        {
            return await _medicalScheduleRepository.GetLastValidScheduleForAppointment(appointment);
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing..");
        }
    }
}