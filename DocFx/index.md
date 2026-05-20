# XrplCommons

.NET client library for the [XRPL Commons Map API](https://map.xrpl-commons.org).

Provides typed access to the XRPL ecosystem directory — sections, categories, and projects built on the XRP Ledger.

## Packages

| Package | Description |
|---------|-------------|
| **XrplCommons.Client** | HTTP client with DI support, models, and serialization |

## Quick Start

```csharp
// Register in DI container
services.AddXrplCommonsClient();

// Inject and use
public class MyService(IXrplCommonsClient client)
{
    public async Task<List<Project>> GetActiveProjectsAsync(CancellationToken ct)
    {
        List<Project> projects = await client.GetProjectsAsync(ct);
        return projects.Where(p => p.Status == ProjectStatus.Active).ToList();
    }
}
```

## Documentation

- [Getting Started](getting-started.md) — installation and basic usage
- [API Reference](reference/) — full class and interface documentation

## Links

- [GitHub Repository](https://github.com/StaticBit-io/XrplCommons)
- [XRPL Commons Map](https://map.xrpl-commons.org)
- [XRP Ledger](https://xrpl.org)
