using System.Linq.Expressions;
using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Repository
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task CreateAsync(Appointment appointment)
        {
            if (appointment == null) throw new ArgumentNullException(nameof(appointment));

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public override async Task Update(Appointment appointment)
        {
            if (appointment == null) throw new ArgumentNullException(nameof(appointment));

            appointment.UpdatedAt = DateTime.UtcNow;
            await base.Update(appointment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await base.Remove(id);
        }

        public async Task<Appointment?> FindAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid ID.", nameof(id));

            return await DbSet.Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalSchedule)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}