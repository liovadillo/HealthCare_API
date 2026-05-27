using AutoMapper;
using HealthCare_API.DTOs.Appointment;
using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.Patient;
using HealthCare_API.Entities;
using HealthCare_API.Enums;
using HealthCare_API.Exceptions;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;

namespace HealthcareAPI.Tests.AppointmentTests
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _appointmentRepoMock;
        private Mock<IDoctorRepository> _doctorRepoMock;
        private Mock<IPatientRepository> _patientRepoMock;
        private Mock<IMapper> _mapperMock;
        private AppointmentService _service;

        [SetUp]
        public void Setup()
        {
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(
                _appointmentRepoMock.Object,
                _mapperMock.Object,
                _doctorRepoMock.Object,
                _patientRepoMock.Object
            );
        }

        [Test]
        public async Task InsertAsync_HappyPath_CreatesSuccessfully()
        {
            // Arrange
            var dto = new CreateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            var appointment = new Appointment { Id = 1, DoctorId = 1, PatientId = 1 };
            var appointmentFromDb = new Appointment { Id = 1, DoctorId = 1, PatientId = 1 };
            var detailDto = new AppointmentDetailDTO { Id = 1 };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });

            _patientRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Patient { Id = 1 });

            _appointmentRepoMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Appointment, bool>>>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Appointment>(dto))
                .Returns(appointment);

            _appointmentRepoMock.Setup(r => r.InsertAsync(It.IsAny<Appointment>()))
                .ReturnsAsync(appointment);

            // Mock del GetByIdAsync que llama GetAppointmentDetailsAsync internamente
            _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointmentFromDb);

            _mapperMock.Setup(m => m.Map<AppointmentDetailDTO>(appointmentFromDb))
                .Returns(detailDto);

            // Act
            var result = await _service.InsertAsync(dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            _appointmentRepoMock.Verify(r => r.InsertAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public async Task InsertAsync_WhenOverlapExists_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });

            _patientRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Patient { Id = 1 });

            // ExistsAsync retorna true = hay traslape
            _appointmentRepoMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Appointment, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.InsertAsync(dto));
        }

        [Test]
        public async Task InsertAsync_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new CreateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            // Doctor no existe → GetByIdAsync regresa null
            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            // Act & Assert — todo en uno, sin llamar InsertAsync por separado
            Assert.ThrowsAsync<NotFoundException>(() => _service.InsertAsync(dto));
        }

        [Test]
        public async Task InsertAsync_WhenPatientNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new CreateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            // Doctor existe
            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });
            //Patient no existe, GetByIdAsync() = null
            _patientRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Patient) null);

            // Act & Assert — todo en uno, sin llamar InsertAsync por separado
            Assert.ThrowsAsync<NotFoundException>(() => _service.InsertAsync(dto));
        }

        [Test]
        public async Task UpdateStatusAsync_HappyPath_ReturnsAppointmentDetailDTO()
        {
            // Arrange
            var id = 1;
            var newStatus = AppointmentStatus.Confirmed;
            
            var appointment = new Appointment { Id = 1, DoctorId = 1, PatientId = 1, Status = AppointmentStatus.Scheduled };
            var appointmentFromDb = new Appointment { Id = 1, DoctorId = 1, PatientId = 1, Status = newStatus };
            var detailDto = new AppointmentDetailDTO { Id = 1, Status = newStatus };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(appointment);

            _appointmentRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(appointmentFromDb);

            _mapperMock.Setup(m => m.Map<AppointmentDetailDTO>(It.IsAny<Appointment>()))
            .Returns(detailDto);

            // Act
            var result = await _service.UpdateStatusAsync(id, newStatus);
            // Assert
            Assert.That(result, Is.Not.Null);
            _appointmentRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }
        
        [Test]
        public async Task UpdateStatusAsync_WhenAppointmentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var id = 1;
            var newStatus = AppointmentStatus.Confirmed;

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Appointment)null);

            // Act & Assert — todo en uno, sin llamar InsertAsync por separado
            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateStatusAsync(id, newStatus));
        }

        [Test]
        public async Task UpdateStatusAsync_WhenInvalidTransition_ThrowsBadRequestException()
        {
            // Arrange
            var id = 1;
            var newStatus = AppointmentStatus.Confirmed;


            var appointment = new Appointment { Id = 1, DoctorId = 1, PatientId = 1, Status = AppointmentStatus.Cancelled };


            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(appointment);

            // Act & Assert — todo en uno, sin llamar InsertAsync por separado
            Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateStatusAsync(id, newStatus));
        }

        [Test]
        public async Task DeleteAppointment_HappyPath_ReturnBool()
        {
            // Arrange
            var id = 1;

            var appointment = new Appointment { Id = id, DoctorId = 1, PatientId = 1, Status = AppointmentStatus.Scheduled };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(appointment);

            // Act
            var result = await _service.DeleteAsync(id);
            // Assert
            Assert.That(result, Is.True);
            _appointmentRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Appointment>()), Times.Once);

        }

        [Test]
        public async Task DeleteAppointment_WhenAppointmentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var id = 1;       

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Appointment) null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(id));
        }

        [Test]
        public async Task UpdateAppointment_HappyPath_AppointmentDetailDTO()
        {

            //Arrage
            var id = 1;
            var dto = new UpdateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            var appointmentFromDB = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            var appointmentDetailDTO = new AppointmentDetailDTO
            {
                Id = 1
            };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
               .ReturnsAsync(appointment);

            _doctorRepoMock.Setup(r => r.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor { Id = 1 });

            _patientRepoMock.Setup(r => r.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient { Id = 1 });

            _appointmentRepoMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Appointment, bool>>>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Appointment>(dto))
                .Returns(appointment);

            _appointmentRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .ReturnsAsync(appointment);

            // Mock del GetByIdAsync que llama GetAppointmentDetailsAsync internamente
            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointment.Id))
                .ReturnsAsync(appointmentFromDB);

            _mapperMock.Setup(m => m.Map<AppointmentDetailDTO>(appointmentFromDB))
                .Returns(appointmentDetailDTO);

            //Act
            var result = await _service.UpdateAsync(id, dto);

            //Assert
            Assert.That(result, Is.Not.Null);
            _appointmentRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public async Task UpdateAppointment_WhenAppointmentNotFound_NotFoundException()
        {
            //Arrage
            var id = 1;
            var dto = new UpdateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Appointment)null);

            // Act & Assert — todo en uno, sin llamar UpdateAsync por separado
            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(id, dto));
        }

        [Test]
        public async Task UpdateAppointment_WhenDoctorNotFound_NotFoundException()
        {
            //Arrage
            var id = 1;
            var dto = new UpdateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(appointment);

            // Doctor no existe → GetByIdAsync regresa null
            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            // Act & Assert — todo en uno, sin llamar InsertAsync por separado
            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(id, dto));

        }

        [Test]
        public async Task UpdateAppointment_PatientNotFound_NotFoundException()
        {
            //Arrange
            var id = 1;
            var dto = new UpdateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(appointment);

            _doctorRepoMock.Setup(r => r.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor { Id = dto.DoctorId });

            _patientRepoMock.Setup(r => r.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient)null);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(id, dto));

        }

        [Test]
        public async Task UpdateAppointment_WhenOverlapExists_BadRequestException()
        {
            //Arrange
            var id = 1;
            var dto = new UpdateAppointmentDTO
            {
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(appointment);

            _doctorRepoMock.Setup(r => r.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor { Id = dto.DoctorId });

            _patientRepoMock.Setup(r => r.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient { Id = dto.PatientId });

            // ExistsAsync retorna true = hay traslape
            _appointmentRepoMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Appointment, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateAsync(id, dto));

        }

        [Test]
        public async Task GetAll_WhenHappyPath_ListAppointmentDTO()
        {

            //Arrange
            var appointments = new List<Appointment>
            {
                new Appointment{ Id=1 },
                new Appointment{ Id=2 },
            };

            var appointmentsDTO = new List<AppointmentDTO>
            {
                new AppointmentDTO{ Id=1 },
                new AppointmentDTO{ Id=2 },
            };

            _appointmentRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<IEnumerable<AppointmentDTO>>(appointments))
                .Returns(appointmentsDTO);
            //Act
            var result = await _service.GetAllAsync();
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            _appointmentRepoMock.Verify(r => r.GetAllAsync(), Times.Once);        
        }

        [Test]
        public async Task GetAppointmentById_HappyPath_AppointmentDetailDTO()
        {

            //Arrange
            var id = 1;
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            var appointmentDetailDTO = new AppointmentDetailDTO
            {
                Id = 1,
                Doctor = new DoctorSummaryDTO { Id = 1, Name = "Test" },
                Patient = new PatientSummaryDTO { Id = 1, Name = "Test" },
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
                Status = AppointmentStatus.Scheduled
            };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(id)).
                ReturnsAsync(appointment);

            _mapperMock.Setup(m => m.Map<AppointmentDetailDTO>(appointment)).
                Returns(appointmentDetailDTO);

            //Act
            var result = await _service.GetByIdAsync(id);

            //Assert
            Assert.That(result, Is.Not.Null);

            _appointmentRepoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Test]
        public async Task GetAppointmentsByDoctor_HappyPath_ListAppointmentDTO()
        {
            
            //Arrange
            var doctorId = 1;
            var doctor = new Doctor { Id = 1 };
            var appointments = new List<Appointment>
            {
                new Appointment{ Id=1 },
                new Appointment{ Id=2 },
            };
            var appointmentsDTO = new List<AppointmentDTO>
            {
                new AppointmentDTO{ Id=1 },
                new AppointmentDTO{ Id=2 },
            };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(doctorId)).
                ReturnsAsync(doctor);

            _appointmentRepoMock.Setup(r => r.GetByDoctorAsync(doctorId)).
                ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<IEnumerable<AppointmentDTO>>(appointments)).
                Returns(appointmentsDTO);

            //Act
            var result = await _service.GetByDoctorAsync(doctorId);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));

            _appointmentRepoMock.Verify(r => r.GetByDoctorAsync(doctorId), Times.Once);

        }

        [Test]
        public async Task GetAppointmentsByDoctor_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var doctorId = 1;

            _doctorRepoMock.Setup(r => r.GetByIdAsync(doctorId)).
                ReturnsAsync((Doctor) null);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.GetByDoctorAsync(doctorId));
        }

        [Test]
        public async Task GetAppointmentsByPatient_HappyPath_ListAppointmentDTO()
        {
            //Arrange
            var patientId = 1;
            var patient = new Patient { Id = patientId };
            var appointments = new List<Appointment>
            {
                new Appointment{ Id=1 },
                new Appointment{ Id=2 },
            };
            var appointmentsDTO = new List<AppointmentDTO>
            {
                new AppointmentDTO{ Id=1 },
                new AppointmentDTO{ Id=2 },
            };

            _patientRepoMock.Setup(r => r.GetByIdAsync(patientId)).
                ReturnsAsync(patient);

            _appointmentRepoMock.Setup(r => r.GetByPatientAsync(patient.Id)).
                ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<IEnumerable<AppointmentDTO>>(appointments)).
                Returns(appointmentsDTO);

            //Act
            var result = await _service.GetByPatientAsync(patientId);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));

            _appointmentRepoMock.Verify(r => r.GetByPatientAsync(patientId), Times.Once);

        }

        [Test]
        public async Task GetAppointmentsByPatient_WhenPatientNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var patientId = 1;

            _patientRepoMock.Setup(r => r.GetByIdAsync(patientId)).
                ReturnsAsync((Patient) null);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.GetByPatientAsync(patientId));

        }

    }
}
