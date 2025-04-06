//using HealthCare.Business.Interfaces;
//using HealthCare.Business.Models;
//using HealthCare.Business.Models.Validators;

//namespace HealthCare.Business.Services
//{
//    public class DoctorService : BaseService, idoctor
//    {
//        private readonly IDoctorRepository _repository;

//        public DoctorService(IDoctorRepository repository, INotifier notifier) : base(notifier)
//        {
//            {
//                _repository = repository;
//            }
//        }

//        public void Dispose()
//        {
//            _repository?.Dispose();
//        }

//        public async Task CreateDoctorAsync(Doctor doctor)
//        {
//            if (!Validate(new DoctorValidator(), doctor)) return;

//            if (_repository.Find(x => x.Document == doctor.Document).Result.Any())
//            {
//                Notify("Já existe um médico cadastrado com o documento informado.");
//                return;
//            }

//            await _repository.CreateAsync(doctor);
//        }

//        public async Task UpdateDoctorAsync(Doctor doctor)
//        {
//            if (!Validate(new DoctorValidator(), doctor)) return;

//            await _repository.Update(doctor);
//        }

//        public async Task DeleteDoctorAsync(Guid id)
//        {
//            await _repository.Remove(id);
//        }
//    }
//}