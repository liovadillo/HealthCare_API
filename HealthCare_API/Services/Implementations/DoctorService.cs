using AutoMapper;
using HealthCare_API.DTOs.Doctor;
using HealthCare_API.Entities;
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
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
        }

        public async Task<DoctorDTO?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            return _mapper.Map<DoctorDTO>(doctor);
        }

        public async Task<DoctorDTO> InsertAsync(CreateDoctorDTO dto)
        {
            var doctor = await _repository.InsertAsync(_mapper.Map<Doctor>(dto));
            return _mapper.Map<DoctorDTO>(doctor);
        }

        public async Task<DoctorDTO?> UpdateAsync(int id, UpdateDoctorDTO dto)
        {
            var doctor = await _repository.UpdateAsync(id, _mapper.Map<Doctor>(dto));

            if (doctor == null)
                return null;

            return _mapper.Map<DoctorDTO>(doctor);
        }
    }
}
