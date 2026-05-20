using System.Text.Json;
using XrplCommons.Client.Models;

namespace XrplCommons.Client.Tests.Unit.Models;

public sealed class ProjectDeserializationTests
{
    [Theory]
    [InlineData("\"Active\"", ProjectStatus.Active)]
    [InlineData("\"Inactive\"", ProjectStatus.Inactive)]
    [InlineData("\"Pre-launch\"", ProjectStatus.PreLaunch)]
    public void ProjectStatus_DeserializesFromString(string json, ProjectStatus expected)
    {
        ProjectStatus result = JsonSerializer.Deserialize<ProjectStatus>(json);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ProjectStatus.Active, "\"Active\"")]
    [InlineData(ProjectStatus.Inactive, "\"Inactive\"")]
    [InlineData(ProjectStatus.PreLaunch, "\"Pre-launch\"")]
    public void ProjectStatus_SerializesToString(ProjectStatus status, string expected)
    {
        string result = JsonSerializer.Serialize(status);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Section_DeserializesWithMissingOptionalFields()
    {
        string json = """{"_id":"s1","name":"Apps","primaryColor":"blue","updatedAt":"2025-06-24T10:00:00Z"}""";

        Section? result = JsonSerializer.Deserialize<Section>(json);

        Assert.NotNull(result);
        Assert.Equal("s1", result.Id);
        Assert.Null(result.CreatedAt);
        Assert.Null(result.Version);
    }

    [Fact]
    public void Category_DeserializesCorrectly()
    {
        string json = """
        {"_id":"c1","name":"DEX","sectionId":"s1","createdAt":"2025-06-04T12:10:19.559Z","updatedAt":"2025-06-04T12:10:19.559Z","__v":0}
        """;

        Category? result = JsonSerializer.Deserialize<Category>(json);

        Assert.NotNull(result);
        Assert.Equal("c1", result.Id);
        Assert.Equal("DEX", result.Name);
        Assert.Equal("s1", result.SectionId);
        Assert.Equal(0, result.Version);
    }

    [Fact]
    public void Project_DeserializesGithubAsStringArray()
    {
        string json = """
        {
            "_id":"p1","slug":"test","name":"Test","description":"D","categoryId":"c1",
            "status":"Active","visible":true,"programs":[],"tags":[],
            "github":["https://github.com/org1","https://github.com/org2"]
        }
        """;

        Project? result = JsonSerializer.Deserialize<Project>(json);

        Assert.NotNull(result);
        Assert.Equal(2, result.Github.Count);
        Assert.Equal("https://github.com/org1", result.Github[0]);
        Assert.Equal("https://github.com/org2", result.Github[1]);
    }
}
