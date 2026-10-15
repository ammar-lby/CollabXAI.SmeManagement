# CollabXAI SME Management

A small ASP.NET Core Web API for managing the SMEs that take part in CollabXAI, a platform where several SMEs train an AI model together while keeping their data in their own environments.

The API lets you register an SME (organisation name and contact email), view them, update them and delete them.

## Plan

- Layered solution: Domain, Application, Infrastructure and Api projects, plus a test project
- Entity Framework Core with SQLite for storage
- Swagger for trying out the API

## How to run

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```
dotnet build
dotnet run --project src/CollabXAI.Api
```

Then open `/swagger` on the address shown in the console to try the API.

To run the tests:

```
dotnet test
```

## Status

Work in progress. The code is being added step by step.
