using System.ComponentModel.DataAnnotations;

namespace HealthCare.Business.Models
{
    public class MedicalSchedule : Entity
    {
        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [Display(Name = "Schedule Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Schedule End Time")]
        public DateTime EndTime { get; set; }

        public bool IsAvailable { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}