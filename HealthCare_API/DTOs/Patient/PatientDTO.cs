namespace HealthCare_API.DTOs.Patient
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
        public bool IsActive { get; set; }
    }
}
