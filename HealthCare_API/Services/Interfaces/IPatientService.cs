using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.DTOs.Patient;

namespace HealthCare_API.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDTO>> GetAllAsync();
        Task<IEnumerable<PatientDTO>> GetAllActiveAsync();
        Task<PatientDTO?> GetByIdAsync(int id);
        Task<PatientDTO> InsertAsync(CreatePatientDTO dto);
        Task<PatientDTO?> UpdateAsync(int id, UpdatePatientDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<PaginationResponseDTO<PatientDTO>> GetByPage(int pageNumber, int pageSize);
    }
}
