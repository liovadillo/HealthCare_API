using AutoMapper;
using HealthCare_API.DTOs.Doctor;
using HealthCare_API.Entities;
using HealthCare_API.Exceptions;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace HealthCare_API.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        
        public DoctorService(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await GetDoctorOrThrowAsync(id);

            await _repository.DeleteAsync(doctor);

            return true;

        }

        public async Task<IEnumerable<DoctorDTO>> GetAllActiveAsync()
        {
            var doctors = await _repository.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
        }

        public async Task<DoctorDTO?> GetByIdAsync(int id)
        {
            var doctor = await GetDoctorOrThrowAsync(id);

            return _mapper.Map<DoctorDTO>(doctor);
        }

        public async Task<DoctorDTO> InsertAsync(CreateDoctorDTO dto)
        {
            if(string.IsNullOrEmpty(dto.Name))
                throw new BadRequestException("Doctor name is required");

            if (string.IsNullOrEmpty(dto.Specialty))
                throw new BadRequestException("Specialty is required");

            if (dto.YearsOfExperience < 0)
                throw new BadRequestException("Years of experience cannot be negative");

            var doctor = await _repository.InsertAsync(_mapper.Map<Doctor>(dto));
            return _mapper.Map<DoctorDTO>(doctor);
        }

        public async Task<DoctorDTO?> UpdateAsync(int id, UpdateDoctorDTO dto)
        {
            var doctor = await GetDoctorOrThrowAsync(id);
            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);

            return _mapper.Map<DoctorDTO>(doctor);
        }

        private async Task<Doctor> GetDoctorOrThrowAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null)
                throw new NotFoundException($"Doctor ID: {id} not found.");

            return doctor;
        }
    }
}
