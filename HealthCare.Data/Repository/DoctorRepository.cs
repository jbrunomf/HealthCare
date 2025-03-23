using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Repository
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialty(Guid specialtyId)
        {
            return await _context.Doctors
                .AsNoTracking()
                .Where(d => d.SpecialtyId == specialtyId)
                .ToListAsync();
        }
    }
}