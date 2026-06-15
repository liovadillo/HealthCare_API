using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Enums;

namespace HealthCare_API.DTOs.Appointment
{
    public class AppointmentDetailDTO
    {
        public int Id { get; set; }
        public DoctorSummaryDTO? Doctor { get; set; }
        public PatientSummaryDTO? Patient { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
