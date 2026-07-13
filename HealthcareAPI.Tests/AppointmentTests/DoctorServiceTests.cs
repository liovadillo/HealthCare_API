using AutoMapper;
using HealthCare_API.DTOs.Appointment;
using HealthCare_API.DTOs.Doctor;
using HealthCare_API.DTOs.PaginationDTOs;
using HealthCare_API.Entities;
using HealthCare_API.Exceptions;
using HealthCare_API.Repositories.Interfaces;
using HealthCare_API.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareAPI.Tests.AppointmentTests
{
    [TestFixture]
    public class DoctorServiceTests
    {
        private Mock<IDoctorRepository> _doctorRepoMock;
        private Mock<IMapper> _mapperMock;
        private DoctorService _service;

        [SetUp]
        public void Setup()
        {

            _doctorRepoMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new DoctorService(
                _doctorRepoMock.Object,
                 _mapperMock.Object
            );
        }

        [Test]
        public async Task InsertAsync_HappyPath_CreatesSuccessfully()
        {
            //Arrange
            var dto = new CreateDoctorDTO
            {
                Name = "test",
                Specialty = "test",
                IsAvailable = true,
                YearsOfExperience = 1
            };

            var doctor = new Doctor
            {
                Name = "test",
                Specialty = "test",
                IsAvailable = true,
                YearsOfExperience = 1
            };

            var doctorFromDB = new Doctor
            {
                Id = 1,
                Name = "test",
                Specialty = "test",
                IsAvailable = true,
                YearsOfExperience = 1
            };

            var doctorDTO = new DoctorDTO
            {
                Id = 1,
                Name = "test",
                Specialty = "test",
                IsAvailable = true,
                YearsOfExperience = 1
            };

            _mapperMock.Setup(m => m.Map<Doctor>(dto))
               .Returns(doctor);

            _doctorRepoMock.Setup(r => r.InsertAsync(It.IsAny<Doctor>()))
                .ReturnsAsync(doctorFromDB);

            _mapperMock.Setup(m => m.Map<DoctorDTO>(doctorFromDB))
                .Returns(doctorDTO);

            //Act
            var result = await _service.InsertAsync(dto);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            _doctorRepoMock.Verify(r => r.InsertAsync(It.IsAny<Doctor>()), Times.Once);
        }

        [Test]
        public async Task InsertAsync_WhenNameIsNull_ThrowsBadRequestException()
        {
            //Arrange
            var dto = new CreateDoctorDTO
            {
                Name = null,
                Specialty = "test",
                IsAvailable = true,
                YearsOfExperience = 1
            };

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.InsertAsync(dto));
        }

        [Test]
        public async Task InsertAsync_WhenSpecialityIsNull_ThrowsBadRequestException()
        {
            //Arrange
            var dto = new CreateDoctorDTO
            {
                Name = "Test",
                Specialty = null,
                IsAvailable = true,
                YearsOfExperience = 1
            };

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.InsertAsync(dto));
        }

        [Test]
        public async Task InsertAsync_WhenYearsOfExperienceIsNegative_ThrowsBadRequestException()
        {
            //Arrange
            var dto = new CreateDoctorDTO
            {
                Name = "Test",
                Specialty = "test",
                IsAvailable = true,
                YearsOfExperience = -1
            };

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.InsertAsync(dto));
        }

        [Test]
        public async Task GetAll_HappyPath_ListDoctorDTO()
        {
            //arrange
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1 },
                 new Doctor { Id = 2}
            };

            var doctorsDTO = new List<DoctorDTO>
            {
                new DoctorDTO { Id = 1 },
                 new DoctorDTO { Id = 2}
            };

            _doctorRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(m => m.Map<IEnumerable<DoctorDTO>>(doctors))
                .Returns(doctorsDTO);

            //Act
            var result = await _service.GetAllAsync();
            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo(2));
            _doctorRepoMock.Verify(r => r.GetAllAsync(), Times.Once);

        }

        [Test]
        public async Task GetAllActive_HappyPath_ListDoctorDTO()
        {
            //arrange
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1, IsAvailable = true },
                 new Doctor { Id = 2, IsAvailable = true }
            };

            var doctorsDTO = new List<DoctorDTO>
            {
                new DoctorDTO { Id = 1, IsAvailable = true },
                 new DoctorDTO { Id = 2, IsAvailable = true}
            };

            _doctorRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(m => m.Map<IEnumerable<DoctorDTO>>(doctors))
                .Returns(doctorsDTO);

            //Act
            var result = await _service.GetAllAsync();
            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(result.ElementAt(0).IsAvailable, Is.EqualTo(true));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo(2));
            Assert.That(result.ElementAt(1).IsAvailable, Is.EqualTo(true));
            _doctorRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetDoctorById_HappyPath_DoctorDTO()
        {
            //Arrange
            var id = 1;

            var doctorFromDB = new Doctor
            {
                Id = id,
                Name = "Test"
            };

            var doctorDTO = new DoctorDTO
            {
                Id = doctorFromDB.Id,
                Name = doctorFromDB.Name
            };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(doctorFromDB);

            _mapperMock.Setup(m => m.Map<DoctorDTO>(doctorFromDB))
                .Returns(doctorDTO);

            //Act
            var result = await _service.GetByIdAsync(id);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            _doctorRepoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Test]
        public async Task GetDoctorById_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var id = 1;

            _doctorRepoMock.Setup(r => r.GetByIdAsync(id))
               .ReturnsAsync((Doctor)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(id));
        }

        [Test]
        public async Task UpdateDoctor_HappyPath_DoctorDTO()
        {
            //Arrange
            var id = 1;

            var doctorFromGet = new Doctor
            {
                Id = id,
                Name = "test",
                IsAvailable = true,
                Specialty = "test",
                YearsOfExperience = 1
            };

            var updateDoctorDto = new UpdateDoctorDTO {
                Name = "test",
                IsAvailable = true,
                Specialty = "test2",
                YearsOfExperience = 1
            };

            var doctorDTO = new DoctorDTO
            {
                Id = id,
                Name = "test",
                IsAvailable = true,
                Specialty = "test2",
                YearsOfExperience = 1
            };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(doctorFromGet);

            _mapperMock.Setup(m => m.Map(updateDoctorDto, doctorFromGet))
                .Callback<UpdateDoctorDTO, Doctor>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.IsAvailable = src.IsAvailable;
                    dest.Specialty = src.Specialty;
                    dest.YearsOfExperience = src.YearsOfExperience;
                });

            _doctorRepoMock.Setup(r => r.UpdateAsync(doctorFromGet))
                .ReturnsAsync(doctorFromGet);

            _mapperMock.Setup(m => m.Map<DoctorDTO>(doctorFromGet))
                .Returns(doctorDTO);

            //Act
            var result = await _service.UpdateAsync(id, updateDoctorDto);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Specialty, Is.EqualTo(updateDoctorDto.Specialty));
            _doctorRepoMock.Verify(r => r.UpdateAsync(doctorFromGet), Times.Once);
        }

        [Test]
        public async Task UpdateDoctor_WhenDoctorNotFound_ThrowsNotFoundException() {

            //Arrange
            var id = 1;

            var updateDoctorDto = new UpdateDoctorDTO
            {
                Name = "test",
                IsAvailable = true,
                Specialty = "test2",
                YearsOfExperience = 1
            };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Doctor)null);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(id, updateDoctorDto));
        }

        [Test]
        public async Task DeleteDoctor_HappyPath_ReturnBool()
        {
            //Arrange
            var id = 1;
            var doctorFromGet = new Doctor
            {
                Id = id,
                Name = "test",
                IsAvailable = true,
                Specialty = "test",
                YearsOfExperience = 1
            };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(doctorFromGet);

            

            //Act
            var result = await _service.DeleteAsync(id);

            //Arrange
            Assert.That(result, Is.EqualTo(true));
            _doctorRepoMock.Verify(r => r.DeleteAsync(doctorFromGet), Times.Once);
        }

        [Test]
        public async Task DeleteDoctor_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var id = 1;

            _doctorRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Doctor) null);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(id));
        }

        [Test]
        public async Task GetByPage_HappyPath_PaginationResponseDTO()
        {
            //Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var paginationFromDB = new PaginationResponseDTO<Doctor>
            {
                Data = new List<Doctor>
                {
                    new Doctor{
                        Id = 1,
                        Name = "test"
                    },
                    new Doctor{
                        Id = 2,
                        Name = "test"
                    }
                },
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 1,
                TotalRecords = 2
            };


            var doctorsDTO = new List<DoctorDTO>
                {
                    new DoctorDTO{
                        Id = 1,
                        Name = "test"
                    },
                    new DoctorDTO{
                        Id = 2,
                        Name = "test"
                    }
                };

            _doctorRepoMock.Setup(r => r.GetByPage(pageNumber, pageSize))
                .ReturnsAsync(paginationFromDB);

            _mapperMock.Setup(m => m.Map<IEnumerable<DoctorDTO>>(paginationFromDB.Data))
                .Returns(doctorsDTO);

            //Act
            var results = await _service.GetByPage(pageNumber, pageSize);
            //Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Data, Is.Not.Empty);
            Assert.That(results.Data.Count, Is.EqualTo(2));
            Assert.That(results.PageNumber, Is.EqualTo(pageNumber));
            Assert.That(results.PageSize, Is.EqualTo(pageSize));

            _doctorRepoMock.Verify(r => r.GetByPage(pageNumber, pageSize), Times.Once);
        }

        [Test]
        public async Task GetByPage_WhenPageNumberIsZero_ThrowsBadRequestException()
        {

            //Arrange
            var pageNumber = 0;
            var pageSize = 10;

            //Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.GetByPage(pageNumber, pageSize));

        }

        [Test]
        public async Task GetByPage_WhenPageSizeIsZero_ThrowsBadRequestException()
        {

            //Arrange
            var pageNumber = 10;
            var pageSize = 0;

            //Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.GetByPage(pageNumber, pageSize));

        }


        [Test]
        public async Task GetByPage_WhenPageNumberIsNegative_ThrowsBadRequestException()
        {

            //Arrange
            var pageNumber = -1;
            var pageSize = 10;

            //Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.GetByPage(pageNumber, pageSize));

        }

        [Test]
        public async Task GetByPage_WhenPageSizeIsNegative_ThrowsBadRequestException()
        {

            //Arrange
            var pageNumber = 10;
            var pageSize = -1;

            //Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _service.GetByPage(pageNumber, pageSize));

        }


    }
}
