using HealthCare_API.DTOs.Appointment;
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

    }
}
