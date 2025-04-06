using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace HealthCare.Business.Models.Validators
{
    public class ScheduleValidator : AbstractValidator<MedicalSchedule>
    {
        public ScheduleValidator()
        {
            RuleFor(v => v.StartTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start time must be in the future.");
            RuleFor(v => v.IsAvailable)
                .Equal(true).WithMessage("Schedule is not available");
        }
    }
}