namespace HealthCare.Business.Models
{
    public class MedicalSchedule : Entity
    {
        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsAvailable { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}