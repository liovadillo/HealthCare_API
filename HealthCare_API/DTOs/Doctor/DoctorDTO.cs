namespace HealthCare_API.DTOs.Doctor
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public bool IsAvailable { get; set; }
        public int YearsOfExperience { get; set; }
    }
}
