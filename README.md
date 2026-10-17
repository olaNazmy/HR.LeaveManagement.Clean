# HR Leave Management System

A RESTful API for managing employee leave types, allocations, and leave requests with an approval workflow. Built with **.NET 8** using **Clean Architecture** and **CQRS**.

## Tech Stack

- **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core 8, SQL Server
- **Architecture:** Clean Architecture, CQRS, MediatR, Repository Pattern
- **Validation & Mapping:** FluentValidation, AutoMapper
- **Email:** SendGrid
- **Testing:** xUnit, Moq, Shouldly, EF Core InMemory
- **Docs:** Swagger

## Project Structure

| Project | Responsibility |
|---|---|
| `Domain` | Core entities |
| `Application` | Commands, queries, handlers, validators, DTOs, and interfaces |
| `Infrastructure` | SendGrid email service and logging |
| `Persistence` | EF Core DbContext, migrations, and repositories |
| `Api` | Controllers, global exception handling, Swagger |
| `UnitTests` / `IntegrationTests` | Handler unit tests and database integration tests |

## Key Features

- **CQRS with MediatR:** every use case is a separate command or query with its own handler; controllers stay thin.
- **Validation pipeline:** a MediatR `ValidationBehavior` runs FluentValidation validators before every handler and returns all errors at once.
- **Global exception handling:** a .NET 8 `IExceptionHandler` returns consistent `ProblemDetails` responses (400, 404, 500).
- **Email notifications:** SendGrid emails on leave request creation, update, cancellation, and approval.
- **Testing:** unit tests with Moq and Shouldly, plus integration tests with EF Core InMemory.

## Getting Started

1. Update the connection string in `HR.LeaveManagement.Api/appsettings.json`.
2. Apply migrations:
   ```bash
   dotnet ef database update --project HR.LeaveManagement.Persistence --startup-project HR.LeaveManagement.Api
   ```
3. Run the API and open `/swagger`:
   ```bash
   dotnet run --project HR.LeaveManagement.Api
   ```
4. Run tests:
   ```bash
   dotnet test
   ```

## Author

**Ola Nazmy Mohamed**, .NET Full Stack Developer
[LinkedIn](https://linkedin.com/in/ola-nazmy-419235128) · [GitHub](https://github.com/olaNazmy)
