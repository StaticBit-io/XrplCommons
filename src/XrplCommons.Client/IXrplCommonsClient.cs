using XrplCommons.Client.Models;

namespace XrplCommons.Client;

public interface IXrplCommonsClient
{
    Task<List<Section>> GetSectionsAsync(CancellationToken cancellationToken = default);

    Task<Section> GetSectionAsync(string id, CancellationToken cancellationToken = default);

    Task<List<Category>> GetCategoriesAsync(string? sectionId = null, CancellationToken cancellationToken = default);

    Task<List<Project>> GetProjectsAsync(CancellationToken cancellationToken = default);

    Task<Project> GetProjectBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
