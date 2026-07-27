using HealthCare_API.DTOs.Appointment;
using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.Services.Implementations;
using HealthCare_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDetailDTO>> InsertAppointment([FromBody] CreateAppointmentDTO dto)
        {

            var appointment = await _appointmentService.InsertAsync(dto);

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailDTO>> GetAppointmentById([FromRoute] int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            return Ok(appointment);

        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponseDTO<AppointmentDTO>>> GetByPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) { 
            var appointmentsByPage = await _appointmentService.GetByPage(pageNumber, pageSize);
            return Ok(appointmentsByPage);
            
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAll()
        {
            var appointments = await _appointmentService.GetAllAsync();

            return Ok(appointments);
        }

        [HttpGet("doctor/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllByDoctor([FromRoute] int id)
        {
            var appointments = await _appointmentService.GetByDoctorAsync(id);
            return Ok(appointments);
        }

        [HttpGet("patient/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllByPatient([FromRoute] int id)
        {
            var appointments = await _appointmentService.GetByPatientAsync(id);
            return Ok(appointments);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDetailDTO>> UpdateAppointment([FromRoute] int id,[FromBody] UpdateAppointmentDTO dto)
        {
            var appointment = await _appointmentService.UpdateAsync(id, dto);
            
            return Ok(appointment);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<AppointmentDetailDTO>> CancelAppointment([FromRoute] int id)
        {                       
            var appointment = await _appointmentService.UpdateStatusAsync(id, Enums.AppointmentStatus.Cancelled);

            return Ok(appointment);

        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult<AppointmentDetailDTO>> CompleteAppointment([FromRoute] int id)
        {
            var appointment = await _appointmentService.UpdateStatusAsync(id, Enums.AppointmentStatus.Completed);

            return Ok(appointment);

        }

        [HttpPost("{id}/confirm")]
        public async Task<ActionResult<AppointmentDetailDTO>> ConfirmAppointment([FromRoute] int id)
        {
            var appointment = await _appointmentService.UpdateStatusAsync(id, Enums.AppointmentStatus.Confirmed);

            return Ok(appointment);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointment([FromRoute] int id)
        {
            var wasDeleted = await _appointmentService.DeleteAsync(id);
            return NoContent();
        }

    }
}
