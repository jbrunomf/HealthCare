using FluentValidation;
using FluentValidation.Results;
using HealthCare.Business.Interfaces;
using HealthCare.Business.Models;
using HealthCare.Business.Notifications;

namespace HealthCare.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors) Notify(error.ErrorMessage);
        }

        protected void Notify(string message)
        {
            _notifier.Handle(new Notification { Message = message });
        }

        protected bool Validate<TValidate, TEntity>(TValidate validate, TEntity entity)
            where TValidate : AbstractValidator<TEntity>
            where TEntity : Entity
        {
            var validationResult = validate.Validate(entity);

            if (validationResult.IsValid)
                return true;

            //Notify validation errors.
            foreach (var result in validationResult.Errors)
            {
                _notifier.Handle(new Notification()
                {
                    Message = result.ErrorMessage
                });
            }

            return false;
        }
    }
}