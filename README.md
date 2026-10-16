# CollabXAI SME Management

This is my submission for the CollabXAI technical task: a small ASP.NET Core Web API for managing the SMEs that take part in the platform. You can register an SME with an organisation name and contact email, list them, get one by ID, update it and delete it.

It only stores who the SMEs are. The training data stays with each SME, which is the point of the platform.

## How it's built

I split it into five projects so each one has a single job:

- `CollabXAI.Domain` - the `Sme` entity and the custom exceptions
- `CollabXAI.Application` - the business rules (`SmeService`), the DTOs (data transfer objects, used for requests and responses) and the interfaces
- `CollabXAI.Infrastructure` - Entity Framework Core, the `DbContext` and the repository
- `CollabXAI.Api` - the controller, the error handling middleware and startup
- `CollabXAI.Tests` - the unit tests

A request goes from the controller to the service, then the repository, then the database. The layers only talk to each other through interfaces, and dependency injection joins them up, which also means the service can be tested without a database.

## Running it

You need the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```
dotnet build
dotnet run --project src/CollabXAI.Api
```

Then go to `http://localhost:5111/swagger` to try the endpoints. The SQLite database (`collabxai.db`) gets created the first time the app starts, so there's nothing to set up.

To run the tests:

```
dotnet test
```

## Endpoints

- GET `/api/smes` - list all SMEs
- GET `/api/smes/{id}` - get one SME
- POST `/api/smes` - register an SME
- PUT `/api/smes/{id}` - update an SME
- DELETE `/api/smes/{id}` - delete an SME

`{id}` is the SME's ID (a Guid), for example `/api/smes/1e4c2ff5-cb9e-46ed-af01-60068673a8cf`. The list and register calls act on all SMEs, the others act on the one with that ID.

Bad input returns 400, an unknown ID returns 404, and an email that's already registered returns 409.

## Assumptions

- The server generates the ID (a Guid), so the client doesn't send one.
- Emails are unique. They're trimmed and lowercased before they're saved or compared.
- I only built the SME management part. The local agents mentioned in the brief aren't included.
- There's no authentication, to keep the scope small.

## Packages

- `Microsoft.EntityFrameworkCore.Sqlite` for the database
- `Swashbuckle.AspNetCore` for Swagger
- `xunit`, `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio` and `coverlet.collector` for the tests (all from the standard xUnit template)

## What I'd do next

- Add authentication and roles
- Write integration tests for the real endpoints
- Handle the case where two requests use the same email at the same moment (it currently falls to the database's unique index and returns a 500, not a 409)
- Add pagination to the list endpoint
- Use EF migrations instead of `EnsureCreated`, and SQL Server for deployment
- Add the local agents as a second entity
