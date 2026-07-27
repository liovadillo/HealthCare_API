using HealthCare_API.DTOs.Appointment;
using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.Entities;
using HealthCare_API.Enums;

namespace HealthCare_API.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDetailDTO> InsertAsync(CreateAppointmentDTO dto);
        Task<AppointmentDetailDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDTO>> GetAllAsync();
        Task<IEnumerable<AppointmentDTO>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<AppointmentDTO>> GetByPatientAsync(int patientId);
        Task<AppointmentDetailDTO> UpdateAsync(int id, UpdateAppointmentDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<AppointmentDetailDTO> UpdateStatusAsync(int id, AppointmentStatus status);
        Task<PaginationResponseDTO<AppointmentDTO>> GetByPage(int pageNumber, int pageSize);
    }
}
