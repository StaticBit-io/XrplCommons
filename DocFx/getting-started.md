# Getting Started

## Installation

Install the NuGet package:

```bash
dotnet add package XrplCommons.Client
```

## Registration

Add the client to your dependency injection container:

```csharp
using XrplCommons.Client.IoC;

builder.Services.AddXrplCommonsClient();
```

### Custom base URL

```csharp
builder.Services.AddXrplCommonsClient(options =>
{
    options.BaseUrl = "https://your-proxy.example.com";
});
```

## Usage

Inject `IXrplCommonsClient` into your services or controllers:

```csharp
using XrplCommons.Client;
using XrplCommons.Client.Models;

public class EcosystemService
{
    private readonly IXrplCommonsClient _client;

    public EcosystemService(IXrplCommonsClient client)
    {
        _client = client;
    }

    public async Task<List<Section>> GetSectionsAsync(CancellationToken ct)
    {
        return await _client.GetSectionsAsync(ct);
    }

    public async Task<List<Category>> GetCategoriesBySectionAsync(
        string sectionId, CancellationToken ct)
    {
        return await _client.GetCategoriesAsync(sectionId, ct);
    }

    public async Task<Project> GetProjectAsync(string slug, CancellationToken ct)
    {
        return await _client.GetProjectBySlugAsync(slug, ct);
    }
}
```

## Available Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `GetSectionsAsync` | `List<Section>` | All ecosystem sections |
| `GetSectionAsync` | `Section` | Section by ID |
| `GetCategoriesAsync` | `List<Category>` | Categories, optionally filtered by section ID |
| `GetProjectsAsync` | `List<Project>` | All projects |
| `GetProjectBySlugAsync` | `Project` | Single project by URL slug |

## Models

### Section

Represents a top-level ecosystem section (e.g., "Apps", "Infrastructure").

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | MongoDB ObjectId |
| `Name` | `string` | Section name |
| `PrimaryColor` | `string` | Hex color for UI display |

### Category

Represents a category within a section.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | MongoDB ObjectId |
| `Name` | `string` | Category name |
| `SectionId` | `string` | Parent section ID |

### Project

Represents an XRPL ecosystem project.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | MongoDB ObjectId |
| `Slug` | `string` | URL-friendly identifier |
| `Name` | `string` | Project name |
| `Description` | `string` | Project description |
| `Status` | `ProjectStatus` | Active, Inactive, PreLaunch, or Paid |
| `Tags` | `List<string>` | Project tags |
| `Url` | `string?` | Project website URL |
| `Github` | `List<string>` | GitHub repository URLs |

### ProjectStatus

```csharp
public enum ProjectStatus
{
    Active,
    Inactive,
    PreLaunch,  // JSON: "Pre-launch"
    Paid
}
```

## Blazor WebAssembly Example

```csharp
// Program.cs
using XrplCommons.Client.IoC;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddXrplCommonsClient(options =>
{
    options.BaseUrl = builder.HostEnvironment.BaseAddress;
});
```

> **Note:** When using Blazor WebAssembly, the API does not provide CORS headers.
> Use an ASP.NET Core server host with an API proxy middleware to forward requests.
