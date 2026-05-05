# MtgInventoryManagement

Magic: The Gathering card inventory tracker and collection manager.

## Tech Stack

- **Language:** C# / .NET 10.0
- **Web Framework:** ASP.NET Core
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL 16
- **Containerization:** Docker / Docker Compose

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- Docker & Docker Compose

### Run with Docker Compose (Recommended)

Starts PostgreSQL, pgAdmin, and the API backend:

```bash
docker compose up --build
```

| Service    | URL / Port              |
|------------|-------------------------|
| API        | http://localhost:8080   |
| pgAdmin    | http://localhost:5050   |
| PostgreSQL | Internal only (no host port) |

### Run Locally (Development)

```bash
cd MtgInventoryManagementApi/MtgInventoryManagement.Api
dotnet run
```

| Profile | URL                        |
|---------|----------------------------|
| HTTP    | http://localhost:5174      |
| HTTPS   | https://localhost:7296     |

### Build & Publish

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Publish (Release)
dotnet publish -c Release
```

## Database

The project uses Entity Framework Core with PostgreSQL. The data layer lives in the `MtgInventoryManagement.Data` project; the API project (`MtgInventoryManagement.Api`) provides configuration and connection strings.

### Apply Migrations

```bash
dotnet ef database update \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>
```

### Create a New Migration

```bash
dotnet ef migrations add <MigrationName> \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>
```

### Build / Update the Model Snapshot

EF Core automatically generates a model snapshot file (`<DbContextName>ModelSnapshot.cs`) inside the `Migrations/` folder whenever you create or remove a migration. This snapshot represents the current state of your `DbContext` model and is used by EF Core to compute diffs for future migrations.

You rarely need to interact with the snapshot directly. Instead, manage it through these commands:

#### Create a migration (generates or updates the snapshot)

```bash
dotnet ef migrations add <MigrationName> \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>
```

#### Remove the last migration (reverts the snapshot)

```bash
dotnet ef migrations remove \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>
```

#### Rebuild the snapshot from scratch

If the snapshot becomes corrupted or out of sync with your model, delete the `Migrations/` folder contents (or at least the snapshot file) and regenerate it:

```bash
# Remove the last migration and its snapshot entry
dotnet ef migrations remove \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>

# Recreate a fresh migration (this rebuilds the snapshot)
dotnet ef migrations add InitialCreate \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>
```

#### Drop the database (reset everything)

```bash
dotnet ef database drop \
    --project <path-to-data-project.csproj> \
    --startup-project <path-to-api-project.csproj>
```

### Auto-Migrate on Startup

Pending migrations are applied automatically when the application starts via `dbContext.Database.Migrate()` in the `AddDatabase()` service extension.

### Configuration

Set the connection string in `appsettings.Development.json` or use the `.env` file for Docker Compose. The expected connection string key is `DatabaseConnection`.
