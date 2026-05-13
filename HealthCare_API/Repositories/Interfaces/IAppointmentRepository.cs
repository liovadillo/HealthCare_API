using HealthCare_API.Entities;
using System.Linq.Expressions;

namespace HealthCare_API.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId);
        Task<Appointment> InsertAsync(Appointment appointment);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);
        Task<bool> ExistAsync(Expression<Func<Appointment, bool>> predicate);


    }
}
