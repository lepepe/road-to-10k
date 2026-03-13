# Road to 10K

A personal 16-week running tracker to go from zero to 10K.

**Stack:** C# ASP.NET Core 10 · SQLite · EF Core 10 · React + TypeScript · Vite

---

## Project Structure

```
running-tracker/
├── package.json                        ← root dev scripts (concurrently)
├── backend/
│   └── RunningTracker.API/
│       ├── Controllers/
│       │   ├── SessionsController.cs   ← progress tracking
│       │   └── PlanController.cs       ← plan, phases, schedule
│       ├── Data/AppDbContext.cs         ← EF Core context + seed data
│       ├── Migrations/                 ← EF Core migrations
│       ├── Models/
│       │   ├── Session.cs
│       │   ├── TrainingWeek.cs
│       │   ├── PhaseInfo.cs
│       │   └── ScheduleDay.cs
│       ├── Properties/launchSettings.json
│       └── Program.cs
└── frontend/
    ├── index.html
    └── src/
        ├── api.ts                      ← typed API client
        ├── main.tsx
        └── RunningTracker.tsx
```

---

## Quick Start

From the project root — starts both API and UI simultaneously:

```bash
npm install
npm run dev
```

- API → `http://localhost:5000`
- UI  → `http://localhost:5173`
- API explorer (Scalar) → `http://localhost:5000/scalar/v1`

The UI waits for the API to be ready before launching (no race condition on startup).

---

## Backend

```bash
cd backend/RunningTracker.API
dotnet restore
dotnet run --urls http://localhost:5000
```

On first run, EF Core migrations run automatically and seed the database with the full 16-week plan, phase colors, and weekly schedule template.

### API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET    | `/api/sessions` | Get all completed sessions |
| PUT    | `/api/sessions/{week}/{sessionNum}` | Mark session complete/incomplete |
| DELETE | `/api/sessions/reset` | Wipe all progress |
| GET    | `/api/plan` | Get the full 16-week training plan |
| GET    | `/api/plan/phases` | Get phase colors and labels |
| GET    | `/api/plan/schedule` | Get the weekly schedule template |

### Database

SQLite file (`running_tracker.db`) is auto-created next to the binary on first run. Schema is managed via EF Core migrations — no manual setup needed.

To add a new migration after model changes:

```bash
cd backend/RunningTracker.API
dotnet ef migrations add <MigrationName>
```

---

## Frontend

```bash
cd frontend
npm install
npm run dev
```

All plan data (weeks, phases, schedule) is fetched from the API — nothing is hardcoded in the UI. To change the plan, update the seed data in `AppDbContext.cs` and add a new migration.

---
