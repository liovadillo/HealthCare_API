using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCare_Client.DTOs.Patient
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
