using HealthCare_API.Data;
using HealthCare_API.Entities;
using HealthCare_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthCare_API.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthCareDbContext _context;

        public PatientRepository(HealthCareDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient> InsertAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient?> UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
