using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Repository
{
    public class MedicalScheduleRepository : Repository<MedicalSchedule>, IMedicalScheduleRepository
    {
        public MedicalScheduleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public async Task<IEnumerable<MedicalSchedule>> GetAllAsync()
        {
            var schedules = await _context.MedicalSchedules.ToListAsync();

            return schedules;
        }

        public async Task<MedicalSchedule> GetByIdAsync(Guid id)
        {
            var schedule = await _context.MedicalSchedules.FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                throw new InvalidOperationException($"MedicalSchedule with id {id} not found.");
            }

            return schedule;
        }

        public async Task AddAsync(MedicalSchedule medicalSchedule)
        {
            if (medicalSchedule == null) throw new ArgumentNullException(nameof(medicalSchedule));

            await _context.MedicalSchedules.AddAsync(medicalSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MedicalSchedule medicalSchedule)
        {
            if (medicalSchedule == null) throw new ArgumentNullException(nameof(medicalSchedule));

            var existingSchedule = await _context.MedicalSchedules.FirstOrDefaultAsync(s => s.Id == medicalSchedule.Id);

            if (existingSchedule == null)
                throw new InvalidOperationException($"MedicalSchedule with id {medicalSchedule.Id} not found.");

            existingSchedule.DoctorId = medicalSchedule.DoctorId;
            existingSchedule.StartTime = medicalSchedule.StartTime;
            existingSchedule.EndTime = medicalSchedule.EndTime;
            existingSchedule.UpdatedAt = DateTime.UtcNow;
            existingSchedule.IsAvailable = medicalSchedule.IsAvailable;

            _context.MedicalSchedules.Update(existingSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var schedule = await _context.MedicalSchedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) throw new InvalidOperationException($"MedicalSchedule with id {id} not found.");

            _context.MedicalSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MedicalSchedule>> GetAvailableSlots(Guid doctorId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            var availableSlots = await _context.MedicalSchedules.Where(schedule =>
                    schedule.DoctorId == doctorId && schedule.StartTime >= startOfDay && schedule.EndTime <= endOfDay)
                .ToListAsync();

            return availableSlots;
        }
        
        public async Task<bool> MarkAsUnavailable(Guid scheduleId)
        {

            try
            {
                var existingSchedule = await _context.MedicalSchedules.FirstOrDefaultAsync(s => s.Id == scheduleId);
                if (existingSchedule == null)
                    throw new InvalidOperationException($"MedicalSchedule with id {scheduleId} not found.");

                if (!existingSchedule.IsAvailable) return false; // Already unavailable

                existingSchedule.IsAvailable = false;
                existingSchedule.UpdatedAt = DateTime.UtcNow;

                _context.MedicalSchedules.Update(existingSchedule);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> MarkAsAvailable(Guid scheduleId)
        {
            _context.ChangeTracker.Clear();
            
            var existingSchedule = await _context.MedicalSchedules.FirstOrDefaultAsync(s => s.Id == scheduleId);
            if (existingSchedule == null)
                throw new InvalidOperationException($"MedicalSchedule with id {scheduleId} not found.");
            
            if (existingSchedule.IsAvailable) return false; // Already available

            existingSchedule.IsAvailable = true;
            existingSchedule.UpdatedAt = DateTime.Now;

            _context.MedicalSchedules.Update(existingSchedule);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<MedicalSchedule> FindAsync(Guid id)
        {
            var schedule = await _context.MedicalSchedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) throw new InvalidOperationException($"MedicalSchedule with id {id} not found.");

            return schedule;
        }

        public async Task<MedicalSchedule?> GetLastValidScheduleForAppointment(Appointment appointment)
        {
            var lstSchedules =
                _context.MedicalSchedules.Where(s => s.Appointments.Contains(appointment));

            var pastSchedule = lstSchedules.FirstOrDefault(s => s.IsAvailable == false);

            return await Task.FromResult(pastSchedule);
        }
        
    }
}