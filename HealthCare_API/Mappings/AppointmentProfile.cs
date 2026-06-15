using AutoMapper;
using HealthCare_API.DTOs.Appointment;
using HealthCare_API.Entities;

namespace HealthCare_API.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile() { 
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor!.Name))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient!.Name))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore());

            CreateMap<Appointment , AppointmentDetailDTO>()
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient));

            CreateMap<CreateAppointmentDTO, Appointment>();

            CreateMap<UpdateAppointmentDTO, Appointment>();
        }
    }
}
