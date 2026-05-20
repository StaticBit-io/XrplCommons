using XrplCommons.Client.Models;

namespace XrplCommons.Client.Tests.Integration;

[Trait("Category", "Integration")]
public sealed class XrplCommonsClientIntegrationTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly XrplCommonsClient _client;

    public XrplCommonsClientIntegrationTests()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://map.xrpl-commons.org")
        };
        _client = new XrplCommonsClient(_httpClient);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    [Fact]
    public async Task GetSectionsAsync_ReturnsNonEmptyList()
    {
        List<Section> sections = await _client.GetSectionsAsync();

        Assert.NotEmpty(sections);
        Assert.True(sections.Count >= 5, $"Expected at least 5 sections, got {sections.Count}");
        Assert.All(sections, section =>
        {
            Assert.False(string.IsNullOrWhiteSpace(section.Id));
            Assert.False(string.IsNullOrWhiteSpace(section.Name));
        });
    }

    [Fact]
    public async Task GetSectionAsync_WithKnownId_ReturnsAppsSection()
    {
        Section section = await _client.GetSectionAsync("67656d692664a9eebf468fc1");

        Assert.Equal("Apps", section.Name);
        Assert.False(string.IsNullOrWhiteSpace(section.PrimaryColor));
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsCategories()
    {
        List<Category> categories = await _client.GetCategoriesAsync();

        Assert.NotEmpty(categories);
        Assert.True(categories.Count >= 50, $"Expected at least 50 categories, got {categories.Count}");
    }

    [Fact]
    public async Task GetCategoriesAsync_WithSectionId_ReturnsResults()
    {
        string appsSectionId = "67656d692664a9eebf468fc1";

        List<Category> categories = await _client.GetCategoriesAsync(appsSectionId);

        Assert.NotEmpty(categories);
        Assert.Contains(categories, category => category.SectionId == appsSectionId);
    }

    [Fact]
    public async Task GetProjectsAsync_ReturnsProjects()
    {
        List<Project> projects = await _client.GetProjectsAsync();

        Assert.NotEmpty(projects);
        Assert.True(projects.Count >= 100, $"Expected at least 100 projects, got {projects.Count}");
        Assert.All(projects, project =>
        {
            Assert.False(string.IsNullOrWhiteSpace(project.Id));
            Assert.False(string.IsNullOrWhiteSpace(project.Slug));
            Assert.False(string.IsNullOrWhiteSpace(project.Name));
        });
    }

    [Fact]
    public async Task GetProjectBySlugAsync_WithKnownSlug_ReturnsXrplFoundation()
    {
        Project project = await _client.GetProjectBySlugAsync("xrp-ledger-foundation");

        Assert.Equal("XRP Ledger Foundation", project.Name);
        Assert.Equal(ProjectStatus.Active, project.Status);
        Assert.True(project.Visible);
        Assert.False(string.IsNullOrWhiteSpace(project.Description));
        Assert.False(string.IsNullOrWhiteSpace(project.Url));
    }

    [Fact]
    public async Task GetProjectBySlugAsync_WithUnknownSlug_ThrowsHttpRequestException()
    {
        await Assert.ThrowsAsync<HttpRequestException>(
            () => _client.GetProjectBySlugAsync("this-project-does-not-exist-12345"));
    }
}
