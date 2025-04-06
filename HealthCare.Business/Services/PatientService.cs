using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Business.Models.Validators;

namespace HealthCare.Business.Services
{
    public class PatientService : BaseService, IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository, INotifier notifier) : base(notifier)
        {
            _repository = repository;
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        public async Task CreatePatientAsync(Patient patient)
        {
            if (!Validate(new PatientValidator(), patient)) return;
            if (_repository.Find(x => x.Document == patient.Document && x.Id != patient.Id).Result.Any())
            {
                Notify("Já existe um paciente cadastrado com esse documento.");
                return;
            }

            await _repository.CreateAsync(patient);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            if (!Validate(new PatientValidator(), patient)) return;
            await _repository.Update(patient);
        }

        public async Task DeletePatientAsync(Guid id)
        {
            var patient = _repository.GetById(id);

            if (patient == null)
            {
                Notify("O paciente não foi encontrado.");
                return;
            }


            await _repository.Remove(id);
        }

        public async Task<Patient?> FindAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                Notify("Invalid patient identifier.");
                return null;
            }

            var patient = await _repository.FindAsync(userId);

            if (patient == null) Notify("Patient not found.");

            return patient;
        }
    }
}