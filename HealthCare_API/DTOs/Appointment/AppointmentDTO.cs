using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Enums;

namespace HealthCare_API.DTOs.Appointment
{
    public class AppointmentDTO
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}
