using Microsoft.AspNetCore.Components;
using XrplCommons.Client.Models;

namespace XrplCommons.Blazor.Shared;

public partial class StatusBadge : ComponentBase
{
    [Parameter, EditorRequired]
    public ProjectStatus Status { get; set; }
}
