using System.Linq.Expressions;
using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Repository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<Patient?> GetPatientByDocument(string document)
        {
            return await DbSet.FirstOrDefaultAsync(p => p.Document == document);
        }

        public async Task<Patient?> FindAsync(Guid id)
        {
            return await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

