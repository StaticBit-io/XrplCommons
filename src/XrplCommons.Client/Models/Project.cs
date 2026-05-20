using System.Text.Json.Serialization;
using XrplCommons.Client.Models.Converters;

namespace XrplCommons.Client.Models;

public sealed class Project
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("hideName")]
    public bool HideName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("categoryId")]
    public string CategoryId { get; set; } = string.Empty;

    [JsonPropertyName("programs")]
    public List<string> Programs { get; set; } = [];

    [JsonPropertyName("status")]
    public ProjectStatus Status { get; set; }

    [JsonPropertyName("visible")]
    public bool Visible { get; set; }

    [JsonPropertyName("launchDate")]
    public DateTime? LaunchDate { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = [];

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("github")]
    [JsonConverter(typeof(FlexibleStringListConverter))]
    public List<string> Github { get; set; } = [];

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
