using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.Patient;

namespace HealthCare_API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetAllAsync();
        Task<DoctorDTO?> GetByIdAsync(int id);
        Task<DoctorDTO> InsertAsync(CreateDoctorDTO dto);
        Task<DoctorDTO?> UpdateAsync(int id, UpdateDoctorDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
