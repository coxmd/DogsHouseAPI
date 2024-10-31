
# Dogs House API

A RESTful API service for managing a dog house database, built with ASP.NET Core and Entity Framework Core.

## Project Structure

The solution follows a clean architecture pattern with the following projects:

- **DogsHouse.API:** Main API project containing controllers and middleware
- **DogsHouse.Core:** Core business logic, interfaces, and domain models
- **DogsHouse.Infrastructure:** Data access layer and implementations
- **DogsHouse.Tests:** Unit tests for the application

## Features

-  Dog management (create, query)
-  Sorting and pagination support
-  Rate limiting
-  Input validation
-  Error handling
-  Comprehensive unit tests

## Prerequisites

    1.  .NET 7.0 SDK or later

    2.  PostgreSQL or SQL Server

    3.  Visual Studio 2022 or VS Code

## Database Configuration
The application supports both PostgreSQL and SQL Server. Update the connection string in appsettings.json:

## For PostgreSQL:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=dogshouse;Username=your_username;Password=your_password;",
  },
  "DatabaseProvider": "PostgreSQL"
}
```

## For For SQL Server::

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=dogshouse;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
  "DatabaseProvider": "SQLServer"
}
```

Update the database provider in Program.cs:


## For PostgreSQL:

```
builder.Services.AddDbContext<DogsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## For SQL Server::

```
builder.Services.AddDbContext<DogsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Setting Up the Database

1. Install the EF Core CLI tools:

```
dotnet tool install --global dotnet-ef
```

2.  Add the initial migration:
```
dotnet ef migrations add InitialCreate --project DogsHouse.Infrastructure --startup-project DogsHouse.API
```

2.  Update the database:
```
dotnet ef database update --project DogsHouse.Infrastructure --startup-project DogsHouse.API
```

## Rate Limiting Configuration

Configure the rate limiting in appsettings.json:

```
"RateLimiting": {
  "PermitLimit": 10, // e.g For example, to allow 10 requests per 30 seconds:
  "Window": 30,
  "QueueLimit": 0
}
```

## Running the Application

    1.  Clone the repository
    2.  Set up the database as described above
    3.  Run the application:

The API will be available at either:

1.  HTTP: http://localhost:5250
2.  HTTPS: https://localhost:7155

## Running Tests

```
dotnet test DogsHouse.Tests
```

