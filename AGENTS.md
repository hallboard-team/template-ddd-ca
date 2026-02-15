## Project overview

- Clean Architecture template with .NET (backend) and Angular (client + SSR landing).
- Backend projects live under `backend/src/Ca`; tests under `backend/tests`.
- Frontend workspace lives in `client` with multiple Angular projects: `dashboard`, `landing`, `ui`, `tool`.
- This repository is a reusable template for future SaaS projects, not a single product implementation.

## Template-first guidance

- Prefer reusable, provider-agnostic patterns that future teams can copy safely.
- Avoid app-specific assumptions, hardcoded business rules, or one-off shortcuts unless explicitly requested.
- When proposing architecture, optimize for secure defaults, clarity, and maintainability across many future projects.
- Add concise comments/docs where a template consumer needs intent, tradeoffs, or extension points.

## Repo layout

- `backend/src/Ca` — solution root (`ca-template.slnx`) and application projects.
- `backend/tests` — xUnit test projects.
- `client` — Angular CLI workspace (pnpm-managed).
- `docs` — documentation (currently empty).

## Tooling / prerequisites

- .NET SDK targeting `net10.0` (see `backend/src/Ca/Directory.Build.props`).
- Node.js (Angular 21) and `pnpm@10.25.0` (see `client/package.json`).
- pnpm store is expected to be global (`~/.pnpm-store`); the repo should not contain `.pnpm-store`.
- Local databases for development:
  - PostgreSQL at `localhost:5432` (`MyPostgresSettings`).
  - MongoDB at `localhost:27017` (`MyMongoDbSettings`).

## Backend commands

From repo root:

```bash
dotnet build backend/src/Ca/ca-template.slnx
dotnet test backend/src/Ca/ca-template.slnx
dotnet run --project backend/src/Ca/Ca.WebApi/Ca.WebApi.csproj
```

Backend URLs (Development):

- HTTP: `http://localhost:5176`
- HTTPS: `https://localhost:7181`

See `backend/src/Ca/Ca.WebApi/Properties/launchSettings.json`.

## Frontend commands

From `client`:

```bash
pnpm install
pnpm start            # ng serve (uses the first project if not specified)
pnpm ng serve dashboard
pnpm ng serve landing
pnpm test
pnpm build
```

SSR landing:

```bash
pnpm build
pnpm run serve:ssr:landing
```

Angular dev server defaults to `http://localhost:4200`.

## Configuration / environment

- `backend/src/Ca/Ca.WebApi/appsettings.Development.json` defines:
  - Postgres and MongoDB connection strings.
  - JWT issuer/audience/key.
  - Super admin seed info.
- Keep secrets out of commits; prefer env overrides for real credentials.

## Integration notes

- Frontend proxy in `client/proxy.conf.json` targets `http://localhost:5000` for `/api`.
  - Backend default is `http://localhost:5176`; update the proxy if you want local API calls to hit the default Web API port.

## Common pitfalls

- Missing `pnpm` or Node versions cause Angular CLI to fail.
- API and client ports may be out of sync (proxy vs launch settings).
- DB services must be running locally for API startup in Development.

## EF Core conventions (Postgres)

- GUID primary keys default to client-side GUIDv7 (single `Guid` PKs without DB defaults).
- Postgres `xmin` is used as a global optimistic concurrency token.
- Opt out of `xmin` by implementing `IAppendOnly` on the domain entity (or add a type to the infra exclusion list).
- Concurrency conflicts are mapped to `409 Conflict` by Web API middleware.
- Tenant isolation is secure-by-default:
  - Entities that implement `ITenantScoped` are filtered globally by current request tenant id.
  - Missing tenant context is fail-closed for tenant-scoped entities (returns no rows).
- Soft-delete is explicit-by-query (do not add a global soft-delete filter in this template).
- Avoid `IgnoreQueryFilters()` in shared query helpers; only use internal/admin-only paths if absolutely required.
