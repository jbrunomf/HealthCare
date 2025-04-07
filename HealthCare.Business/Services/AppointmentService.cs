using System.Linq.Expressions;
using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Business.Models.Validators;

namespace HealthCare.Business.Services
{
    public class AppointmentService : BaseService, IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMedicalScheduleRepository _medicalScheduleRepository;

        public AppointmentService(IAppointmentRepository repository,  IMedicalScheduleRepository medicalScheduleRepository, INotifier notifier) : base(notifier)
        {
            {
                _repository = repository;
                _medicalScheduleRepository = medicalScheduleRepository;
            }
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        public async Task CreateAsync(Appointment appointment)
        {
            if (appointment == null)
            {
                Notify("Invalid appointment data.");
                return;
            }

            if (!Validate(new AppointmentValidator(), appointment)) return;

            appointment.CreatedAt = DateTime.Now;
            appointment.UpdatedAt = DateTime.Now;

            // Add the appointment to the repository
            await _repository.CreateAsync(appointment);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            if (appointment == null)
            {
                Notify("Invalid appointment data.");
                return;
            }

            if (!Validate(new AppointmentValidator(), appointment)) return;

            var existingAppointment = await _repository.FindAsync(appointment.Id);
            if (existingAppointment == null)
            {
                Notify("Appointment not found.");
                return;
            }

            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.CreatedAt = existingAppointment.CreatedAt;

            await _repository.Update(appointment);
        }

        public async Task MarkAsCancelled(Appointment appointment)
        {
            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            await _repository.Update(appointment);
        }

        public Task<Appointment> Find(Expression<Func<Appointment, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var appointment = await _repository.FindAsync(id);
            if (appointment == null)
            {
                Notify("Appointment not found.");
                return;
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<Appointment?> FindAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                Notify("Invalid appointment ID.");
                return null;
            }

            var appointment = await _repository.FindAsync(id);
            var medicalSchedule = await _medicalScheduleRepository.FindAsync(appointment.MedicalScheduleId);

            if (appointment == null || medicalSchedule == null)
            {
                Notify("Appointment not found.");
                return null;
            }
            
            appointment.MedicalSchedule = medicalSchedule;
            return appointment;
        }
        
    }
}