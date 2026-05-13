using HealthCare_API.Data;
using HealthCare_API.Entities;
using HealthCare_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace HealthCare_API.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthCareDbContext _context;
        
        public AppointmentRepository(HealthCareDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return await _context.Appointments.AnyAsync(predicate);
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Appointment> InsertAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }
    }
}
