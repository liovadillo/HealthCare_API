using HealthCare_API.Entities;

namespace HealthCare_API.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor> InsertAsync(Doctor doctor);
        Task<Doctor?> UpdateAsync(int id, Doctor doctor);
        Task<bool> DeleteAsync(int id);
    }
}
