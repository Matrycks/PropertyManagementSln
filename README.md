# Property Management Solution

An ASP.NET Core MVC application for managing rental properties, floor plans, and rental applications. The app uses ASP.NET Core Identity and role-based access for manager features.

## Requirements

- .NET 10 SDK
- SQL Server running locally on port `1433`
- Optional: Docker Desktop if you want to run SQL Server in a container

## Local database setup

This project is configured to use SQL Server via the connection string in [`Client/appsettings.json`](Client/appsettings.json):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=PropertyManagementDb;User Id=SA;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
}
```

If you do not already have SQL Server running locally, start one with Docker:

```sh
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Then ensure the database exists and the schema is applied:

```sh
dotnet ef database update --project Client/Client.csproj
```

If `dotnet ef` is not available, install the EF Core CLI tool first:

```sh
dotnet tool install --global dotnet-ef
```

> Update the password in `Client/appsettings.json` to match your local SQL Server instance if you use a different value.

## Run the application

From the solution root:

```sh
dotnet run --project Client/Client.csproj
```

The configured launch profiles use `http://localhost:5258` and `https://localhost:7134`.

On first startup, the app seeds sample properties, floor plans, Identity roles, and a manager account.

### Run from VS Code

Open the solution folder in VS Code, select a configuration from the Run and Debug menu, and press **F5**:

- **Client (MVC)** builds the solution and starts the web app under the debugger.
- **Client.Tests (run)** runs the xUnit test project in the integrated terminal.

## Build and test

Build the solution:

```sh
dotnet build PropertyManagementSln.slnx
```

Run the xUnit tests:

```sh
dotnet test PropertyManagementSln.slnx
```

The MVC application is in [`Client/`](Client/), and its xUnit test project is in [`Client.Tests/`](Client.Tests/).
