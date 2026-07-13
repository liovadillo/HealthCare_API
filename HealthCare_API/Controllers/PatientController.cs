using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAll()
        {
            var patients = await _patientService.GetAllAsync();
            return Ok(patients);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponseDTO<PatientDTO>>> GetByPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var patientsByPage = await _patientService.GetByPage(pageNumber, pageSize);
            return Ok(patientsByPage);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllActive()
        {
            var patients = await _patientService.GetAllActiveAsync();
            return Ok(patients);        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetByID([FromRoute] int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDTO>> InsertPatient([FromBody] CreatePatientDTO objDTO)
        {
            var patient = await _patientService.InsertAsync(objDTO);
            return CreatedAtAction(nameof(GetByID), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDTO>> UpdatePatient([FromRoute] int id, [FromBody] UpdatePatientDTO objDTO)
        {
            var patient = await _patientService.UpdateAsync(id, objDTO);

            return Ok(patient);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient([FromRoute] int id)
        {
            var wasDeleted = await _patientService.DeleteAsync(id);

            return NoContent();
        }
    }
}
