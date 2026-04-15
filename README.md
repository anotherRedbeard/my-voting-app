# My Voting App

A .NET voting application built with ASP.NET Core Razor Pages.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Getting Started

```bash
# Restore dependencies
dotnet restore MyVotingApp.slnx

# Build the solution
dotnet build MyVotingApp.slnx

# Run the web application
dotnet run --project src/MyVotingApp.Web

# Run the tests
dotnet test MyVotingApp.slnx
```

## Project Structure

```
MyVotingApp.slnx              # Solution file
src/
  MyVotingApp.Web/             # ASP.NET Core Razor Pages web application
    Models/                    # Domain models
    Services/                  # Business logic and service interfaces
    Data/                      # Data access (EF Core, DbContext)
    Pages/                     # Razor Pages
    Program.cs                 # Application entry point
  MyVotingApp.Tests/           # xUnit test project
docs/
  implementation-plan.md       # Implementation roadmap
```

## Planning

The canonical implementation roadmap lives in [docs/implementation-plan.md](docs/implementation-plan.md).

Follow the GitHub issues in numeric order because the issue list is intentionally sequenced by dependency and execution order.

## Working Agreement

- Keep [docs/implementation-plan.md](docs/implementation-plan.md) aligned with the current issue roadmap.
- When work is completed for an issue, update the implementation plan if scope, sequencing, status, or follow-up work changed.
- Do not close an issue without first checking whether the implementation plan needs to be updated.