using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace HealthCare.Business.Models.Validators
{
    class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(patient => patient.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(patient => patient.LastName).NotEmpty().MaximumLength(100);
            RuleFor(patient => patient.Document).NotEmpty().MaximumLength(11);
            RuleFor(patient => patient.DateOfBirth).NotEmpty();
            RuleFor(patient => patient.Email).EmailAddress().MaximumLength(100);
        }
    }
}
