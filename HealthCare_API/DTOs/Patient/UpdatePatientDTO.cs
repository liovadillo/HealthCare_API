namespace HealthCare_API.DTOs.Patient
{
    public class UpdatePatientDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
