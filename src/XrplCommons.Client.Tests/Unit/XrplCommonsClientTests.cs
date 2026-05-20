using System.Net;
using XrplCommons.Client.Models;
using XrplCommons.Client.Tests.Unit.Helpers;

namespace XrplCommons.Client.Tests.Unit;

public sealed class XrplCommonsClientTests
{
    private static XrplCommonsClient CreateClient(MockHttpMessageHandler handler)
    {
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("https://map.xrpl-commons.org")
        };
        return new XrplCommonsClient(httpClient);
    }

    [Fact]
    public async Task GetSectionsAsync_ReturnsDeserializedSections()
    {
        string json = """
        [
            {"_id":"id1","name":"Apps","primaryColor":"rgba(31,135,232,0.8)","updatedAt":"2025-06-24T10:00:41.937Z"},
            {"_id":"id2","name":"Protocols","primaryColor":"rgba(165,133,219,0.8)","createdAt":"2025-06-04T11:51:49.662Z","updatedAt":"2025-06-04T11:51:49.662Z","__v":0}
        ]
        """;

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        List<Section> result = await client.GetSectionsAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("id1", result[0].Id);
        Assert.Equal("Apps", result[0].Name);
        Assert.Null(result[0].CreatedAt);
        Assert.Equal("id2", result[1].Id);
        Assert.NotNull(result[1].CreatedAt);
        Assert.Equal(0, result[1].Version);
    }

    [Fact]
    public async Task GetSectionAsync_AppendsIdToPath()
    {
        string json = """{"_id":"abc123","name":"Apps","primaryColor":"blue"}""";

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        Section result = await client.GetSectionAsync("abc123");

        Assert.Equal("abc123", result.Id);
        Assert.Contains("api/sections/abc123", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetCategoriesAsync_WithSectionId_AppendsQueryParam()
    {
        string json = """[{"_id":"cat1","name":"DEX","sectionId":"sec1"}]""";

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        List<Category> result = await client.GetCategoriesAsync("sec1");

        Assert.Single(result);
        Assert.Contains("sectionId=sec1", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetCategoriesAsync_WithoutSectionId_NoQueryParam()
    {
        string json = """[{"_id":"cat1","name":"DEX","sectionId":"sec1"}]""";

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        await client.GetCategoriesAsync();

        string requestUrl = handler.LastRequest!.RequestUri!.ToString();
        Assert.DoesNotContain("sectionId", requestUrl);
        Assert.EndsWith("api/categories", requestUrl);
    }

    [Fact]
    public async Task GetProjectsAsync_ReturnsAllProjects()
    {
        string json = """
        [
            {"_id":"p1","slug":"proj-1","name":"Project 1","description":"Desc","categoryId":"c1","status":"Active","visible":true,"programs":[],"tags":[],"github":[]},
            {"_id":"p2","slug":"proj-2","name":"Project 2","description":"Desc","categoryId":"c2","status":"Inactive","visible":false,"programs":[],"tags":[],"github":[]}
        ]
        """;

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        List<Project> result = await client.GetProjectsAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("proj-1", result[0].Slug);
        Assert.Equal(ProjectStatus.Active, result[0].Status);
        Assert.Equal(ProjectStatus.Inactive, result[1].Status);
        Assert.False(result[1].Visible);
    }

    [Fact]
    public async Task GetProjectBySlugAsync_ReturnsProjectWithAllFields()
    {
        string json = """
        {
            "_id":"p1",
            "slug":"test-project",
            "name":"Test Project",
            "logo":"https://cdn.example.com/logo.png",
            "hideName":false,
            "description":"A test project",
            "categoryId":"c1",
            "programs":["grants"],
            "status":"Pre-launch",
            "visible":true,
            "launchDate":"2024-01-15T00:00:00.000Z",
            "tags":["defi","xrpl"],
            "url":"https://example.com",
            "github":["https://github.com/test/repo"],
            "createdAt":"2025-06-04T12:46:14.595Z",
            "updatedAt":"2025-06-04T12:46:14.595Z"
        }
        """;

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        Project result = await client.GetProjectBySlugAsync("test-project");

        Assert.Equal("test-project", result.Slug);
        Assert.Equal("Test Project", result.Name);
        Assert.Equal("https://cdn.example.com/logo.png", result.Logo);
        Assert.False(result.HideName);
        Assert.Equal(ProjectStatus.PreLaunch, result.Status);
        Assert.Equal(["grants"], result.Programs);
        Assert.Equal(["defi", "xrpl"], result.Tags);
        Assert.Equal(["https://github.com/test/repo"], result.Github);
        Assert.NotNull(result.LaunchDate);
        Assert.Contains("api/projects/test-project", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetProjectBySlugAsync_HttpError_ThrowsHttpRequestException()
    {
        MockHttpMessageHandler handler = new(HttpStatusCode.NotFound, """{"error":"not found"}""");
        XrplCommonsClient client = CreateClient(handler);

        await Assert.ThrowsAsync<HttpRequestException>(
            () => client.GetProjectBySlugAsync("nonexistent"));
    }

    [Fact]
    public async Task GetProjectBySlugAsync_EmptyOptionalFields_HandledGracefully()
    {
        string json = """
        {
            "_id":"p1",
            "slug":"minimal",
            "name":"Minimal",
            "description":"Desc",
            "categoryId":"c1",
            "status":"Active",
            "visible":true,
            "programs":[],
            "tags":[],
            "github":[]
        }
        """;

        MockHttpMessageHandler handler = new(HttpStatusCode.OK, json);
        XrplCommonsClient client = CreateClient(handler);

        Project result = await client.GetProjectBySlugAsync("minimal");

        Assert.Null(result.Logo);
        Assert.Null(result.Url);
        Assert.Null(result.LaunchDate);
        Assert.Empty(result.Programs);
        Assert.Empty(result.Tags);
        Assert.Empty(result.Github);
    }
}
