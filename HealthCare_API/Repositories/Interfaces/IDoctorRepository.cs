using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.Entities;

namespace HealthCare_API.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor> InsertAsync(Doctor doctor);
        Task<Doctor?> UpdateAsync(Doctor doctor);
        Task DeleteAsync(Doctor doctor);
        Task<IEnumerable<Doctor>> GetAllActiveAsync();
        Task<PaginationResponseDTO<Doctor>> GetByPage(int pageNumber, int pageSize);
    }
}
