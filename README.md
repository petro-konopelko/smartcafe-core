# smartcafe-core

The central orchestration repository for the SmartCafe system. Contains the **.NET Aspire AppHost** that starts and wires together all SmartCafe services for local development.

## What it orchestrates

| Service | Source repo |
|---------|-------------|
| Menu API (`menu-api`) | [smartcafe-menu](https://github.com/petro-konopelko/smartcafe-menu) |
| Menu DB Migrator (`migrator`) | [smartcafe-menu](https://github.com/petro-konopelko/smartcafe-menu) |
| Admin Client (`admin`) | [smartcafe-admin-client](https://github.com/petro-konopelko/smartcafe-admin-client) |
| PostgreSQL + pgAdmin | via Aspire |
| Azurite (Azure Storage Emulator) | via Aspire |

## Getting Started

### 1. Clone all required repositories

The AppHost references the other service projects by relative path, so clone them side by side:

```bash
git clone https://github.com/petro-konopelko/smartcafe-core.git
git clone https://github.com/petro-konopelko/smartcafe-menu.git
git clone https://github.com/petro-konopelko/smartcafe-admin-client.git
```

Expected directory layout:

```
SmartCafe/
├── smartcafe-core/          ← this repo
├── smartcafe-menu/
└── smartcafe-admin-client/
```

### 2. Configure secrets

In the Menu API project (inside `smartcafe-menu`):

```bash
cd ../smartcafe-menu/src/SmartCafe.Menu.API
dotnet user-secrets init
dotnet user-secrets set "postgres-password" "your_postgres_password"
```

### 3. Run the AppHost

```bash
cd smartcafe-core/src/SmartCafe.AppHost
dotnet run
```

This starts all services and the Aspire Dashboard.

### Service URLs

| Service | URL |
|---------|-----|
| Menu API | http://localhost:5000 |
| Swagger UI | http://localhost:5000/swagger |
| Admin Client | http://localhost:4200 |
| Aspire Dashboard | http://localhost:15888 |
| pgAdmin | via Aspire Dashboard |
