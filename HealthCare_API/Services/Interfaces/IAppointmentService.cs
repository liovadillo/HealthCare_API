using HealthCare_API.DTOs.Appointment;
using HealthCare_API.Entities;

namespace HealthCare_API.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDetailDTO> InsertAsync(CreateAppointmentDTO dto);
        Task<AppointmentDetailDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDTO>> GetAllAsync();
        Task<IEnumerable<AppointmentDTO>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<AppointmentDTO>> GetByPatientAsync(int patientId);
        Task<AppointmentDetailDTO> UpdateAsync(int id, AppointmentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
