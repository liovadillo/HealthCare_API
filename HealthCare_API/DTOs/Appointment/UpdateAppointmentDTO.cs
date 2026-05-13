using HealthCare_API.Enums;

namespace HealthCare_API.DTOs.Appointment
{
    public class UpdateAppointmentDTO
    {
        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
