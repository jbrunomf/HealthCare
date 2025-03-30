using System.ComponentModel;

namespace HealthCare.Business.Models
{
    public class Appointment : Entity
    {
        [DisplayName("Doctor")]
        public Guid DoctorId { get; set; }

        [DisplayName("Doctor")]
        public Doctor? Doctor { get; set; }

        [DisplayName("Patient")]
        public Guid PatientId { get; set; }
        [DisplayName("Patient")]
        public Patient? Patient { get; set; }

        [DisplayName("Date")] public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;


        public enum AppointmentStatus
        {
            [Description("Scheduled")] Scheduled = 1,
            [Description("Cancelled")] Cancelled = 2,
            [Description("Completed")] Completed = 3
        }
    }
}