using HealthCare.Business.Interfaces;
using HealthCare.Business.Notifications;
using HealthCare.Business.Services;
using HealthCare.Data.Context;
using HealthCare.Data.Email;
using HealthCare.Data.Repository;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<IMedicalScheduleService, MedicalScheduleService>();
        services.AddScoped<IMedicalScheduleRepository, MedicalScheduleRepository>();

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        


        services.AddRazorPages();


        return services;
    }
}