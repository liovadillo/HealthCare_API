using AutoMapper;
using HealthCare_API.DTOs.Appointment;
using HealthCare_API.Entities;
using HealthCare_API.Exceptions;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Interfaces;

namespace HealthCare_API.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IAppointmentRepository repository, IMapper mapper, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }
                
        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await GetAppointmentOrThrowAsync(id);
            await _repository.DeleteAsync(appointment);

            return true;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAsync()
        {
            var appointments = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetByDoctorAsync(int doctorId)
        {
            await ValidateDoctorExistAsync(doctorId);
            var appointments = await _repository.GetByDoctorAsync(doctorId);

            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }

        public async Task<AppointmentDetailDTO?> GetByIdAsync(int id)
        {
            var appointment = await GetAppointmentOrThrowAsync(id);

            return _mapper.Map<AppointmentDetailDTO>(appointment);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetByPatientAsync(int patientId)
        {
            await ValidatePatientExistAsync(patientId);
            var appointments = await _repository.GetByPatientAsync(patientId);

            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }

        public async Task<AppointmentDetailDTO> InsertAsync(CreateAppointmentDTO dto)
        {
            await ValidateEntitiesExistAsync(dto.DoctorId, dto.PatientId);
            await ValidateAppointmentOverlapAsync(dto.DoctorId, dto.PatientId, dto.StartTime, dto.EndTime);           

            var appointment = await _repository.InsertAsync(_mapper.Map<Appointment>(dto));

            return await GetAppointmentDetailsAsync(appointment.Id);

        }

        public async Task<AppointmentDetailDTO> UpdateAsync(int id, AppointmentDTO dto)
        {
            var appointment = await GetAppointmentOrThrowAsync(id);
            await ValidateEntitiesExistAsync(dto.DoctorId, dto.PatientId);
            await ValidateAppointmentOverlapAsync(dto.DoctorId, dto.PatientId, dto.StartTime, dto.EndTime);

            _mapper.Map(dto, appointment);

            await _repository.UpdateAsync(appointment);

            return await GetAppointmentDetailsAsync(appointment.Id);
        }


        private async Task<Appointment> GetAppointmentOrThrowAsync(int id) 
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Appointment ID: {id} not found.");

            return appointment;        
        }

        private async Task ValidateEntitiesExistAsync(int doctorId, int patientId)
        {
            await ValidateDoctorExistAsync(doctorId);
            await ValidatePatientExistAsync(patientId);
        }

        private async Task ValidateDoctorExistAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
                throw new NotFoundException($"Doctor ID: {id} not found.");
        }

        private async Task ValidatePatientExistAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
                throw new NotFoundException($"Patient ID: {id} not found.");
        }

        private async Task ValidateAppointmentOverlapAsync(int doctorId, int patientId, DateTime startTime, DateTime endTime)
        {
            var doctorHasOverlap = await _repository.ExistAsync(a =>
            a.DoctorId == doctorId &&
            a.StartTime < endTime &&
            a.EndTime > startTime);

            if (doctorHasOverlap)
                throw new BadRequestException("Doctor already has an appointment in that time slot.");

            var patientHasOverlap = await _repository.ExistAsync(a =>
            a.PatientId == patientId &&
            a.StartTime < endTime &&
            a.EndTime > startTime);

            if (patientHasOverlap)
                throw new BadRequestException("Patient already has an appointment in that time slot.");

        }

        private async Task<AppointmentDetailDTO> GetAppointmentDetailsAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);

            return _mapper.Map<AppointmentDetailDTO>(appointment);
        }
    }
}
