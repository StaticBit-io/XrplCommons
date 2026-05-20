# XrplCommons

[![.NET](https://github.com/StaticBit-io/XrplCommons/actions/workflows/dotnet.test.yml/badge.svg)](https://github.com/StaticBit-io/XrplCommons/actions/workflows/dotnet.test.yml)
[![Documentation](https://github.com/StaticBit-io/XrplCommons/actions/workflows/docs.yml/badge.svg)](https://staticbit-io.github.io/XrplCommons/)
[![License](https://img.shields.io/github/license/StaticBit-io/XrplCommons)](LICENSE)

.NET client library for the [XRPL Commons Map API](https://map.xrpl-commons.org) — explore projects, tools, and protocols built on the XRP Ledger.

## Features

- Typed HTTP client for all XRPL Commons API endpoints
- Sections, categories, and projects with full model mapping
- Dependency injection support via `IServiceCollection`
- `System.Text.Json` serialization with custom converters
- Async/await with `CancellationToken` support
- Blazor WebAssembly demo application

## Installation

```bash
dotnet add package Staticbit.XrplCommons.Client
```

## Quick Start

### Register the client

```csharp
builder.Services.AddXrplCommonsClient();
```

Or with a custom base URL:

```csharp
builder.Services.AddXrplCommonsClient(options =>
{
    options.BaseUrl = "https://map.xrpl-commons.org";
});
```

### Use the client

```csharp
public class MyService
{
    private readonly IXrplCommonsClient _client;

    public MyService(IXrplCommonsClient client)
    {
        _client = client;
    }

    public async Task ListProjectsAsync(CancellationToken cancellationToken)
    {
        List<Project> projects = await _client.GetProjectsAsync(cancellationToken);

        foreach (Project project in projects)
        {
            Console.WriteLine($"{project.Name} — {project.Status}");
        }
    }
}
```

## API Reference

Full API documentation is available at [staticbit-io.github.io/XrplCommons](https://staticbit-io.github.io/XrplCommons/).

### Endpoints

| Method | Description |
|--------|-------------|
| `GetSectionsAsync()` | Get all ecosystem sections |
| `GetSectionAsync(id)` | Get a section by ID |
| `GetCategoriesAsync(sectionId?)` | Get categories, optionally filtered by section |
| `GetProjectsAsync()` | Get all projects |
| `GetProjectBySlugAsync(slug)` | Get a project by its URL slug |

### Models

**Section** — ecosystem section (e.g., "Apps", "Infrastructure")
- `Id`, `Name`, `PrimaryColor`, `CreatedAt`, `UpdatedAt`

**Category** — project category within a section
- `Id`, `Name`, `SectionId`, `CreatedAt`, `UpdatedAt`

**Project** — individual XRPL ecosystem project
- `Id`, `Slug`, `Name`, `Logo`, `Description`
- `CategoryId`, `Programs`, `Status`, `Visible`
- `LaunchDate`, `Tags`, `Url`, `Github`
- `CreatedAt`, `UpdatedAt`

**ProjectStatus** — `Active`, `Inactive`, `PreLaunch`, `Paid`

## Project Structure

```
src/
├── XrplCommons.Client/          # API client library (NuGet package)
├── XrplCommons.Client.Tests/    # Unit & integration tests
├── XrplCommons.Blazor/          # Blazor WebAssembly frontend
└── XrplCommons.Blazor.Server/   # ASP.NET Core host with API proxy
```

## Development

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Build

```bash
dotnet build
```

### Run tests

```bash
# Unit tests
dotnet test

# Integration tests (requires network access)
dotnet test --filter Category=Integration
```

### Run the Blazor demo

```bash
dotnet run --project src/XrplCommons.Blazor.Server/XrplCommons.Blazor.Server.csproj
```

Open [http://localhost:5000](http://localhost:5000) in your browser.

## Documentation

Documentation is built with [DocFX](https://dotnet.github.io/docfx/) and published to GitHub Pages.

### Build docs locally

```bash
dotnet tool install -g docfx
docfx DocFx/docfx.json --serve
```

## License

This project is licensed under the Apache License 2.0 — see the [LICENSE](LICENSE) file for details.

## Links

- [XRPL Commons Map](https://map.xrpl-commons.org)
- [API Documentation](https://staticbit-io.github.io/XrplCommons/)
- [XRP Ledger](https://xrpl.org)
