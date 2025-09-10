# Backend Setup

This project follows a **Domain-Driven Design (DDD)** approach with **CQRS pattern** and uses **Entity Framework Core** for database access. The backend is an ASP.NET Core Web API project.

## Prerequisites
- Visual Studio 2022 (with .NET 8.0 workload)
- SQL Server Express LocalDB (default with VS 2022)
  - Ensure to select the **SQL Server Express 2019 LocalDB** checkbox under the "_Optional_" section when installing VS 2022.

  **NOTE**: Make sure the NuGet package sources are correctly set up.
  - Go to Tools → NuGet Package Manager → Package Manager Settings → Package Sources.
  - You should see at least these two sources:
    - nuget.org
    - Microsoft Visual Studio Offline Packages

This ensures that all required packages (like EF Core, NSwagger, etc.) can be restored without errors.

## Database Setup
The project uses LocalDB for local development. By default, you can use the standard LocalDB.

### Steps:
- Ensure LocalDB is installed. Visual Studio 2022 typically installs it automatically `(localdb)\MSSQLLocalDB`
- Get the connection string using one of the options below.
  - Click View --> SQL Server Object Explorer --> Expand SQL Server --> Right click `(localdb)\MSSQLLocalDB` and check _Properties_. Copy the connection string from _Properties_ section.
  - Copy the connection string below
  ```
  Server=(localdb)\\MSSQLLocalDB;Database=DtsDb;Trusted_Connection=True;
  ```
- Add the copied connection string in the _**appsettings.Development.json**_ file under API project.
```
{
  "DatabaseSettings": {
    "ConnectionString": "copied string"
  }
}
```
- Apply EF Core migrations to create the database schema:
  #### Initial Database Setup
  - Navigate to the **Infrastructure** folder and run
  ```bash
  dotnet ef database update --context DtsDbContext
  ```
  This applies the already committed `InitialCreate` migration, creating the database and tables.

  #### Updating the Database Schema
  - When adding new properties or changing tables:
  ```bash
  dotnet ef migrations add <MigrationName> --context DtsDbContext
  dotnet ef database update --context DtsDbContext
  ```
  The first command generates a migration reflecting your model changes.
  
  The second command applies it to the database.


## Entity Framework Core
- **DbContext**: `DtsDbContext`
- **Repositories** : Implemented for each aggregate root.
- **Unit of Work** : Coordinated through `IUnitOfWork` to ensure consistency.
- **CQRS** : Commands and Queries separate write and read operations.

## NSwagger
- The project uses NSwagger to generate TypeScript API clients for the frontend.
- The necessary packages are already installed. The configuration file for that is `nswag.json` under API folder.
- Whenever the solution is built, the generated Typescript will be placed under `ClientApp\src\app\services` folder.

## API Overview
- **Global Exception Handling**: Uses `GlobalExceptionHandlerMiddleware` to handle exceptions consistently (e.g., 404 for not found, 500 for unexpected errors).
- **DTOs**: All endpoints use DTOs (`TaskItemResponseDto`, `CreateTaskItemCommandDto`, etc.) for data transfer. NSwagger generates TypeScript clients from these DTOs for the frontend.


## Unit Tests
- Located in `Tests.Unit` project.
- Use xUnit and SQLite In-Memory database for repository/unit tests.
  - The in-memory database ensures tests run fast without affecting your local development database.
  - SQLite In-Memory is only for unit tests.
- To run tests, click Test --> Test Explorer --> Right click `Tests.Unit` and select "**Run**"

## Running the solution
- The API project is setup as **Startup** project.
- Click the "**https**" option on the tool bar.
- The web page `https://localhost:<PORT>/swagger/index.html` will be opened documenting all the APIs used.
- The API testing can be done from this page.

# Frontend Setup
The frontend Angular application is located in the `ClientApp` folder. Please refer to [`ClientApp/README.md`](ClientApp/README.md) for detailed instructions on setting up and running the Angular app.

