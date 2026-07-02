using AutoMapper;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Entities;
using HealthCare_API.Exceptions;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Interfaces;
using System.Numerics;

namespace HealthCare_API.Services.Implementations
{
    public class PatientService : IPatientService
    {

        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await GetPatientOrThrowAsync(id);

            await _repository.DeleteAsync(patient);

            return true;

        }

        public async Task<IEnumerable<PatientDTO>> GetAllActiveAsync()
        {
            var patients = await _repository.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<PatientDTO>>(patients);
        }

        public async Task<IEnumerable<PatientDTO>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDTO>>(patients);
        }

        public async Task<PatientDTO?> GetByIdAsync(int id)
        {
            var patient = await GetPatientOrThrowAsync(id);

            return _mapper.Map<PatientDTO>(patient);
        }

        public async Task<PatientDTO> InsertAsync(CreatePatientDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                throw new BadRequestException("Patient name is required");

            if (dto.Age <= 0)
                throw new BadRequestException("Age must be greater than 0");

            var patient = await _repository.InsertAsync(_mapper.Map<Patient>(dto));
            return _mapper.Map<PatientDTO>(patient);
        }

        public async Task<PatientDTO?> UpdateAsync(int id, UpdatePatientDTO dto)
        {
            var patient = await GetPatientOrThrowAsync(id);

            _mapper.Map(dto, patient);

            await _repository.UpdateAsync(patient);

            return _mapper.Map<PatientDTO>(patient);
        }

        private async Task<Patient> GetPatientOrThrowAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null)
                throw new NotFoundException($"Patient ID: {id} not found.");

            return patient;
        }
    }
}
