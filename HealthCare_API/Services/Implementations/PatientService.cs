using AutoMapper;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Entities;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Interfaces;

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
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PatientDTO>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDTO>>(patients);
        }

        public async Task<PatientDTO?> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            return _mapper.Map<PatientDTO>(patient);
        }

        public async Task<PatientDTO> InsertAsync(CreatePatientDTO dto)
        {
            var patient = await _repository.InsertAsync(_mapper.Map<Patient>(dto));
            return _mapper.Map<PatientDTO>(patient);
        }

        public async Task<PatientDTO?> UpdateAsync(int id, UpdatePatientDTO dto)
        {
            var patient = await _repository.UpdateAsync(id, _mapper.Map<Patient>(dto));

            if (patient == null)
                return null;

            return _mapper.Map<PatientDTO>(patient);
        }
    }
}
