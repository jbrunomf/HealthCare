using HealthCare.Business.Models;

namespace HealthCare.Business.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient> GetPatientByDocument(string document);
    }
}
