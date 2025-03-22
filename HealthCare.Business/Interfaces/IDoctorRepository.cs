using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetDoctorsBySpecialty(Guid specialtyId);
    }
}
