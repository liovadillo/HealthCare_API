namespace HealthCare_API.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
        public bool IsActive { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
