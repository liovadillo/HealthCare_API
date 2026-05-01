using HealthCare_API.Data;
using HealthCare_API.Entities;
using HealthCare_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthCare_API.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthCareDbContext _dbContext;
        public DoctorRepository(HealthCareDbContext dbContext) { 

            _dbContext = dbContext;
        
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return false;

            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _dbContext.Doctors.ToListAsync();            
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
             
        }

        public async Task<Doctor> InsertAsync(Doctor doctor)
        {
            await _dbContext.Doctors.AddAsync(doctor);
            await _dbContext.SaveChangesAsync();

            return doctor;
        }

        public async Task<Doctor?> UpdateAsync(int id, Doctor doctor)
        {
            var existing = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (existing == null)
                return null;

            existing.Name = doctor.Name;
            existing.Specialty = doctor.Specialty;
            existing.YearsOfExperience = doctor.YearsOfExperience;
            existing.IsAvailable = doctor.IsAvailable;

            await _dbContext.SaveChangesAsync();

            return existing;
        }
    }
}
