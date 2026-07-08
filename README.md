# HealthCare REST API

A RESTful API built with ASP.NET Core for managing healthcare appointments, doctors, and patients.

## Tech Stack

- **Framework:** ASP.NET Core (.NET 8)
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Mapping:** AutoMapper
- **Testing:** NUnit + Moq
- **CI/CD:** GitHub Actions
- **Cloud:** Azure App Service

## Architecture

The project follows an N-Layer Architecture:

```
Controllers → Services → Repositories → Database
```

- **Controllers** — Handle HTTP requests and responses
- **Services** — Business logic and validations
- **Repositories** — Data access layer (Repository Pattern)
- **DTOs** — Data Transfer Objects for input/output separation
- **Entities** — EF Core models mapped to database tables

## Features

- Patient CRUD
- Doctor CRUD
- Appointment management with overlap validation
- Appointment status flow (Scheduled → Confirmed → Completed/Cancelled)
- Soft delete via status change
- Global exception handling middleware
- AutoMapper for DTO mapping
- Unit tests with NUnit and Moq (20+ tests)
- CI/CD pipeline with GitHub Actions
- Deployed to Azure App Service

## Endpoints

### Appointments
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/appointments | Get all appointments |
| GET | /api/appointments/{id} | Get appointment by ID |
| GET | /api/appointments/doctor/{doctorId} | Get appointments by doctor |
| GET | /api/appointments/patient/{patientId} | Get appointments by patient |
| POST | /api/appointments | Create appointment |
| PUT | /api/appointments/{id} | Update appointment |
| PATCH | /api/appointments/{id}/cancel | Cancel appointment |
| PATCH | /api/appointments/{id}/confirm | Confirm appointment |
| PATCH | /api/appointments/{id}/complete | Complete appointment |
| DELETE | /api/appointments/{id} | Delete appointment |

### Doctors
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/doctors | Get all doctors |
| GET | /api/doctors/active | Get active doctors |
| GET | /api/doctors/{id} | Get doctor by ID |
| POST | /api/doctors | Create doctor |
| PUT | /api/doctors/{id} | Update doctor |
| DELETE | /api/doctors/{id} | Delete doctor |

### Patients
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/patients | Get all patients |
| GET | /api/patients/active | Get active patients |
| GET | /api/patients/{id} | Get patient by ID |
| POST | /api/patients | Create patient |
| PUT | /api/patients/{id} | Update patient |
| DELETE | /api/patients/{id} | Delete patient |

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository
```bash
git clone https://github.com/liovadillo/HealthCare_API.git
```

2. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HealthCareDB;Trusted_Connection=True;"
}
```

3. Run migrations:
```bash
dotnet ef database update
```

4. Run the project:
```bash
dotnet run
```

## Running Tests

```bash
dotnet test
```

## Status Flow

```
Scheduled → Confirmed → Completed
Scheduled → Cancelled
Confirmed → Cancelled
```

---

Ajusta los endpoints que no coincidan exactamente con los tuyos. 👍
