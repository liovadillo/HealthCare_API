using HealthCare_API.Repositories.Implementations;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Implementations;
using HealthCare_API.Services.Interfaces;

namespace HealthCare_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();

        }
    }
}
