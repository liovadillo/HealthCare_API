using HealthCare_API.Enums;

namespace HealthCare_API.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}
