using System.Linq.Expressions;
using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;

namespace HealthCare.Business.Services
{
    public class DoctorService : BaseService, IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository, INotifier notifier) : base(notifier)
        {
            {
                _repository = repository;
            }
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        public async Task CreateAsync(Doctor doctor)
        {
            // Set creation timestamp
            doctor.CreatedAt = DateTime.UtcNow;
            doctor.UpdatedAt = DateTime.UtcNow;

            // Perform any necessary validation before creating the doctor
            // Assuming there might be a validator specific to Doctor
            // You may want to inject a specific validator if one exists
    
            // Call the repository's CreateAsync method
            await _repository.CreateAsync(doctor);
            await _repository.SaveChanges();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            // Update the timestamp for the update operation
            doctor.UpdatedAt = DateTime.UtcNow;

            // Perform any necessary validation before updating the doctor
            // You might want to add a specific validator for Doctor if one exists
    
            // Call the repository's Update method
            await _repository.Update(doctor);
            await _repository.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            // Remove the doctor by its ID using the repository
            await _repository.Remove(id);
    
            // Save changes to persist the deletion
            await _repository.SaveChanges();
        }

        public async Task<Doctor> FindAsync(Guid id)
        {
            // Check if the id is valid (non-empty Guid)
            if (id == Guid.Empty)
            {
                // Notify about invalid input
                Notify("Invalid doctor ID provided.");
                return null;
            }

            // Use the repository's GetById method to retrieve the doctor
            var doctor = await _repository.GetById(id);

            // If no doctor is found, notify and return null
            if (doctor == null)
            {
                Notify($"Doctor with ID {id} not found.");
                return null;
            }

            // Return the found doctor
            return doctor;
        }

        public Task<IEnumerable<Doctor>> Find(Expression<Func<Doctor, bool>> predicate)
        {
            return _repository.Find(predicate);
        }
    }
}

