using AutoMapper;
using HealthCare_API.DTOs.Doctor;
using HealthCare_API.Entities;

namespace HealthCare_API.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<Doctor, DoctorSummaryDTO>();
            CreateMap<CreateDoctorDTO, Doctor>();
            CreateMap<UpdateDoctorDTO, Doctor>();
        }
    }
}
