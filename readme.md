# StudentSchedule Solution

A multi-project C# solution for managing student schedules. This solution includes:

- **CLI**: A command-line interface for debugging.
- **StudentScheduleBackend**: An ASP.NET Core Web API that handles data and business logic.
- **StudentScheduleClient**: A WPF desktop client with Admin and Student views.

---

## 📁 Solution Structure
```StudentSchedule.sln
├── CLI/ # Command-line interface for development/debugging
├── StudentScheduleBackend/ # EF API to comunicate with DB
│ ├── Entities/ # Domain models
│ ├── Exceptions/ # Custom exceptions
│ ├── Extensions/ # Extension methods
│ ├── Interfaces/ # Abstractions and contracts
│ ├── Migrations/ # EF Core migrations
│ ├── Repositories/ # Data access logic
│ ├── AppDbContextFactory.cs # Factory for DbContext creation
│ ├── Context.cs # Main EF Core DbContext
│ ├── Exporter.cs # Data export logic
│ └── Seeder.cs # Initial seeding logic
│
├── StudentScheduleClient/ # WPF frontend application
│ ├── AdminPages/ # Views/pages for Admin users
│ ├── StudentPages/ # Views/pages for Student users
│ ├── Windows/ # Main and additional windows
│ ├── App.xaml # Application definition
│ ├── AppSettings.json # Configuration file
```

# 🧰 Project Descriptions

### CLI (CommandLine Project)

A console application for development and debugging purposes.

- Used to test backend functionality and simulate inputs
- Loads configurations from `AppSettings.json`

---

### StudentScheduleBackend (ASP.NET Core API)

Handles business logic, database interaction, and exposes a RESTful API.

**Key Features:**

- JSON-based communication  
- Entity Framework Core for data persistence  
- Clean architecture with separation of concerns

**Main Components:**

- `Entities/` – Model definitions  
- `Repositories/` – Data access layer  
- `Interfaces/` – Abstractions for dependency injection  
- `Extensions/` – Helper/utility extensions  
- `Exceptions/` – Custom error handling  
- `Migrations/` – Database migration history  
- `Seeder.cs` – Initializes default data  
- `Exporter.cs` – Handles data exports  
- `Context.cs` – EF DbContext implementation

---

### StudentScheduleClient (WPF Application)

A desktop GUI for students and administrators.

**Features:**

- Role-based navigation (Admin vs. Student)
- Separate views for each role (`AdminPages/`, `StudentPages/`)
- API communication with backend via `HttpClient`
- Styled using WPF’s MVVM pattern (recommended)

---