# Healthcare Client Angular

Frontend SPA built with Angular 22 for managing healthcare appointments, doctors, and patients. Connects to the [Healthcare REST API](https://github.com/liovadillo/healthcare-api) built with .NET.

## Tech Stack

- **Angular 22**
- **Node.js v24**
- **TypeScript**
- **Bulma CSS**
- **RxJS**

## Features

### Appointments
- List all appointments with status indicator (🟡 Scheduled, 🔵 Confirmed, 🟢 Completed, 🔴 Cancelled)
- View appointment detail
- Create new appointment with doctor and patient selection
- Edit existing appointment
- Delete appointment with confirmation modal
- Status flow management — Confirm, Complete, Cancel with validation

### Doctors
- List all doctors
- View doctor detail
- Create / Edit / Delete doctor

### Patients
- List all patients
- View patient detail
- Create / Edit / Delete patient

## Architecture

### Smart / Dumb Component Pattern
- **Smart (Container)** — handles data fetching and business logic
- **Dumb (Presentational)** — receives data via `@Input()` and emits events via `@Output()`

### Project Structure

```
src/app/
├── components/
│   ├── appointments-container/     ← Smart
│   ├── appointment-list/           ← Dumb
│   ├── appointment-detail/         ← Smart
│   ├── appointment-form-container/ ← Smart
│   ├── appointment-form/           ← Smart (form logic)
│   ├── doctors-container/          ← Smart
│   ├── doctor-list/                ← Dumb
│   ├── doctor-detail/              ← Smart
│   ├── doctor-form-container/      ← Smart
│   ├── doctor-form/                ← Smart (form logic)
│   ├── patient-container/          ← Smart
│   ├── patient-list/               ← Dumb
│   ├── patient-detail/             ← Smart
│   ├── patient-form-container/     ← Smart
│   ├── patient-form/               ← Smart (form logic)
│   └── nav-sidebar/                ← Navigation
├── models/
│   ├── appointment/
│   ├── doctor/
│   └── patient/
├── services/
│   ├── appointment/
│   ├── doctor/
│   └── patient/
└── resolvers/
    └── appointment/
```

## Key Concepts Used

- **Reactive Forms** — `FormBuilder`, `FormGroup`, `Validators`
- **AsyncPipe** — handling Observables in templates without manual `subscribe`
- **Route Resolvers** — preloading data before component renders
- **forkJoin** — parallel HTTP calls
- **RouterLink / RouterLinkActive** — navigation
- **`@Input()` / `@Output()` / `EventEmitter`** — component communication
- **`ngOnChanges`** — reacting to `@Input()` changes
- **Query Params** — success/delete notifications

## Routes

```
/appointments              → Appointments list
/appointments/create       → Create appointment
/appointments/:id          → Appointment detail
/appointments/:id/edit     → Edit appointment

/doctors                   → Doctors list
/doctors/create            → Create doctor
/doctors/:id               → Doctor detail
/doctors/:id/edit          → Edit doctor

/patients                  → Patients list
/patients/create           → Create patient
/patients/:id              → Patient detail
/patients/:id/edit         → Edit patient
```

## Getting Started

### Prerequisites
- Node.js v24+
- Angular CLI 22
- [Healthcare REST API](https://github.com/liovadillo/healthcare-api) running locally

### Installation

```bash
git clone https://github.com/liovadillo/healthcare-client-angular.git
cd healthcare-client-angular
npm install
```

### Environment Setup

Update `src/environments/environment.development.ts` with your API URL:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7183/api'
};
```

### Run

```bash
ng serve
```

App runs at `http://localhost:4200`

## Related Project

- [Healthcare REST API (.NET)](https://github.com/liovadillo/healthcare-api)
