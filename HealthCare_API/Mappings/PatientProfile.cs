using AutoMapper;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Entities;

namespace HealthCare_API.Mappings
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient, PatientDTO>();
            CreateMap<CreatePatientDTO, Patient>();
            CreateMap<UpdatePatientDTO, Patient>();
        }
    }
}
