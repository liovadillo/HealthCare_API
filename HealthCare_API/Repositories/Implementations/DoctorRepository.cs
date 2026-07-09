using HealthCare_API.Data;
using HealthCare_API.DTOs.PaginationDTOs;
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
        public async Task DeleteAsync(Doctor doctor)
        {
            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAllActiveAsync()
        {
            return await _dbContext.Doctors.Where(d => d.IsAvailable).ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _dbContext.Doctors.ToListAsync();            
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);                         
        }

        public async Task<PaginationResponseDTO<Doctor>> GetByPage(int pageNumber, int pageSize)
        {
            var totalRecords = await _dbContext.Doctors.CountAsync();
            var doctors = await _dbContext.Doctors
                .AsNoTracking()
                .OrderBy(d => d.Id)
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponseDTO<Doctor>
            {
                Data = doctors,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double) totalRecords / pageSize),
                TotalRecords = totalRecords
            };            
        }

        public async Task<Doctor> InsertAsync(Doctor doctor)
        {
            await _dbContext.Doctors.AddAsync(doctor);
            await _dbContext.SaveChangesAsync();

            return doctor;
        }

        public async Task<Doctor?> UpdateAsync(Doctor doctor)
        {

            _dbContext.Doctors.Update(doctor);
            await _dbContext.SaveChangesAsync();

            return doctor;
        }
    }
}
