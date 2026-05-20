using Microsoft.AspNetCore.Components;
using XrplCommons.Client;
using XrplCommons.Client.Models;

namespace XrplCommons.Blazor.Pages;

public partial class ProjectDetail : ComponentBase
{
    [Inject]
    private IXrplCommonsClient Client { get; set; } = default!;

    [Parameter]
    public string Slug { get; set; } = string.Empty;

    private Project? _project;
    private string? _categoryName;
    private bool _isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        try
        {
            _project = await Client.GetProjectBySlugAsync(Slug);

            List<Category> categories = await Client.GetCategoriesAsync();
            _categoryName = categories
                .FirstOrDefault(c => c.Id == _project.CategoryId)?.Name;
        }
        catch
        {
            _project = null;
        }
        finally
        {
            _isLoading = false;
        }
    }
}
