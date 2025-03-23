using HealthCare.Business.Interfaces;
using HealthCare.Business.Notifications;
using HealthCare.Business.Services;
using HealthCare.Data.Repository;

namespace HealthCare.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<INotifier, Notifier>();
            //services.AddScoped<IPatientService, PatientService>();
            //services.AddScoped<IPatientRepository, PatientRepository>();


            return services;
        }
    }
}
