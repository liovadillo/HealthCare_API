using HealthCare_API.DTOs.Doctor;
using HealthCare_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;


        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetById([FromRoute] int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);

        }

        [HttpPost]
        public async Task<ActionResult<DoctorDTO>> InsertDoctor([FromBody] CreateDoctorDTO dto)
        {
            var doctor = await _doctorService.InsertAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorDTO>> UpdateDoctor([FromRoute] int id, [FromBody] UpdateDoctorDTO dto)
        {

            var doctor = await _doctorService.UpdateAsync(id, dto);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor([FromRoute] int id)
        {
            var wasDeleted = await _doctorService.DeleteAsync(id);
            if (!wasDeleted)
                return NotFound();

            return NoContent();

        }


    }
}
