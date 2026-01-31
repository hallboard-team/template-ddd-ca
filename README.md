# DDD Clean Architecture Template

Opinionated template for building domain-driven, clean-architecture applications with a .NET backend and an Angular frontend (CSR dashboard + SSR landing).

This repository is intended as a starting point for new projects. Keep it clean, minimal, and easy to adapt.

## Tech stack

- Backend: .NET (Clean Architecture, DDD)
- Frontend: Angular (multi-project workspace with SSR landing)
- Tests: xUnit
- Package manager: pnpm
- Datastores (dev defaults): PostgreSQL, MongoDB

## Repository structure

```
backend/                 .NET solution, projects, tests
client/                  Angular workspace (dashboard, landing, ui, tool)
docs/                    Project documentation
AGENTS.md                Agent guidance for this repo
```

## Quick start

### Prerequisites

- .NET SDK matching `net10.0`
- Node.js + pnpm (`pnpm@10.25.0`)
- PostgreSQL and MongoDB (for development)
- pnpm store should be global (`~/.pnpm-store`); the repo should not contain `.pnpm-store`

### Backend

From repo root:

```bash
dotnet build backend/src/Ca/ca-template.slnx
dotnet run --project backend/src/Ca/Ca.WebApi/Ca.WebApi.csproj
```

Default dev URLs:

- HTTP: `http://localhost:5176`
- HTTPS: `https://localhost:7181`

### Frontend

From `client`:

```bash
pnpm install
pnpm start
```

Other common commands:

```bash
pnpm ng serve dashboard
pnpm ng serve landing
pnpm test
pnpm build
pnpm run serve:ssr:landing
```

Angular dev server defaults to `http://localhost:4200`.

## Configuration

Development settings live in:

- `backend/src/Ca/Ca.WebApi/appsettings.Development.json`

You should override credentials and secrets with environment variables or user secrets in real projects.

## EF Core conventions (Postgres)

- GUID primary keys use client-side GUIDv7 generation by default (single `Guid` PKs without DB defaults).
- Postgres `xmin` is used as a global optimistic concurrency token.
- Opt out of `xmin` by implementing `IAppendOnly` on the domain entity (or add a type to the infrastructure exclusion list).
- Concurrency conflicts return `409 Conflict` with `ProblemDetails` from the Web API middleware.

## Testing

```bash
dotnet test backend/src/Ca/ca-template.slnx
```

```bash
cd client
pnpm test
```

## Creating a new project from this template

- Duplicate the repository or use it as a template in your VCS.
- Rename namespaces, solution files, and project folders to match your product.
- Replace seed data, JWT settings, and connection strings.
- Add/replace modules in the domain and application layers.

## Contributing

- Keep architecture boundaries strict (Domain has no infrastructure dependencies).
- Add tests for new behavior.
- Prefer small, isolated commits.

## Security

- Do not commit secrets or credentials.
- Rotate sample secrets when cloning into a real project.

## License

Add your license here when you create a new project from this template.
