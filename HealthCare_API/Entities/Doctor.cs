namespace HealthCare_API.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public bool IsAvailable { get; set; }
        public int YearsOfExperience { get; set; }
    }
}
