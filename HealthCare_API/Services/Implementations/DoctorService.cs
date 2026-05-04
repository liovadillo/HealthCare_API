using AutoMapper;
using HealthCare_API.DTOs.Doctor;
using HealthCare_API.Entities;
using HealthCare_API.Exceptions;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Interfaces;

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
            var wasDeleted = await _repository.DeleteAsync(id);
            if(!wasDeleted)
                throw new NotFoundException($"Doctor ID: {id} not found.");

            return wasDeleted;

        }

        public async Task<IEnumerable<DoctorDTO>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
        }

        public async Task<DoctorDTO?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null)
                throw new NotFoundException($"Doctor ID: {id} not found.");

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
            var doctor = await _repository.UpdateAsync(id, _mapper.Map<Doctor>(dto));

            if (doctor == null)
                throw new NotFoundException($"Doctor ID: {id} not found.");

            return _mapper.Map<DoctorDTO>(doctor);
        }
    }
}
