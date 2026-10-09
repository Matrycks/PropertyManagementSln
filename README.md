# Client

An ASP.NET Core MVC application for managing rental properties, floor plans, and rental applications. The app includes ASP.NET Core Identity with role-based access for manager features.

## Requirements

- .NET 10 SDK

## Run the application

From the solution root:

```sh
dotnet run --project Client/Client.csproj
```

The configured launch profiles use `http://localhost:5258` and `https://localhost:7134`. On first startup, the app seeds sample properties and floor plans, plus Identity roles and a manager account. Data is stored in an Entity Framework Core in-memory database and is reset when the application restarts.

### Run from VS Code

Open the solution folder in VS Code, select a configuration from the Run and Debug menu, and press **F5**:

- **Client (MVC)** builds the solution, starts the web app under the debugger, and opens it in a browser.
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
