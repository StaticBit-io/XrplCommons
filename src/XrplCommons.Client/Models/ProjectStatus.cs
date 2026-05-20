using System.Text.Json.Serialization;

namespace XrplCommons.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectStatus>))]
public enum ProjectStatus
{
    [JsonStringEnumMemberName("Active")]
    Active,

    [JsonStringEnumMemberName("Inactive")]
    Inactive,

    [JsonStringEnumMemberName("Pre-launch")]
    PreLaunch,

    [JsonStringEnumMemberName("Paid")]
    Paid
}
