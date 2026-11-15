# Mini Store Management Backend

Mini Store Management is the backend of a simple store-management project. It is an ASP.NET Core application built with Entity Framework Core and SQL Server.

> **Learning project:** This repository is teaching material. I am using it to teach developers how to build a backend step by step, including data models, relationships, database migrations, and APIs. It is not currently intended for production use.

## What the project contains

The current data model includes:

- Categories
- Products and stock quantities
- Customers
- Orders
- Order items

`OrderItem` connects an order to a product with a composite key. Product prices and order-item unit prices use decimal precision suitable for learning basic store and order management concepts.

The project currently contains the database models, Entity Framework Core context, and an initial migration. API endpoints/controllers can be added as the lessons progress.

## Technology stack

- .NET 11 / ASP.NET Core
- Entity Framework Core 11
- SQL Server
- ASP.NET Core OpenAPI

## Prerequisites

Install the following before running the project:

1. [.NET 11 SDK](https://dotnet.microsoft.com/download)
2. SQL Server LocalDB (included with many Visual Studio installations), or another SQL Server instance
3. The Entity Framework Core CLI tool

Install or update the EF Core CLI tool with:

```powershell
dotnet tool install --global dotnet-ef
```

If it is already installed:

```powershell
dotnet tool update --global dotnet-ef
```

## Setup

1. Clone the repository and enter its directory:

   ```powershell
   git clone <repository-url>
   cd MiniStoreManagement
   ```

2. Restore the project dependencies:

   ```powershell
   dotnet restore
   ```

3. Check the database connection string in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "defaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MiniStore;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

   This default configuration uses SQL Server LocalDB on Windows. If you use a different SQL Server instance, replace the connection string with one for your environment. Do not commit real usernames, passwords, or other secrets.

4. Create or update the database from the included migration:

   ```powershell
   dotnet ef database update
   ```

5. Run the backend:

   ```powershell
   dotnet run
   ```

The development launch profiles use:

- HTTP: `http://localhost:5173`
- HTTPS: `https://localhost:7151`

When running in the Development environment, the OpenAPI document is available at `/openapi/v1.json`.

## Creating new migrations

After changing the entity models or `AppDbContext`, create and apply a migration:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Use a descriptive migration name, for example `AddProductDiscount`.

## Project structure

```text
MiniStoreManagement/
|-- Data/                 # Entity Framework Core database context
|-- Migrations/           # Database schema migrations
|-- Models/               # Store domain entities
|-- Properties/           # Local launch profiles
|-- Program.cs            # Application services and HTTP pipeline
|-- appsettings.json      # Application and database configuration
`-- MiniStoreManagement.csproj
```

## Educational purpose

This backend is intentionally kept small and understandable so developers can follow how its pieces fit together. As the course progresses, it can be expanded with controllers or minimal API endpoints, validation, authentication, service classes, tests, and a frontend client.

Contributions and experiments are welcome, but remember that the repository is primarily learning material rather than a production-ready application.
