using Microsoft.AspNetCore.Components;
using XrplCommons.Client.Models;

namespace XrplCommons.Blazor.Shared;

public partial class ProjectCard : ComponentBase
{
    [Parameter, EditorRequired]
    public Project Project { get; set; } = default!;
}
