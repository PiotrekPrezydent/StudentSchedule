# PiotrekPrezydent/StudentSchedule
Overview
--------

Relevant source files

*   [StudentScheduleBackend/Context.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs)
*   [StudentScheduleBackend/Seeder.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Seeder.cs)
*   [StudentScheduleClient/AdminPages/AdminEntityPage.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/AdminPages/AdminEntityPage.cs)
*   [StudentScheduleClient/App.xaml.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/App.xaml.cs)
*   [StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs)

Purpose and Scope
-----------------

This document provides a comprehensive overview of the StudentSchedule repository, a desktop-based student scheduling management system. The system enables educational institutions to manage student enrollments, academic programs, class schedules, and facility assignments through a WPF desktop application with role-based access control.

The system implements a three-tier architecture consisting of a WPF client application, Entity Framework Core-based backend services, and SQL Server database. For detailed information about specific components: authentication mechanisms are covered in [Authentication & Authorization](https://deepwiki.com/PiotrekPrezydent/StudentSchedule/3-authentication-and-authorization), the data model structure is detailed in [Data Model](https://deepwiki.com/PiotrekPrezydent/StudentSchedule/4-data-model), and administrative interface functionality is explained in [Admin Interface](https://deepwiki.com/PiotrekPrezydent/StudentSchedule/5-admin-interface).

System Architecture
-------------------

The StudentSchedule system follows a modular architecture with clear separation of concerns across presentation, business logic, and data access layers.

### High-Level Component Architecturee

## Client Tier

### App.xaml.cs
- Application entry point

### LoginWindow
- Authentication UI

### StudentWindow
- Student schedule view

### AdminWindow
- Administrative interface

## Business Tier (Backend)

### Repository
- CRUD operations for Gerente

### Sender.cs
- Database initialization

### CUItoolsCommand Line
- Standard command line tools
- Database backup tool

### Content.cs
- DbContext Singleton

## Data Tier

### SQL Server
- EF Core Migrations
- StudentSchedule Database


Sources: [StudentScheduleClient/App.xaml.cs1-59](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/App.xaml.cs#L1-L59) [StudentScheduleBackend/Context.cs1-236](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs#L1-L236) [StudentScheduleBackend/Seeder.cs1-173](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Seeder.cs#L1-L173)


Core System Components
----------------------

### Database Context and Connection Management

The `Context` class implements a thread-safe singleton pattern for database access with dynamic connection string generation based on user authentication:



* Component: Context.Instance
  * Responsibility: Singleton access point
  * Key Methods: Initialize(), BuildConnectionString()
* Component: Connection String Builder
  * Responsibility: Role-based database access
  * Key Methods: Uses ReadWriteLogin for admins, ReadLogin for students
* Component: Entity Access
  * Responsibility: Type-safe entity retrieval
  * Key Methods: GetEntitiesByType() for reflection-based operations


The system uses separate database credentials for different user roles:

*   `ReadWriteLogin`/`ReadWritePass` for administrative operations
*   `ReadLogin`/`ReadPass` for student read-only access

Sources: [StudentScheduleBackend/Context.cs25-92](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs#L25-L92) [StudentScheduleBackend/Context.cs94-122](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs#L94-L122)

### Generic CRUD Operations

The `AdminEntityPage<T>` class implements a generic pattern for administrative data management:
# AdminEntityPage<T> Class

## Fields
- `Repository<T> _repository`
- `List<KeyValuePair> _filters`

## Methods
- `+ OnEdit()`
- `+ OnDelete()`
- `+ OnAdd()`
- `+ OnFilter()`

---

# Repository<T> Class

## Methods
| Method | Description |
|--------|-------------|
| `+ GetAll()` | Retrieves all entities |
| `+ Add(entity)` | Adds new entity |
| `+ Update(entity)` | Updates existing entity |
| `+ Delete(id)` | Removes entity by ID |

## ShowColumnsPopup Component
- `+ List<KeyValuePair> ReadedValues`
- `+ GenerateColumns(entityType, popupType)`
- `+ SetReadedValues()`

---

# Abstract Entity Class
- `+ int Id` (identifier field)
- `+ CreateFromKVP<T>(kvps)` (factory method that creates entity from KeyValuePairs)


Sources: [StudentScheduleClient/AdminPages/AdminEntityPage.cs10-111](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/AdminPages/AdminEntityPage.cs#L10-L111) [StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs1-211](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs#L1-L211)

### Dynamic UI Generation

The `ShowColumnsPopup` class dynamically generates forms based on entity properties using reflection:


|UI Control Type       |Property Type       |Generated Control                  |
|----------------------|--------------------|-----------------------------------|
|Foreign Key Properties|[ForeignKeyOf(Type)]|ComboBox with entity IDs           |
|Boolean Properties    |bool                |CheckBox                           |
|Time Properties       |TimeSpan            |TextBox with time format validation|
|Numeric Properties    |int, double, decimal|TextBox with numeric validation    |
|String Properties     |string              |TextBox                            |


The system determines control types through reflection and custom attributes, enabling automatic form generation for any entity type.

Sources: [StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs68-165](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs#L68-L165)

Data Seeding and Initialization
-------------------------------

### Application Startup Process

In debug mode, the application automatically recreates and seeds the database:

Sources: [StudentScheduleClient/App.xaml.cs23-40](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/App.xaml.cs#L23-L40) [StudentScheduleBackend/Seeder.cs9-48](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Seeder.cs#L9-L48)

### Database User Management

The seeding process creates SQL Server logins and database users with appropriate permissions:

*   `ReadWriteLogin` receives `db_owner` role for full database access
*   `ReadLogin` receives `db_datareader` role for read-only access

Sources: [StudentScheduleBackend/Seeder.cs20-47](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Seeder.cs#L20-L47)

Key Architectural Patterns
--------------------------

### Repository Pattern Implementation

The system implements a generic repository pattern where `Repository<T>` provides consistent CRUD operations for any entity type inheriting from the `Entity` base class.

### Singleton Database Context

The `Context` class uses a thread-safe singleton pattern with lazy initialization to ensure single database connection management across the application.

### Dynamic Form Generation

The UI layer uses reflection to automatically generate appropriate input controls based on entity property types and custom attributes, eliminating the need for hand-coded forms for each entity type.

### Role-Based Data Access

Authentication determines database connection permissions, with admin users receiving full access and students receiving read-only access at the database level.

Sources: [StudentScheduleBackend/Context.cs25-92](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs#L25-L92) [StudentScheduleClient/AdminPages/AdminEntityPage.cs10-20](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/AdminPages/AdminEntityPage.cs#L10-L20) [StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs68-165](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs#L68-L165)
