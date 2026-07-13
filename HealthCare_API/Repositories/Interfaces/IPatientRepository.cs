using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.Entities;

namespace HealthCare_API.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<IEnumerable<Patient>> GetAllActiveAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient> InsertAsync(Patient patient);
        Task<Patient?> UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
        Task<PaginationResponseDTO<Patient>> GetByPage(int pageNumber, int pageSize);
    }
}
