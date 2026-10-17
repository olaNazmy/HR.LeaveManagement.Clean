# HR Leave Management System

A RESTful API for managing employee leave — leave types, yearly allocations, and leave requests with an approval workflow. Built with **.NET 8** following **Clean Architecture** and the **CQRS** pattern.

> 🚧 **Status:** Backend API and tests are complete. The Blazor WebAssembly UI is in progress.

---

## 🛠 Tech Stack

| Area | Technologies |
|---|---|
| Framework | .NET 8, ASP.NET Core Web API |
| Architecture | Clean Architecture, CQRS, Repository Pattern |
| Mediator | MediatR (with pipeline behaviors) |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Database | SQL Server, Entity Framework Core 8 (Code First + Migrations) |
| Email | SendGrid |
| Error Handling | `IExceptionHandler` + RFC 7807 `ProblemDetails` |
| Testing | xUnit, Moq, Shouldly, EF Core InMemory |
| API Docs | Swagger / OpenAPI |
| Frontend | Blazor WebAssembly *(in progress)* |

---

## 🏛 Architecture

The solution follows Clean Architecture: dependencies point inward, so the business logic does not depend on the database, email provider, or web framework.

```
            ┌─────────────────────────────┐
            │      API  /  Blazor UI      │   Presentation
            └──────────────┬──────────────┘
                           │
   ┌───────────────────────┼───────────────────────┐
   │  Infrastructure       │        Persistence    │   Infrastructure
   │  (Email, Logging)     │   (EF Core, SQL)      │
   └───────────────────────┼───────────────────────┘
                           │
            ┌──────────────▼──────────────┐
            │         Application         │   Use cases (CQRS)
            └──────────────┬──────────────┘
                           │
            ┌──────────────▼──────────────┐
            │           Domain            │   Entities
            └─────────────────────────────┘
```

| Project | Responsibility |
|---|---|
| `HR.LeaveManagement.Domain` | Core entities (`LeaveType`, `LeaveAllocation`, `LeaveRequest`) and `BaseEntity` with audit fields. No external dependencies. |
| `HR.LeaveManagement.Application` | Business logic: commands, queries, handlers, validators, DTOs, mapping profiles, and contracts (interfaces) for persistence, email, and logging. |
| `HR.LeaveManagement.Infrastructure` | External services: SendGrid email sender and a logging adapter. |
| `HR.LeaveManagement.Persistence` | EF Core `DbContext`, entity configurations, migrations, and generic + specific repositories. |
| `HR.LeaveManagement.Api` | Controllers, global exception handling, Swagger, and dependency injection setup. |
| `HR.LeaveManagement.BlazorUI` | Blazor WebAssembly client *(in progress)*. |
| `HR.LeaveManagement.Application.UnitTests` | Unit tests for command and query handlers. |
| `HR.LeaveManagement.Persistence.IntegrationTests` | Integration tests for the database context. |

---

## ✨ Key Features

### CQRS with MediatR
Every use case is a separate **Command** (write) or **Query** (read) with its own handler, organized by feature:

```
Features/
├── LeaveType/
│   ├── Commands/  CreateLeaveType · UpdateLeaveType · DeleteLeaveType
│   └── Queries/   GetAllLeaveTypes · GetLeaveTypeDetails
├── LeaveAllocation/
│   ├── Commands/  Create · Update · Delete
│   └── Queries/   GetLeaveAllocations · GetLeaveAllocationDetails
└── LeaveRequest/
    ├── Commands/  Create · Update · Cancel · ChangeApproval · Delete
    └── Queries/   GetLeaveRequestList · GetLeaveRequestDetails
```

Controllers stay thin: they only send a request through `IMediator` and return the result.

### Validation Pipeline Behavior
A generic MediatR `ValidationBehavior<TRequest, TResponse>` runs **all FluentValidation validators** for a request before its handler executes. If validation fails, it throws a `BadRequestException` carrying the validation errors — so no handler ever needs to validate input manually.

### Global Exception Handling
A `GlobalExceptionHandler` (implements .NET 8 `IExceptionHandler`) converts exceptions into consistent **`ProblemDetails`** responses:

| Exception | HTTP Status |
|---|---|
| `BadRequestException` | 400 — includes the list of validation errors |
| `NotFoundException` | 404 |
| Anything else | 500 |

Error details and stack traces are only shown in the Development environment.

### Email Notifications (SendGrid)
Emails are sent when a leave request is created, updated, cancelled, or approved/rejected. The Application layer only knows the `IEmailSender` interface; the SendGrid implementation lives in Infrastructure, so the provider can be swapped without touching business logic.

### Logging Abstraction
Handlers log through an `IAppLogger<T>` interface, implemented by a `LoggerAdapter` in Infrastructure.

---

## 📡 API Endpoints

| Resource | Method | Endpoint |
|---|---|---|
| **Leave Types** | GET | `/api/LeaveTypes` |
| | GET | `/api/LeaveTypes/{id}` |
| | POST | `/api/LeaveTypes` |
| | PUT | `/api/LeaveTypes/{id}` |
| | DELETE | `/api/LeaveTypes/{id}` |
| **Leave Allocations** | GET | `/api/LeaveAllocations` |
| | GET | `/api/LeaveAllocations/{id}` |
| | POST | `/api/LeaveAllocations` |
| | PUT | `/api/LeaveAllocations` |
| | DELETE | `/api/LeaveAllocations/{id}` |
| **Leave Requests** | GET | `/api/LeaveRequests` |
| | GET | `/api/LeaveRequests/{id}` |
| | POST | `/api/LeaveRequests` |
| | PUT | `/api/LeaveRequests` |
| | PUT | `/api/LeaveRequests/CancelRequest` |
| | PUT | `/api/LeaveRequests/UpdateApproval` |
| | DELETE | `/api/LeaveRequests/{id}` |

Full interactive documentation is available in Swagger when running locally.

<!-- Add a Swagger screenshot here:
![Swagger UI](docs/swagger.png)
-->

---

## 🧪 Testing

- **Unit tests** — command and query handlers are tested in isolation, with repositories mocked using **Moq** and assertions written with **Shouldly**.
- **Integration tests** — the `HrDatabaseContext` is tested against the **EF Core InMemory** provider (e.g. audit dates are set automatically on save).

Run all tests:

```bash
dotnet test
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express, or Developer edition)

### 1. Clone the repository
```bash
git clone https://github.com/olaNazmy/HR.LeaveManagement.Clean.git
cd HR.LeaveManagement.Clean
```

### 2. Configure the database
Update the connection string in `HR.LeaveManagement.Api/appsettings.json` to point to your SQL Server instance:

```json
"ConnectionStrings": {
  "HrDatabaseConnectionString": "Server=(localdb)\\MSSQLLocalDB;Database=hr_db_LeaveManagement;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### 3. Apply migrations
```bash
dotnet ef database update --project HR.LeaveManagement.Persistence --startup-project HR.LeaveManagement.Api
```

### 4. (Optional) Configure SendGrid
Store your API key with User Secrets so it is never committed:

```bash
cd HR.LeaveManagement.Api
dotnet user-secrets init
dotnet user-secrets set "EmailSettings:ApiKey" "<your-sendgrid-api-key>"
```

### 5. Run the API
```bash
dotnet run --project HR.LeaveManagement.Api
```

Then open `https://localhost:<port>/swagger`.

---

## 🗺 Roadmap

- [x] Clean Architecture solution structure
- [x] CQRS with MediatR
- [x] FluentValidation + validation pipeline behavior
- [x] Global exception handling with ProblemDetails
- [x] SendGrid email service
- [x] Unit and integration tests
- [ ] Blazor WebAssembly UI
- [ ] Authentication & authorization (JWT)

---

## 📚 Acknowledgment

Built while following a [Udemy course on Clean Architecture in ASP.NET Core](https://www.udemy.com/share/104XPk3@Jo2Xs1uuvHFrCy93kLhQfVTFQN54iBu6weQWJKkkpSCyjxZnS-tFd50x6MqWEAWl/), with my own implementation choices along the way.

## 👩‍💻 Author

**Ola Nazmy Mohamed** — .NET Full Stack Developer
[LinkedIn](https://www.linkedin.com/in/YOUR-LINKEDIN) · [GitHub](https://github.com/olaNazmy)
