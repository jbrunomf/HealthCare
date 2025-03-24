using HealthCare.Business.Interfaces;
using HealthCare.Business.Notifications;
using HealthCare.Business.Services;
using HealthCare.Data.Context;
using HealthCare.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<INotifier, Notifier>();
        //services.AddScoped<IPatientService, PatientService>();
        //services.AddScoped<IPatientRepository, PatientRepository>();

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AppDbContext>();


        return services;
    }
}