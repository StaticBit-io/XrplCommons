using Microsoft.AspNetCore.Components;

namespace XrplCommons.Blazor.Shared;

public partial class LoadingSpinner
{
    [Parameter]
    public string? Text { get; set; }
}
