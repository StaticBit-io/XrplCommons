using System.Text.Json;
using XrplCommons.Client.Models;

namespace XrplCommons.Client;

public sealed class XrplCommonsClient : IXrplCommonsClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public XrplCommonsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Section>> GetSectionsAsync(CancellationToken cancellationToken = default)
    {
        return await SendAndDeserializeAsync<List<Section>>("api/sections", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Section> GetSectionAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SendAndDeserializeAsync<Section>($"api/sections/{Uri.EscapeDataString(id)}", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<Category>> GetCategoriesAsync(string? sectionId = null, CancellationToken cancellationToken = default)
    {
        string path = sectionId is not null
            ? $"api/categories?sectionId={Uri.EscapeDataString(sectionId)}"
            : "api/categories";

        return await SendAndDeserializeAsync<List<Category>>(path, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<Project>> GetProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await SendAndDeserializeAsync<List<Project>>("api/projects", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Project> GetProjectBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await SendAndDeserializeAsync<Project>($"api/projects/{Uri.EscapeDataString(slug)}", cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<T> SendAndDeserializeAsync<T>(string path, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(path, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        T? result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return result ?? throw new JsonException($"Failed to deserialize response from '{path}'");
    }
}
