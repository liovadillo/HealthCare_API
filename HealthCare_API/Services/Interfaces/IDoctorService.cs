using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Entities;

namespace HealthCare_API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetAllAsync();
        Task<IEnumerable<DoctorDTO>> GetAllActiveAsync();
        Task<DoctorDTO?> GetByIdAsync(int id);
        Task<DoctorDTO> InsertAsync(CreateDoctorDTO dto);
        Task<DoctorDTO?> UpdateAsync(int id, UpdateDoctorDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<PaginationResponseDTO<DoctorDTO>> GetByPage(int pageNumber, int pageSize);
    }
}
