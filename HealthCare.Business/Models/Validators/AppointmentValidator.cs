using FluentValidation;

namespace HealthCare.Business.Models.Validators
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required.");
            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("Patient ID is required.");
            RuleFor(a => a.AppointmentDate.Date)
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Appointment date must be in the future.");
            RuleFor(a => a.Status)
                .IsInEnum().WithMessage("Invalid appointment status.");
            RuleFor(a => a.MedicalScheduleId)
                .NotEmpty().WithMessage("Medical schedule ID is required.");
        }
    }
}