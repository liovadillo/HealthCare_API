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

        public async Task<Patient?> UpdateAsync(int id, Patient patient)
        {
            var existing = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return null;

            existing.Name = patient.Name;
            existing.Age = patient.Age;
            existing.Diagnosis = patient.Diagnosis;
            existing.IsActive = patient.IsActive;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
